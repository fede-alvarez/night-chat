using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Main settings")]
    public List<Transform> enemies;
    public List<GameObject> lamps;
    [Space]
    public TextMeshProUGUI screenText;

    private int _batteryLevel = 5;
    private int _difficulty = 0;

    private bool _gameStarted = false;

    private Transform _currentEnemy;
    private int _previousEnemy;

    private static GameManager instance;

    private bool _timerActive = false;
    private int _totalMinutes = 2;
    private int _totalTime = 120;
    private int _currentTime = 0;

    private int _currentLamps = 3;

    void Awake()
    {
        if (instance == null)
            instance = this;

        DOTween.Init();
    }

    void Start()
    {
        ResetEnemies();
        SetLamps();
        //SetRandomEnemy();
    }

    void Update()
    {
        if (!_timerActive) return;
        _currentTime++;

        if (_currentTime != 0 && _currentTime % 60 == 0) {
            _totalTime--;
            _currentTime = 0;

            if (_totalTime <= 0) {
                _totalTime = 0;
                _timerActive = false;
            }
        }

        screenText.text = _totalTime.ToString();
    }

    private void SetLamps()
    {
        foreach( GameObject lamp in lamps) {
            lamp.GetComponent<LampBehaviour>().OnLampBroken -= OnLampBroken;
            lamp.GetComponent<LampBehaviour>().OnLampBroken += OnLampBroken;
        }
    }

    private void OnLampBroken()
    {
        _currentLamps--;

        if (_currentLamps <= 0)
        {
            Debug.Log("Perdiste");
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
}
