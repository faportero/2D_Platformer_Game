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
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject UI_Habilidades;
    [SerializeField] private GameObject UI_MensajeHabilidades;
    [SerializeField] private GameObject UI_CurrentEffect;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] private GameObject[] piezasNivel;
    [SerializeField] private GameObject fogPanel;
    [SerializeField] private Transform newStartPos;
    public static bool usedPA, usedPB, usedPC, usedPD, isFogTransition;
    private Material playerMaterial;
    private float progress;
    //[SerializeField] private UI_Piezas uiPiezas;

    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerMaterial = playerMovementNew.GetComponent<SpriteRenderer>().material;

        // fogPanel.SetActive(false);
        // if (newStartPos) playerController.transform.position = newStartPos.position;
        //uiCoins.coinCount = UserData.coins;
        //uiCoins.coinCountText.text = uiCoins.coinCount.ToString();

        //playerController.escudo = UserData.escudo;
        //playerController.vidaExtra = UserData.vidaExtra;
        //playerController.saltoDoble = UserData.saltoDoble;
        //playerController.paracaidas = UserData.paracaidas;
        //print("TerminoLobby: " + UserData.terminoLobby);
        //print("TerminoLimbo: " + UserData.terminoLimbo);
        //print("TerminoNivel1: " + UserData.terminoNivel1);
        switch (currentScene)
        {
            case CurrentScene.Lobby:

                InitFogTransition();

                if (UserData.terminoLobby)
                {
                    if (newStartPos)
                    {
                        AudioManager.Instance.PlayMusic("Bg_Lobby", 0);
                        //AudioManager.Instance.ToggleMusic();

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
                AudioManager.Instance.PlayMusic("Bg_Limbo", 0);

                if (UserData.terminoLimbo)
                {
                    //AudioManager.Instance.ToggleMusic();

                    if (newStartPos) playerController.transform.position = newStartPos.position;
                    //print("Limboooo");
                }
                break;
            case CurrentScene.Nivel1:
                InitFogTransition();
                AudioManager.Instance.PlayMusic("Bg_Nivel_1", 0);
                playerMovementNew.inputsEnabled = false;
                if (UserData.terminoTutorial)
                {
                   // AudioManager.Instance.ToggleMusic();
                    if (newStartPos) playerController.transform.position = newStartPos.position;
                    playerMovementNew.inputsEnabled = true;
                    //print("Limboooo");
                }

                break;
            case CurrentScene.Nivel2:
                InitFogTransition();
                AudioManager.Instance.PlayMusic("Bg_Nivel_2", 0);
                if (UserData.terminoTutorial2)
                {
                    // AudioManager.Instance.ToggleMusic();
                    if (newStartPos) playerController.transform.position = newStartPos.position;
                    playerMovementNew.inputsEnabled = true;
                    //print("Limboooo");
                }
                //if (UserData.terminoTutorial)
                //{
                //    if (newStartPos) playerController.transform.position = newStartPos.position;
                //}
                break;
            case CurrentScene.Nivel3:
                InitFogTransition();
                AudioManager.Instance.PlayMusic("Bg_Nivel_3", 0);
                if (UserData.terminoTutorial3)
                {
                    // AudioManager.Instance.ToggleMusic();
                    if (newStartPos) playerController.transform.position = newStartPos.position;
                    playerMovementNew.inputsEnabled = true;
                    //print("Limboooo");
                }
                break;

            default:

                break;
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
        if(piezasNivel != null) CheckLevelPieces();

        if(currentScene != CurrentScene.Lobby)
        {
            AudioManager.Instance.ToggleMusic(true);
        }

    }
    private void InitFogTransition()
    {
        if (isFogTransition)
        {
            //fogPanel.SetActive(true);
            StartCoroutine(ShowFogPanel());
            fogPanel.transform.GetChild(0).gameObject.SetActive(true);
            fogPanel.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            fogPanel.transform.GetChild(0).GetComponent<Animator>().Play("FogTransitionEnd");
            //fogPanel.GetComponent<UI_LoadingScene>().ShowOppener();

        }
    }
    IEnumerator ShowFogPanel()
    {
        fogPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4);
        if(currentScene != CurrentScene.Limbo)StartCoroutine(PlayerSolidify());
        else playerMaterial.SetFloat("_DissolveAmmount", 0);        
        fogPanel.SetActive(false);

    }
    private IEnumerator PlayerSolidify()
    {
        AudioManager.Instance.PlaySfx("Solidify");


        float dissolveAmount = 1;
        float duration = 2f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / duration);
            playerMaterial.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 0
        playerMaterial.SetFloat("_DissolveAmmount", 0);
    }
    private IEnumerator PlayerDisolve()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = 1f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            playerMaterial.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 1
        playerMaterial.SetFloat("_DissolveAmmount", 1);
    }
    public void GameOver()
    {
        //print("isDrugged: "+playerController.isDrugged + ". isDie:"+playerController.isDie);
        if (playerController != null)
        {

             if (playerController.isDrugged && playerController.isDie)
            {
                Time.timeScale = 0;
                UI_MensajeHabilidades.SetActive(true);
                if(panelHUD != null) panelHUD.SetActive(false);
            }
            else
            {                
                ResetLevel();
            }
        }
    }

    public void ResetLevel()
    {
        //UserData.coins = uiCoins.coinCount;
        //UserData.escudo = playerController.escudo;
        //UserData.saltoDoble = playerController.saltoDoble;
        //UserData.vidaExtra = playerController.vidaExtra;
        //UserData.paracaidas = playerController.paracaidas;
        // SceneManager.LoadScene("Nivel_1");
        // UserData.terminoLobby = true;
        // SceneManager.LoadScene("Nivel_1");
        SelectDimension();
    }
    public void SelectDimension()
    {
        //StopAllCoroutines();
        StartCoroutine(SwitchScene());
    }

    private IEnumerator SwitchScene()
    {
        Time.timeScale = 1;

        AudioManager.Instance.PlaySfx("Fog_Transition");

        playerMovementNew.canMove = false;
        //FogTransicion
        LevelManager.isFogTransition = true;
        fogPanel.SetActive(true);
        fogPanel.transform.GetChild(0).gameObject.SetActive(true);
        fogPanel.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        fogPanel.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        StartCoroutine(PlayerDisolve());
        yield return new WaitForSecondsRealtime(5);
        if(currentScene == CurrentScene.Nivel1)SceneManager.LoadScene("Nivel_1");
        else if(currentScene == CurrentScene.Nivel2)SceneManager.LoadScene("Nivel_2");
        else if(currentScene == CurrentScene.Nivel3)SceneManager.LoadScene("Nivel_3");

    }
    private void CheckLevelPieces()
    {
        switch (currentScene)
        {
            case CurrentScene.Nivel1:
                if (UserData.usedPiezaA_N1)
                {
                    if (piezasNivel[0] != null) piezasNivel[0].SetActive(false);
                }
                if (UserData.usedPiezaB_N1)
                {
                    if (piezasNivel[1] != null) piezasNivel[1].SetActive(false);
                }
                if (UserData.usedPiezaC_N1)
                {
                    if (piezasNivel[2] != null) piezasNivel[2].SetActive(false);
                }
                if (UserData.usedPiezaD_N1)
                {
                    if (piezasNivel[3] != null) piezasNivel[3].SetActive(false);
                }
                break;

            case CurrentScene.Nivel2:
                if (UserData.usedPiezaA_N2)
                {
                    if (piezasNivel[0] != null) piezasNivel[0].SetActive(false);
                }
                if (UserData.usedPiezaB_N2)
                {
                    if (piezasNivel[1] != null) piezasNivel[1].SetActive(false);
                }
                if (UserData.usedPiezaC_N2)
                {
                    if (piezasNivel[2] != null) piezasNivel[2].SetActive(false);
                }
                if (UserData.usedPiezaD_N2)
                {
                    if (piezasNivel[3] != null) piezasNivel[3].SetActive(false);
                }
                break;

            case CurrentScene.Nivel3:
                if (UserData.usedPiezaA_N3)
                {
                    if (piezasNivel[0] != null) piezasNivel[0].SetActive(false);
                }
                if (UserData.usedPiezaB_N3)
                {
                    if (piezasNivel[1] != null) piezasNivel[1].SetActive(false);
                }
                if (UserData.usedPiezaC_N3)
                {
                    if (piezasNivel[2] != null) piezasNivel[2].SetActive(false);
                }
                if (UserData.usedPiezaD_N3)
                {
                    if (piezasNivel[3] != null) piezasNivel[3].SetActive(false);
                }
                break;
        }



        //if (usedPA)
        //{
        //   if (piezasNivel[0] != null) piezasNivel[0].SetActive(false);
        //}
        //if (usedPB)
        //{
        //    if (piezasNivel[1] != null) piezasNivel[1].SetActive(false);
        //}
        //if (usedPC)
        //{
        //    if (piezasNivel[2] != null) piezasNivel[2].SetActive(false);
        //}
        //if (usedPD)
        //{
        //    if (piezasNivel[3] != null) piezasNivel[3].SetActive(false);
        //}
    }

}