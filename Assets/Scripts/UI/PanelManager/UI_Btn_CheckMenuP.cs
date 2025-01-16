using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Btn_CheckMenuP : MonoBehaviour
{
    private LevelManager levelManager;
    private Button btnMenuP;
    private void Awake()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
        btnMenuP = GetComponent<Button>();
    }
    private void Start()
    {
        //if(levelManager!= null  && levelManager.currentScene == LevelManager.CurrentScene.Lobby)
        //{
        //    btnMenuP.interactable = false;
        //}
        //else
        //{
        //    btnMenuP.interactable = true;
        //}
    }

}
