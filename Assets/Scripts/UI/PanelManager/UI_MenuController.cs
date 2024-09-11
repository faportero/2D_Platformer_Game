using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        rootCanvas = GetComponent<Canvas>();
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

