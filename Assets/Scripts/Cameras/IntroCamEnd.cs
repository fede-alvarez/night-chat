using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class IntroCamEnd : MonoBehaviour
{
    [Header("Main")]
    public GameObject mainCamera;
    public GameObject introCamera;
    public GameObject mobilePhone;

    [Header("SFX")]
    public AudioSource breathSound;
    [Header("GUI")]
    public GameObject instructions;

    private Quaternion _defaultRotation;
    private Transform _transform;

    void Awake()
    {
        _transform = transform;
        _defaultRotation = _transform.rotation;
    }

    public void CameraZoomsOut()
    {
        mainCamera.GetComponent<MouseLook>().enabled = true;
        introCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        instructions.SetActive(false);
        StartCoroutine("StartSpawningEnemies");
    }

    private IEnumerator StartSpawningEnemies()
    {
        yield return new WaitForSeconds(Random.Range(1.0f,1.5f));
        mobilePhone.SetActive(true);
        breathSound.volume = 0.5f;
        GameManager.Instance.SetRandomEnemy();
        introCamera.SetActive(false);
    }

    public void StartLoseAnimation()
    {
        introCamera.GetComponent<Animator>().SetTrigger("GameOver");
        introCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
    }

    public void StartWinAnimation()
    {
        introCamera.GetComponent<Animator>().SetTrigger("Win");
        introCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
    }

    public void SetDefaultRotation()
    {
        _transform.DORotateQuaternion(_defaultRotation, 1.5f);
        //CinemachineVirtualCamera cvm = introCamera.GetComponent<CinemachineVirtualCamera>();
        //cvm.m_Lens.FieldOfView = Mathf.Lerp(cvm.m_Lens.FieldOfView, 40, 1.5f);
    }
}
