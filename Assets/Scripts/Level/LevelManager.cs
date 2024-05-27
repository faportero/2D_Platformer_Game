using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public PlayerController playerController;
    [SerializeField] private GameObject UI_Habilidades;
    [SerializeField] private GameObject UI_MensajeHabilidades;
    [SerializeField] private GameObject UI_CurrentEffect;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;

    private void Awake()
    {
        uiCoins.coinCount = UserData.coins;
        uiCoins.coinCountText.text = uiCoins.coinCount.ToString();

        playerController.escudo = UserData.escudo;
        playerController.vidaExtra = UserData.vidaExtra;
        playerController.saltoDoble = UserData.saltoDoble;
        playerController.paracaidas = UserData.paracaidas;
    }

    public void GameOver()
    {
        if (playerController != null)
        {
            if (!playerController.isDrugged && playerController.isDie)
            {
                Time.timeScale = 0;
                UI_Habilidades.SetActive(true);
            }
            else if (playerController.isDrugged && playerController.isDie)
            {
                Time.timeScale = 0;
                UI_CurrentEffect.SetActive(false);
                UI_MensajeHabilidades.SetActive(true);

            }

        }
    }

    public void ResetLevel()
    {
        UserData.coins = uiCoins.coinCount;
        UserData.escudo = playerController.escudo;
        UserData.saltoDoble = playerController.saltoDoble;
        UserData.vidaExtra = playerController.vidaExtra;
        UserData.paracaidas = playerController.paracaidas;
        SceneManager.LoadScene("Test");
    }

}