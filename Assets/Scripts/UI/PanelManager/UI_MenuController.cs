using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//tutorial aqui: "https://www.youtube.com/watch?v=9MIwIaRUUhc"

[RequireComponent(typeof(Canvas))]
[DisallowMultipleComponent] // solo queremos un Menu Controller
public class UI_MenuController : MonoBehaviour
{
    [SerializeField]
    private UI_Page initialPage;
    [SerializeField]
    private GameObject firstFocusItem;

    private Canvas rootCanvas;
    private Stack<UI_Page> pageStack = new Stack<UI_Page>();
    
    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        rootCanvas = GetComponent<Canvas>();
        print("termino primer video en menu: " + UserData.terminoPrimerVideo);
        if (levelManager.currentScene == LevelManager.CurrentScene.Menu)
        {
            if (UserData.terminoPrimerVideo == true)
            {
                if (firstFocusItem != null) 
                {
                    firstFocusItem.GetComponent<Button>().interactable = true;
                } 

            }
            else
            {
                if (firstFocusItem != null)
                {
                    firstFocusItem.GetComponent<Button>().interactable = false;
                }

            }
        }
    }

    private void Start()
    {



        if (firstFocusItem != null)
        {
            EventSystem.current.SetSelectedGameObject(firstFocusItem);
        }

        if (initialPage != null)
        {
            PushPage(initialPage);
        }
    }
    private void OnApplicationPause()
    {
        if (InputManager.isPC)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancel();
            }
        }
    }

    public bool IsPageInStack(UI_Page Page)
    {
        return pageStack.Contains(Page);
    }

    public bool IsPageOnTopOfStack(UI_Page Page)
    {
        return pageStack.Count > 0 && Page == pageStack.Peek();
    }

    private void OnCancel()
    {
        if (rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
        {
            if (pageStack.Count != 0)
            {
                PopPage();
            }
        }
    }
    public void PushPage(UI_Page Page)
    {
        Page.Enter(true);

        if (pageStack.Count > 0)
        {
            UI_Page currentPage = pageStack.Peek();

            if (currentPage.ExitOnNewPagePush)
            {
                currentPage.Exit(false);
            }
            else 
            {
                currentPage.Enter(true);
            }
        }

        pageStack.Push(Page);
    }

    public void PopPage()
    {
        if (pageStack.Count > 1)
        {
            UI_Page page = pageStack.Pop();
            page.Exit(true);

            UI_Page newCurrentPage = pageStack.Peek();
            if (newCurrentPage.ExitOnNewPagePush)
            {
                newCurrentPage.Enter(false);
            }

        }
        else
        {
            Debug.LogWarning("Intentando lanzar una pagina pero solo 1 queda 1 pagina en la pila");
        }
    }

    public void PopAllPages()
    {
        for (int i = 1; i < pageStack.Count; i++)
        {
            PopPage();
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("0. Menu Principal");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Lobby2");
    }
    public static void PlayNewGame()
    {
        UserData.lifes = 3;
        UserData.health = 0;
        UserData.coins = 0;
        UserData.escudo = false;
        UserData.saltoDoble = false;
        UserData.vidaExtra = false;
        UserData.paracaidas = false;
        UserData.terminoLobby = false;
        UserData.terminoPrimerVideo = false;
        UserData.terminoLimbo = false;
        UserData.terminoNivel1 = false;
        UserData.terminoTutorial = false;
        UserData.terminoTutorial2 = false;
        UserData.terminoTutorial3 = false;
        UserData.terminoWorldTutorial1 = false;
        UserData.terminoWorldTutorial2 = false;
        UserData.terminoWorldTutorial3 = false;
        UserData.terminoVideoInicio = false;
        UserData.terminoVideoVortex1 = false;
        UserData.terminoVideoVortex2 = false;
        UserData.terminoVideoVortex3 = false;
        UserData.piezaA_N1 = false;
        UserData.piezaA_N2 = false;
        UserData.piezaA_N3 = false;
        UserData.piezaB_N1 = false;
        UserData.piezaB_N2 = false;
        UserData.piezaB_N3 = false;
        UserData.piezaC_N1 = false;
        UserData.piezaC_N2 = false;
        UserData.piezaC_N3 = false;
        UserData.piezaD_N1 = false;
        UserData.piezaD_N2 = false;
        UserData.piezaD_N3 = false;
        UserData.usedPiezaA_N1 = false;
        UserData.usedPiezaA_N2 = false;
        UserData.usedPiezaA_N3 = false;
        UserData.usedPiezaB_N1 = false;
        UserData.usedPiezaB_N2 = false;
        UserData.usedPiezaB_N3 = false;
        UserData.usedPiezaC_N1 = false;
        UserData.usedPiezaC_N2 = false;
        UserData.usedPiezaC_N3 = false;
        UserData.usedPiezaD_N1 = false;
        UserData.usedPiezaD_N2 = false;
        UserData.usedPiezaD_N3 = false;
        UserData.playerGuide1 = false;
        UserData.playerGuide2 = false;
        UserData.playerGuide3 = false;
        UserData.playerGuide4 = false;
        UserData.playerGuide5 = false;
        UserData.playerGuide6 = false;

        // Guardar los datos del login
        string loginEmail = PlayerPrefs.GetString("User_Email");
        string loginPassword = PlayerPrefs.GetString("User_Password");

        // Eliminar todas las preferencias
        PlayerPrefs.DeleteAll();

        // Restaurar los datos del login
        PlayerPrefs.SetString("User_Email", loginEmail);
        PlayerPrefs.SetString("User_Password", loginPassword);

        // Guardar los cambios
        PlayerPrefs.Save();

        SceneManager.LoadScene("Lobby2");
    }
    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás construyendo para una plataforma de ejecución, se cierra la aplicación
        Application.Quit();
#endif
    }

}

