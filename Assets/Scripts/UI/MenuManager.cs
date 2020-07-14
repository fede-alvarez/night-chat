using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            levelLoader.LoadNextLevel();
    }
}
