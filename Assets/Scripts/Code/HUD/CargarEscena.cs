using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class CargarEscena : MonoBehaviour
{
    public GameObject panelEspera;
    public Image imageCharge;
    public TextMeshProUGUI textoCarga, textoTip;
    float progress;
    bool isLoading = false;
    int aux = 0;
    int lvl = 0;
    string txt = "";
    AsyncOperation asyncOperation;
    [System.Serializable]
    public struct Tips
    {
        public string[] tips;
    }
    public Tips[] tips;
    private void Awake()
    {
        GetValueNivel();
    }
    private void Update()
    {
        if (isLoading)
        {
            //Debug.Log(progress);
            imageCharge.fillAmount = Mathf.MoveTowards(imageCharge.fillAmount, progress / .9f, .5f * Time.deltaTime);
            textoCarga.text = ((int)(imageCharge.fillAmount * 100)).ToString() + "%";
        }
    }
    public void GetValueNivel()
    {
        Application.targetFrameRate = 60;
        var nivel = ControlDatos._nivel;
        //Para tomar el nivel localmente.
        //var nivel = ControlDatos._nivel = PlayerPrefs.GetInt("Nivel");        
        //ControlDatos._coins = PlayerPrefs.GetInt("Coins");

        if (nivel == 1)
        {
            ControlDatos._isWinnerLvl1 = true;
        }
        if (nivel == 2)
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
        }
        if (nivel == 3)
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
        }
        if (nivel == 4)
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
            ControlDatos._isWinnerLvl4 = true;
        }
        if (nivel == 5)
        {
            ControlDatos._isWinnerLvl1 = true;
            ControlDatos._isWinnerLvl2 = true;
            ControlDatos._isWinnerLvl3 = true;
            ControlDatos._isWinnerLvl4 = true;
            ControlDatos._isWinnerLvl5 = true;
        }
        lvl = Mathf.Clamp(nivel - 1, 0, 5);
        txt = tips[lvl].tips[(Random.Range(0, tips[lvl].tips.Length))];
        //print("Nivel: " + (lvl + 1) + ". Texto: " + txt);
        textoTip.SetText(txt);
    }
    public void CargarScene(string escena)
    {
        SceneManager.LoadScene(escena);
    }
    public void CargarJuego(string escena)
    {
        //ControladorDatos.isNewGame = true;
        SceneManager.LoadScene(escena);
    }

    public void ContinuarJuego(string escena)
    {
        //ControladorDatos.isNewGame = false;
        SceneManager.LoadScene(escena);
    }
    public void CargarJuegoAsincrono(string escena)
    {
        //ControladorDatos._ganoPuzzle1 = false;
        //ControladorDatos._ganoPuzzle2 = false;
        //ControladorDatos._ganoPuzzle3 = false;
        //ControladorDatos._ganoPuzzle4 = false;
        //ControladorDatos.isNewGame = true;
        //SceneManager.LoadScene(escena);
        //PlayerPrefs.SetInt("Nivel", 2);
        panelEspera.SetActive(true);
        StartCoroutine(EsperaAsincrona(escena));
    }
    public void ContinuarJuegoAsincrono()
    {
        GetValueNivel();
        panelEspera.SetActive(true);
        string escena = "2. Nivel 1";
        if (ControlDatos._isWinnerLvl1) escena = "3. Nivel 2";
        if (ControlDatos._isWinnerLvl2) escena = "4. Nivel 3";
        if (ControlDatos._isWinnerLvl3) escena = "5. Nivel 4";
        if (ControlDatos._isWinnerLvl4) escena = "6. Nivel 5";
        if (ControlDatos._isWinnerLvl5) escena = "6. Nivel 5";
        //if (ControladorDatos._posicionPersonajeNivel1 != 0 || ControladorDatos._posicionPersonajeNivel2 != 0 || ControladorDatos._posicionPersonajeNivel3 != 0 || ControladorDatos._posicionPersonajeNivel4 != 0)
        //    ControladorDatos.isNewGame = false;
        //else ControladorDatos.isNewGame = true;
        StartCoroutine(EsperaAsincrona(escena));
    }
    IEnumerator EsperaAsincrona(string escena)
    {
        //print("Aún no está activo el panel");
        yield return new WaitWhile(() => !panelEspera.activeSelf);
        //print("Ya está activo el panel");
        Animator anim = panelEspera.GetComponent<Animator>();
        AnimationClip animacion = anim.runtimeAnimatorController.animationClips[1];
        float exitSpeed = Mathf.Abs(anim.GetFloat("ExitSpeed"));
        if (exitSpeed > 0)
        {
            yield return new WaitForSeconds(animacion.averageDuration / exitSpeed);
        }
        else
        {
            //Debug.LogError("ExitSpeed es 0, no se puede dividir.");
            yield break;
        }
        //print("name " + animacion.name + " tiempo: " + animacion.averageDuration / exitSpeed + ". ExitSpeed: " + exitSpeed);
        yield return new WaitForSecondsRealtime(animacion.averageDuration / exitSpeed);//antes era realtime aquí y abajo
        yield return new WaitForSecondsRealtime(.5f);
        //yield return Resources.UnloadUnusedAssets();
        isLoading = true;
        while (progress < .9f)
        {
            progress += .025f;
            //Debug.Log("Progreso de carga: " + progress + ". Escena: " + escena);
            yield return new WaitForSecondsRealtime(.1f);
        }
        //yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene(escena);
        //asyncOperation = SceneManager.LoadSceneAsync(escena);
        //asyncOperation.allowSceneActivation = false;
        if (escena == "2. Nivel 1")
        {
            ControlDatos._isWinnerLvl1 = false;
            ControlDatos._isWinnerLvl2 = false;
            ControlDatos._isWinnerLvl3 = false;
            ControlDatos._isWinnerLvl4 = false;
            ControlDatos._isWinnerLvl5 = false;
            ControlDatos._points = 0;
            ControlDatos._coins = 0;
            ControlDatos._currentNivel = 0;
            ControlDatos._listaObjetosInventario = new List<Vector2>
                {
                    new Vector2(2, ControlDatos._points),
                    new Vector2(3, ControlDatos._coins)
                };
            ControlDatos.CrearEditarObjetoInventario();
            PlayerPrefs.SetInt("Nivel", 0);
            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.SetInt("Points", 0);
            PlayerPrefs.Save();
        }
        //while (!asyncOperation.isDone)
        //{
        //    progress = asyncOperation.progress;
        //    Debug.Log("Progreso de carga: " + progress + ". Escena: " + escena);
        //    if (progress == .9f)
        //    {
        //        textoCarga.text = "100 %";
        //        yield return new WaitForSecondsRealtime(4);
        //        anim.Play("AnimacionSalida");
        //        yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
        //        isLoading = false;
        //        asyncOperation.allowSceneActivation = true;
        //    }
        //}
    }
    public void CerrarSesion(string escena)
    {
        ControlDatos._Estado = 1;
        ControlDatos._Mensaje = "";
        ControlDatos._Correo = "";
        ControlDatos._Password = "";
        ControlDatos._loginPassword = "";
        ControlDatos._Nombre = "";
        ControlDatos._Id = "";
        ControlDatos._loginCorreo = "";
        SceneManager.LoadScene(escena);
    }
    public void ReiniciarOMenuJuego(string escena)
    {
        Time.timeScale = 1;
        if (escena == "0. Menu Principal")
        {
            SaveBeforeExit();
            ControlDatos.menu = true;
        }
        SceneManager.LoadScene(escena);
    }
    public void Pause()
    {
        SaveBeforeExit();
        Time.timeScale = 0;
    }
    public void NoPause()
    {
        Time.timeScale = 1;
    }
    public void SalirDelJuego()
    {
        SaveBeforeExit();
        Application.Quit();
    }
    public void SaveBeforeExit()
    {
        PlayerPrefs.SetInt("Points", ControlDatos._points);
        PlayerPrefs.SetInt("Coins", ControlDatos._coins);
        PlayerPrefs.Save();
        ControlDatos._listaObjetosInventario = new List<Vector2>();
        //print("points: " + ControlDatos._points + ". BestPoints: " + ControlDatos._bestPoints);
        if (ControlDatos._points > ControlDatos._bestPoints || ControlDatos._currentNivel > ControlDatos._nivel)
        {
            if (ControlDatos._points > ControlDatos._bestPoints)
            {
                ControlDatos._bestPoints = ControlDatos._points;
                ControlDatos._listaObjetosInventario.Add(new Vector2(1, ControlDatos._bestPoints));
            }
            if (ControlDatos._currentNivel > ControlDatos._nivel)
            {
                ControlDatos._nivel = ControlDatos._currentNivel;
                ControlDatos._listaObjetosInventario.Add(new Vector2(4, ControlDatos._nivel));
            }
        }
        ControlDatos._listaObjetosInventario.Add(new Vector2(3, ControlDatos._coins));
        ControlDatos.CrearEditarObjetoInventario();
    }
}
