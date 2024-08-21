using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminoTutorial : MonoBehaviour
{
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (levelManager.currentScene == LevelManager.CurrentScene.Nivel1)
            {
                UserData.terminoTutorial = true;
            }
            if (levelManager.currentScene == LevelManager.CurrentScene.Nivel2)
            {
                UserData.terminoTutorial2 = true;
            }
            if (levelManager.currentScene == LevelManager.CurrentScene.Nivel3)
            {
                UserData.terminoTutorial3 = true;
            }
        }
    }
}
