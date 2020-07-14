using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Initialize")]
    public Laptop laptop;
    public MobilePhone mobile;

    public List<Transform> enemies;
    public List<GameObject> lamps;

    public GameObject roofEnemy;

    public LevelLoader levelLoader;

    [Space]
    [Header("Cameras")]
    public GameObject mainCamera;
    public GameObject introCamera;

    [Space]
    [Header("GUI")]
    public TextMeshProUGUI screenText;
    public TextMeshProUGUI mobileClock;

    private int _difficulty = 0;

    private bool _gameStarted = false;
    private bool _gameOver = false;

    private Transform _currentEnemy;
    private int _previousEnemy;

    private bool _timerActive = false;
    private int _totalMinutes = 1; //2
    private int _totalTime = 60; //120;
    private int _currentTime = 0;

    private int _currentHour = 23;
    private int _currentMinutes = 58;

    private int _currentLamps = 3;

    private bool _cameraZooming = false;
    private Camera _mainCamera;
    private Quaternion _defaultMCamRotation;

    private AudioSource _breathSound;


    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        _breathSound = GetComponent<AudioSource>();
        _mainCamera = mainCamera.GetComponent<Camera>();

        roofEnemy.SetActive(false);

        DOTween.Init();
    }

    void Start()
    {
        _defaultMCamRotation = mainCamera.transform.rotation;

        ResetEnemies();
        SetLamps();
        UpdateMobileClock();
        //SetRandomEnemy();
        ActivateTimer();
    }

    public void UpdateMobileClock()
    {
        string hour = (_currentHour < 10) ? "0" + _currentHour : _currentHour.ToString();
        string minutes = (_currentMinutes < 10) ? "0" + _currentMinutes : _currentMinutes.ToString();
        mobileClock.text = hour + ":" + minutes;
    }

    void Update()
    {
        if (_gameOver) return;
        if (!_timerActive) return;
        _currentTime++;

        if (_currentTime != 0 && _currentTime % 60 == 0)
        {
            _totalTime--;
            _currentTime = 0;

            if (_totalTime != 0 && _totalTime % 60 == 0)
            {
                _currentMinutes++;

                if (_currentMinutes > 59)
                {
                    _currentHour = 0;
                    _currentMinutes = 0;
                }

                UpdateMobileClock();
            }

            if (_totalTime == 60)
            {
                //Debug.Log("60 - Difficult increased!");
                _difficulty = 1;
            }

            if (_totalTime <= 0) OnTimerEnds();
        }

        /*
        if (Input.GetKeyDown(KeyCode.Q)) {
            OnTimerEnds(); // WIN
        }else if (Input.GetKeyDown(KeyCode.W)) {
            OnAllLampsBroken(); // LOSE
        }
        */

        if (_cameraZooming)
        {
            _mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, 40, 1.5f);

            if (_mainCamera.fieldOfView >= 40f) _cameraZooming = false;
        }

        //screenText.text = _totalTime.ToString();
    }

    private void OnTimerEnds()
    {
        //Debug.Log("Time ends!");
        Debug.Log("Ganaste");
        _gameOver = true;
        _timerActive = false;

        _totalTime = 0;

        MouseLook mouseScript = mainCamera.GetComponent<MouseLook>();
        mouseScript.enabled = false;
        mouseScript.ToggleCursor();

        StartCoroutine("YouWin");
    }

    private IEnumerator YouWin()
    {
        mobile.LightOn();
        HideEnemies();
        RestartLamps();
        yield return new WaitForSeconds(1.5f);
        _breathSound.volume = 0.2f;
        mainCamera.transform.DORotateQuaternion(_defaultMCamRotation, 1.5f);
        _cameraZooming = true;
        mobileClock.text = "00:00";
        yield return new WaitForSeconds(2.0f);
        laptop.RestartLaptop();
        introCamera.SetActive(true);
        introCamera.GetComponent<Animator>().enabled = true;
        IntroCamEnd intro = introCamera.GetComponent<IntroCamEnd>();
        intro.StartWinAnimation();
        roofEnemy.SetActive(true);
        roofEnemy.GetComponent<Animator>().SetTrigger("Idle");
        yield return new WaitForSeconds(11f);
        levelLoader.LoadMenu();
    }

    private void OnLampBroken()
    {
        if (_gameOver) return;
        _currentLamps--;

        if (_currentLamps <= 0)
        {
            OnAllLampsBroken();
        }
    }

    private void OnAllLampsBroken()
    {
        Debug.Log("Perdiste");
        _gameOver = true;
        _timerActive = false;

        _totalTime = 0;

        MouseLook mouseScript = mainCamera.GetComponent<MouseLook>();
        mouseScript.enabled = false;
        mouseScript.ToggleCursor();

        StartCoroutine("YouLose");
    }

    private IEnumerator YouLose()
    {
        mobile.LightOn();
        IntroCamEnd intro = introCamera.GetComponent<IntroCamEnd>();
        mainCamera.transform.DORotateQuaternion(_defaultMCamRotation, 1.5f);
        _cameraZooming = true;
        RestartLamps();
        yield return new WaitForSeconds(1.5f);
        introCamera.SetActive(true);
        introCamera.GetComponent<Animator>().enabled = false;
        _breathSound.volume = 0.2f;
        yield return new WaitForSeconds(2.0f);
        introCamera.GetComponent<Animator>().enabled = true;
        intro.StartLoseAnimation();
        yield return new WaitForSeconds(1f);
        ShowEnemies();
        yield return new WaitForSeconds(5f);
        levelLoader.LoadMenu();
    }

    private void ShowEnemies()
    {
        foreach( Transform enemy in enemies) {
            enemy.gameObject.SetActive(true);
            enemy.GetComponent<MonsterBehaviour>().IsActive = false;
            enemy.GetComponent<Animator>().SetTrigger("Idle");
        }
    }

    private void HideEnemies()
    {
        foreach( Transform enemy in enemies) {
            enemy.gameObject.SetActive(false);
            enemy.GetComponent<MonsterBehaviour>().IsActive = false;
        }
    }

    public void ActivateTimer()
    {
        _timerActive = true;
        _totalTime = 120;
    }

    private void SetLamps()
    {
        foreach( GameObject lamp in lamps) {
            lamp.GetComponent<LampBehaviour>().OnLampBroken -= OnLampBroken;
            lamp.GetComponent<LampBehaviour>().OnLampBroken += OnLampBroken;
        }
    }

    public void RestartLamps()
    {
        foreach( GameObject lamp in lamps) {
            lamp.GetComponent<LampBehaviour>().ResetLamp();
        }
    }

    public void ExplodeLamps()
    {
        foreach( GameObject lamp in lamps) {
            lamp.GetComponent<LampBehaviour>().Explode();
        }
    }

    private void ResetEnemies()
    {
        foreach( Transform enemy in enemies) {
            enemy.gameObject.SetActive(false);
            enemy.GetComponent<MonsterBehaviour>().IsActive = false;
        }
    }

    public void SetRandomEnemy()
    {
        ResetEnemies();
        int enemyRandomIndex = Random.Range(0, enemies.Count);
        while (enemyRandomIndex == _previousEnemy) {
             enemyRandomIndex = Random.Range(0, enemies.Count);
        }

        _currentEnemy = enemies[enemyRandomIndex];
        _currentEnemy.gameObject.SetActive(true);
        _currentEnemy.GetComponent<MonsterBehaviour>().IsActive = true;
        _previousEnemy = enemyRandomIndex;
    }

    public static GameManager Instance {
        get { return instance; }
    }

    public int Difficulty {
        get { return _difficulty; }
    }

    public bool GameIsOver {
        get { return _gameOver; }
    }
}
