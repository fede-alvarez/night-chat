using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Initialize")]
    public Laptop laptop;

    public List<Transform> enemies;
    public List<GameObject> lamps;

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
    private int _totalMinutes = 2;
    private int _totalTime = 120;
    private int _currentTime = 0;

    private int _currentHour = 23;
    private int _currentMinutes = 58;

    private int _currentLamps = 3;

    private AudioSource _breathSound;


    private static GameManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        _breathSound = GetComponent<AudioSource>();

        DOTween.Init();
    }

    void Start()
    {
        ResetEnemies();
        SetLamps();
        UpdateMobileClock();
        //SetRandomEnemy();
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

            if (_totalTime == 80)
            {
                Debug.Log("80 - Difficult increased!");
                _difficulty = 1;
            }

            if (_totalTime <= 0) OnTimerEnds();
        }

        //screenText.text = _totalTime.ToString();
    }

    private void OnTimerEnds()
    {
        _totalTime = 0;
        _timerActive = false;
        _gameOver = true;
        StartCoroutine("NormalEnding");
    }

    private IEnumerator NormalEnding()
    {
        yield return new WaitForSeconds(1.0f);
        RestartLamps();
        _breathSound.volume = 0.2f;

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

    private void OnLampBroken()
    {
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
}
