using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
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
    [SerializeField] private GameObject canvasFog;
    private PlayerMovementNew playerMovementNew;
    private float progress;

    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void Start()
    {
        //espejo = FindAnyObjectByType<Espejo>();
    }

    private void SelectDimension()
    {
        StopAllCoroutines();
        StartCoroutine(SwitchScene());
        Espejo.isChecked = false;
    }
    private IEnumerator SwitchScene()
    {
        canvasFog.SetActive(true);
        canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");

        yield return new WaitForSecondsRealtime(1);
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
                        yield return new WaitForSecondsRealtime(.5f);
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
