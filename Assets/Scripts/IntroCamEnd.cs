using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        GameManager.Instance.ActivateTimer();
        GameManager.Instance.SetRandomEnemy();
        introCamera.SetActive(false);
    }
}
