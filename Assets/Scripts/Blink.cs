using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Transform eyelipTop;
    public Transform eyelipBottom;

    public GameObject directionalLight;

    private bool _isBlinking = false;

    void Awake()
    {
        directionalLight.SetActive(false);

        eyelipTop.gameObject.SetActive(false);
        eyelipBottom.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isBlinking)
        {
            _isBlinking = true;
            eyelipTop.gameObject.SetActive(true);
            eyelipBottom.gameObject.SetActive(true);
            StartCoroutine("CloseEyes");
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool ligthState = !directionalLight.activeInHierarchy;
            directionalLight.SetActive(ligthState);
        }
    }

    private IEnumerator CloseEyes()
    {
        yield return new WaitForSeconds(2f);
        OnBlinkEnds();
    }

    public void OnBlinkEnds()
    {
        Debug.Log("Blinked!");
        _isBlinking = false;
        eyelipTop.gameObject.SetActive(false);
        eyelipBottom.gameObject.SetActive(false);
    }
}
