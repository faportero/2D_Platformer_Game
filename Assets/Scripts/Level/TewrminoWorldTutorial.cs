using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TewrminoWorldTutorial : MonoBehaviour
{
    [SerializeField] private bool tNivel1, tNivel2, tNivel3;
    [SerializeField] GameObject[] objectsToHide; 
    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }
    private void Start()
    {
        if(levelManager.currentScene == LevelManager.CurrentScene.Nivel1 && UserData.terminoWorldTutorial1)
        {
            if(objectsToHide != null) HideObjects();
            return;
        }
        if (levelManager.currentScene == LevelManager.CurrentScene.Nivel2 && UserData.terminoWorldTutorial2)
        {
            if (objectsToHide != null) HideObjects();
            return;
        }
        if (levelManager.currentScene == LevelManager.CurrentScene.Nivel3 && UserData.terminoWorldTutorial3)
        {
            if (objectsToHide != null) HideObjects();
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UserData.terminoWorldTutorial1 = tNivel1;
            UserData.terminoWorldTutorial2 = tNivel2;
            UserData.terminoWorldTutorial3 = tNivel3;
        }
    }

    private void HideObjects()
    {
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }
    }
}
