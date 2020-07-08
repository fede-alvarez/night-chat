using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : MonoBehaviour
{
    [Header("Main settings")]
    public GameObject introCamera;
    public GameObject screen;
    public List<Texture> screenshots;
    public List<Texture> afterScreenshots;

    [Header("Materials")]
    public Material originalMaterial;
    public Material blackMaterial;
    [Header("GUI")]
    public ClickUI clickUIElement;

    [Header("Sounds")]
    public GameObject keySounds;
    public GameObject popupSound;
    public GameObject clickSound;
    public GameObject laptopOffSound;

    private int _currentScreen = 0;
    private MeshRenderer renderer;

    private float _defaultWait = 5f;
    private float _waitFor = 5f;
    private bool _isWaiting = false;
    private bool _autoSlide = false;

    private bool _isActive = true;

    void Awake()
    {
        renderer = screen.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        PlayAudio("keys");
    }

    private void ResetAudios()
    {
        keySounds.SetActive(false);
        popupSound.SetActive(false);
        clickSound.SetActive(false);
        laptopOffSound.SetActive(false);
    }

    private void PlayAudio(string name)
    {
        GameObject audioGo;
        switch(name)
        {
            case "keys":
                keySounds.SetActive(true);
                break;
            case "popup":
                popupSound.SetActive(true);
                break;
            case "click":
                clickSound.SetActive(true);
                break;
            case "off":
                laptopOffSound.SetActive(true);
                break;
        }
    }

    public void ChangeToScreen(int screenIndex)
    {
        _currentScreen = screenIndex;
        renderer.material.SetTexture("_MainTex", screenshots[_currentScreen]);
    }

    public void ChangeScreenDelayed(int screenIndex)
    {
        _currentScreen = screenIndex;
        StartCoroutine("DelayedScreenChange");
    }

    private IEnumerator DelayedScreenChange()
    {
        yield return new WaitForSeconds(1.0f);
        ChangeToScreen(_currentScreen);
    }

    void Update()
    {
        if (!_isActive) return;

        if (_autoSlide || (Input.anyKey && !_isWaiting))
        {
            clickUIElement.StopAnimation();
            NextScreen();
        }
    }

    private void NextScreen()
    {
        ResetAudios();
        _autoSlide = false;

        _currentScreen++;



        switch(_currentScreen)
        {
            case 1:
                PlayAudio("popup");
                break;
            case 2:
                PlayAudio("click");
                _autoSlide = true;
                break;
            case 3:
                PlayAudio("popup");
                PlayAudio("keys");
                break;
            case 4:
                PlayAudio("click");
                break;
        }

        Animator introCamAnimator = introCamera.GetComponent<Animator>();

        if (_currentScreen < screenshots.Count && !introCamAnimator.GetBool("ComputerIsOver"))
        {
            ChangeToScreen(_currentScreen);

            _isWaiting = true;
            StartCoroutine("StopWaiting");
        } else {
            renderer.material = blackMaterial;
            PlayAudio("off");
            introCamAnimator.SetBool("ComputerIsOver", true);
        }
    }

    public void RestartScreen()
    {
        renderer.material = originalMaterial;
        PlayAudio("popup");
        renderer.material.SetTexture("_MainTex", afterScreenshots[0]);
    }

    private IEnumerator StopWaiting()
    {
        yield return new WaitForSeconds((_autoSlide) ? 1.0f : _waitFor);
        _autoSlide = false;
        _isWaiting = false;
        clickUIElement.StartAnimation();
    }
}
