using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private PlayerController playerController;
    public GameObject UI_Habilidades;

    private void Awake()
    {
        
    }

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }


    public void GameOver()
    {
        if (playerController != null)
        {
            if (playerController.isDie)
            {
                Time.timeScale = 0;
                
                
                UI_Habilidades.SetActive(true);
            }
        }
    }

    public void ResetLevel()
    {
        UserData.coins = playerController.coinsAmount;
        UserData.escudo = playerController.escudo;
        UserData.saltoDoble = playerController.saltoDoble;
        UserData.vidaExtra = playerController.vidaExtra;
        UserData.paracaidas = playerController.paracaidas;
        SceneManager.LoadScene("Test");
    }

}