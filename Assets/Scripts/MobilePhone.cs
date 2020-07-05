using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MobilePhone : MonoBehaviour
{
    [Header("Main settings")]
    public Transform barsContainer;
    public Light phoneLight;
    public GameObject lightSound;

    private int SUBSTRACTION_TIME = 200;
    private int SUB_BAR_VALUE = 10;
    private int DEFAULT_INTENSITY = 15;
    [SerializeField]
    private int _bars = 3;
    [SerializeField]
    private int _subBar = 10;
    private int _barsCount = 0;

    private float _currentIntensity;

    [SerializeField]
    private bool _isActive = true;

    void Start()
    {
        _currentIntensity = phoneLight.intensity;
        lightSound.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _isActive = !_isActive;
            phoneLight.intensity = (_isActive) ? _currentIntensity : 0;
            lightSound.SetActive(true);
            StartCoroutine("LightSoundOff");
        }

        if (!_isActive) return;

        if (_barsCount != 0 && _barsCount % SUBSTRACTION_TIME == 0)
        {
            _barsCount = 0;
            _subBar -= 1;
            if (_subBar <= 0) {
                _bars -= 1;
                //phoneLight.intensity -= 5f;
                _currentIntensity -= 5;
                phoneLight.DOIntensity(_currentIntensity, 0.2f);
                _subBar = SUB_BAR_VALUE;

                UpdateScreen();

                if (_bars <= 0) {
                    _isActive = false;
                }
            }
        }

        _barsCount++;
    }

    private IEnumerator LightSoundOff()
    {
        yield return new WaitForSeconds(0.1f);
        lightSound.SetActive(false);
    }

    private void UpdateScreen()
    {
        int index = 0;
        foreach(Transform bar in barsContainer) {
            bar.gameObject.SetActive((index < _bars) ? true : false);
            index++;
        }
    }

    public bool Active {
        get { return _isActive; }
    }
}
