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
        DimensionA,
        DimensionB,
        DimensionC,
        DimensionD,
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
        //canvasFog.SetActive(true);
        //canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        //canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        //canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        StartCoroutine(PlayerDisolve());

        yield return new WaitForSecondsRealtime(.5f);
        playerMovementNew.direction = Vector2.zero;
        playerMovementNew.movementMode = PlayerMovementNew.MovementMode.TapMode;
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.targetPosition = playerMovementNew.transform.position;
        playerMovementNew.anim.Play("Idle");;
        
        // yield return new WaitForSeconds(5);
        yield return new WaitForSecondsRealtime(3);
        switch (dimensions)
        {
            case (Dimensions.DimensionA):
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(limbos[0]);
                asyncOperation.allowSceneActivation = false;
                while (!asyncOperation.isDone)
                {
                    progress = asyncOperation.progress;
                    if (progress == .9f)
                    {
                        //textoCarga.text = "100 %";
                        yield return new WaitForSecondsRealtime(2f);
                        //anim.Play("AnimacionSalida");
                        //yield return new WaitForSecondsRealtime(animacion.averageDuration / Mathf.Abs(anim.GetFloat("ExitSpeed")));
                        //isLoading = false;
                        asyncOperation.allowSceneActivation = true;
                    }
                }
                //SceneManager.LoadScene(limbos[0]);
                break;
            case (Dimensions.DimensionB):
                SceneManager.LoadScene(limbos[1]);
                break;
            case (Dimensions.DimensionC):
                SceneManager.LoadScene(limbos[2]);
                break;
            case (Dimensions.DimensionD):
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
