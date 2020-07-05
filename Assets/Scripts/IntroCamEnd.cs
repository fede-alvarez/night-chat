using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IntroCamEnd : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject introCamera;
    public GameObject mobilePhone;

    public void CameraZoomsOut()
    {
        mainCamera.GetComponent<MouseLook>().enabled = true;
        introCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        StartCoroutine("StartSpawningEnemies");
    }

    private IEnumerator StartSpawningEnemies()
    {
        yield return new WaitForSeconds(Random.Range(1.0f,1.5f));
        mobilePhone.SetActive(true);
        GameManager.Instance.SetRandomEnemy();
        introCamera.SetActive(false);
    }
}
