using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class Portal : MonoBehaviour
{
    public string[] limbos;

   [SerializeField] private enum Dimensions
    {
        Lobby,
        Limbo,
        Nivel1,
        Nivel2,
        Nivel3,
    }
    [SerializeField]  private Dimensions dimensions;
    [SerializeField] private GameObject panelFeedback;
    public GameObject canvasFog;
    private PlayerMovementNew playerMovementNew;
    private PlayerControllerNew playerControllerNew;
    private float progress;
    private Material playerMaterial;

    private void Awake()
    {
        //canvasFog.SetActive(false);
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerControllerNew = FindAnyObjectByType<PlayerControllerNew>();
        playerMaterial = playerControllerNew.GetComponent<SpriteRenderer>().material;
    }
    private void Start()
    {
        //canvasFog.SetActive(false);
        //espejo = FindAnyObjectByType<Espejo>();
    }

    private void SelectDimension()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchScene());
        Espejo.isChecked = false;
    }

    private IEnumerator PlayerDisolve()
    {
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
    private IEnumerator SwitchScene()
    {
        playerMovementNew.canMove = false;
        //FogTransicion
        LevelManager.isFogTransition = true;
        canvasFog.SetActive(true);
        canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        StartCoroutine(PlayerDisolve());

        yield return new WaitForSecondsRealtime(.5f);
        playerMovementNew.direction = Vector2.zero;
        playerMovementNew.movementMode = PlayerMovementNew.MovementMode.TapMode;
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.targetPosition = playerMovementNew.transform.position;
        playerMovementNew.anim.Play("Idle");;
        
        // yield return new WaitForSeconds(5);
        yield return new WaitForSecondsRealtime(3);

        AsyncOperation asyncOperation;
        switch (dimensions)
        {
            case (Dimensions.Lobby):
                asyncOperation = SceneManager.LoadSceneAsync(limbos[0]);
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    progress = asyncOperation.progress;
                    if (progress == .9f)
                    {
                        //textoCarga.text = "100 %";
                        yield return new WaitForSecondsRealtime(1f);
                        //anim.Play("AnimacionSalida");
                        //yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
                        //isLoading = false;
                        UserData.terminoNivel1 = true;
                        asyncOperation.allowSceneActivation = true;
                    }
                }

               

                break;
            case (Dimensions.Limbo):
                asyncOperation = SceneManager.LoadSceneAsync(limbos[1]);
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    progress = asyncOperation.progress;
                    if (progress == .9f)
                    {
                        //textoCarga.text = "100 %";
                        yield return new WaitForSecondsRealtime(1f);
                        //anim.Play("AnimacionSalida");
                        //yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
                        //isLoading = false;
                        UserData.terminoLobby = true;
                        asyncOperation.allowSceneActivation = true;
                    }
                }

               


                break;
            case (Dimensions.Nivel1):
                asyncOperation = SceneManager.LoadSceneAsync(limbos[2]);
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    progress = asyncOperation.progress;
                    if (progress == .9f)
                    {
                        //textoCarga.text = "100 %";
                        yield return new WaitForSecondsRealtime(1f);
                        //anim.Play("AnimacionSalida");
                        //yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
                        //isLoading = false;
                        UserData.terminoLimbo = true;

                        asyncOperation.allowSceneActivation = true;
                    }
                }


                break;
            case (Dimensions.Nivel2):
                SceneManager.LoadScene(limbos[3]);
                break;
            case (Dimensions.Nivel3):
                SceneManager.LoadScene(limbos[3]);
                break;
            default:
                SceneManager.LoadScene(limbos[0]);
                break;

        }
        //canvasFog.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerMovementNew.isPortalEnter = true;
            playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
            playerMovementNew.isMoving = false; // Detener el movimiento
            playerMovementNew.anim.SetBool("SlowWalk", false); // Desactivar animación de caminar
            playerMovementNew.anim.SetBool("Walk", false); // Desactivar animación de caminar
            LevelManager.isFogTransition = true;
                SelectDimension();
            //if (Espejo.piezasRestantes != 0)
            //{
            //    panelFeedback.SetActive(true);
            //}
            //else
            //{
            //    panelFeedback.SetActive(false);
            //    LevelManager.isFogTransition = true;
            //    SelectDimension();
            //}
        }
    }
}
