using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LampBehaviour : MonoBehaviour
{
    public delegate void LampEvent ();
    public event LampEvent OnLampBroken;

    [Header("Main settings")]
    public Light bulb;
    public GameObject panelOn;
    public GameObject panelOff;

    [Header("Effects")]
    public GameObject explosion;

    private float _defaultIntensity = 2f;
    private float _currentIntensity = 2f;
    [SerializeField]
    private bool _isOn = true;
    [SerializeField]
    private bool _isFlickering = false;

    private int _battery = 10;
    private bool _isAbsorbing = false;
    private int _absorbingCount = 0;
    private int _absorbingTime = 100;

    void Awake()
    {
        panelOn.SetActive(true);
        panelOff.SetActive(false);
    }

    void Update()
    {
        if (!_isOn) return;

        if (_isFlickering)
            bulb.DOIntensity(Random.Range(0.1f, _currentIntensity), 0.1f);

        if (_isAbsorbing)
        {
            if (_absorbingCount != 0 && _absorbingCount % _absorbingTime == 0)
            {
                _absorbingCount = 0;
                _battery -= (GameManager.Instance.Difficulty == 0) ? 1 : 2;
                _currentIntensity -= (GameManager.Instance.Difficulty == 0) ? 0.2f : 0.4f;

                bulb.DOIntensity(_currentIntensity, 0.2f);

                if (_battery <= 0) {
                    _battery = 0;
                    _currentIntensity = 0;
                    _isAbsorbing = false;
                    _isOn = false;
                    panelOn.SetActive(false);
                    panelOff.SetActive(true);
                    explosion.SetActive(true);

                    if (OnLampBroken != null)
                        OnLampBroken();

                    StartCoroutine("SpawnRandomEnemy");
                }
            }

            _absorbingCount++;
        }
    }

    private IEnumerator SpawnRandomEnemy()
    {
        float minRange = 1.0f;
        float maxRange = 2.0f;

        if (GameManager.Instance.Difficulty != 0)
        {
            minRange = 0.5f;
            maxRange = 1.2f;
        }

        yield return new WaitForSeconds(Random.Range(minRange, maxRange));
        GameManager.Instance.SetRandomEnemy();
    }

    public void AbsorbPower()
    {
        _isAbsorbing = true;
    }

    public void StopAbsorbing()
    {
        _isAbsorbing = false;
        _absorbingCount = 0;
    }

    public void ResetLamp()
    {
        _isOn = true;
        _isFlickering = false;

        _battery = 10;
        _isAbsorbing = false;
        _absorbingCount = 0;
        _currentIntensity = _defaultIntensity;
        bulb.DOIntensity(_defaultIntensity, 0.2f);
        explosion.SetActive(false);
    }

    public void Explode()
    {
        explosion.SetActive(true);
        _currentIntensity = 0;
        bulb.DOIntensity(0, 0.1f);
        _battery = 0;
        _isAbsorbing = false;
        _absorbingCount = 0;
    }

    public bool IsOn {
        get { return _isOn; }
    }

    public bool Flicking {
        get { return _isFlickering; }
        set {
            _isFlickering = value;
            if (_isFlickering == false)
            {
                StopAbsorbing();
                bulb.intensity = _currentIntensity;
            }else{
                StopAbsorbing();
                AbsorbPower();
            }
        }
    }
}
