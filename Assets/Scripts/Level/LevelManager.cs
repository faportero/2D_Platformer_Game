using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public enum CurrentScene
    {
        Lobby,
        Limbo,
        Nivel1,
        Nivel2,
        Nivel3,
    }
    [Header("Current Level")]
    public CurrentScene currentScene;

    public PlayerControllerNew playerController;
    [SerializeField] private GameObject UI_Habilidades;
    [SerializeField] private GameObject UI_MensajeHabilidades;
    [SerializeField] private GameObject UI_CurrentEffect;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] private GameObject[] piezasNivel;
    [SerializeField] private GameObject fogPanel;
    public static bool usedPA, usedPB, usedPC, usedPD, isFogTransition = false;
    //[SerializeField] private UI_Piezas uiPiezas;
        
    private void Awake()
    {
        uiCoins.coinCount = UserData.coins;
        uiCoins.coinCountText.text = uiCoins.coinCount.ToString();

        //playerController.escudo = UserData.escudo;
        //playerController.vidaExtra = UserData.vidaExtra;
        //playerController.saltoDoble = UserData.saltoDoble;
        //playerController.paracaidas = UserData.paracaidas;
    }
    private void Start()
    {
        Time.timeScale = 1;
        if(piezasNivel != null) CheckLevelPieces();

        //if (isFogTransition)
        //{
        //    fogPanel.SetActive(true);
        //    fogPanel.transform.GetChild(0).gameObject.SetActive(true);
        //    fogPanel.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        //    fogPanel.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        //    fogPanel.GetComponent<UI_LoadingScene>().ShowOppener();
        //    isFogTransition = false;
        //}
    }
    public void GameOver()
    {
        Time.timeScale = 0;

        if (playerController != null)
        {
            if (!playerController.isDrugged && playerController.isDie)
            {                
                UI_Habilidades.SetActive(true);
            }
            else if (playerController.isDrugged && playerController.isDie)
            {  
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
        SceneManager.LoadScene("Nivel_1");
    }

    private void CheckLevelPieces()
    {
        if (usedPA)
        {
            piezasNivel[0].SetActive(false);
        }
        if (usedPB)
        {
            piezasNivel[1].SetActive(false);
        }
        if (usedPC)
        {
            piezasNivel[2].SetActive(false);
        }
        if (usedPD)
        {
            piezasNivel[3].SetActive(false);
        }
    }

}