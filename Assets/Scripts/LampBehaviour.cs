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
    private int _absorbingTime = 120;

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
                _battery -= 1;
                _currentIntensity -= 0.2f;
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
        yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
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
