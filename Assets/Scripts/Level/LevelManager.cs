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
    private PlayerMovementNew playerMovementNew;
    [SerializeField] private GameObject UI_Habilidades;
    [SerializeField] private GameObject UI_MensajeHabilidades;
    [SerializeField] private GameObject UI_CurrentEffect;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] private GameObject[] piezasNivel;
    [SerializeField] private GameObject fogPanel;
    [SerializeField] private Transform newStartPos;
    public static bool usedPA, usedPB, usedPC, usedPD, isFogTransition;
    //[SerializeField] private UI_Piezas uiPiezas;
        
    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
       // if (newStartPos) playerController.transform.position = newStartPos.position;
        //uiCoins.coinCount = UserData.coins;
        //uiCoins.coinCountText.text = uiCoins.coinCount.ToString();

        //playerController.escudo = UserData.escudo;
        //playerController.vidaExtra = UserData.vidaExtra;
        //playerController.saltoDoble = UserData.saltoDoble;
        //playerController.paracaidas = UserData.paracaidas;
        print("TerminoLobby: " + UserData.terminoLobby);
        print("TerminoLimbo: " + UserData.terminoLimbo);
        print("TerminoNivel1: " + UserData.terminoNivel1);
        switch (currentScene)
        {
            case CurrentScene.Lobby:

                InitFogTransition();

                if (UserData.terminoLobby)
                {
                    if (newStartPos)
                    {
                        playerController.transform.position = newStartPos.position;
                        Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                        playerController.transform.rotation = Quaternion.Euler(rotator);
                        playerMovementNew.TurnCheck();

                    }
                    // print("Lobbyyyy");
                }
                break;
            case CurrentScene.Limbo:

                InitFogTransition();

                if (UserData.terminoTutorial)
                {
                    if (newStartPos) playerController.transform.position = newStartPos.position;
                    //print("Limboooo");
                }
                break;
            case CurrentScene.Nivel1:
                InitFogTransition();

                break;
            case CurrentScene.Nivel2:

                break;
            case CurrentScene.Nivel3:

                break;

            default:

                break;
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
        if(piezasNivel != null) CheckLevelPieces();
    }
    private void InitFogTransition()
    {
        if (isFogTransition)
        {
            fogPanel.SetActive(true);
            fogPanel.transform.GetChild(0).gameObject.SetActive(true);
            fogPanel.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            fogPanel.transform.GetChild(0).GetComponent<Animator>().Play("FogTransitionEnd");
            //fogPanel.GetComponent<UI_LoadingScene>().ShowOppener();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;

        if (playerController != null)
        {
            //if (!playerController.isDrugged && playerController.isDie)
            //{                
            //    UI_Habilidades.SetActive(true);
            //}
             if (playerController.isDrugged && playerController.isDie)
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
        // SceneManager.LoadScene("Nivel_1");
       // UserData.terminoLobby = true;
        SceneManager.LoadScene("Lobby2");
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