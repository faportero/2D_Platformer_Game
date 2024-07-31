using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Gargola : MonoBehaviour
{
    public GameObject videoPlayerPlane, viajarBtn, canvasFog;
    private Collider2D solidObjectCol;
    public VideoPlayer videoPlayer;
    [SerializeField] private string nivel;
    [SerializeField] private CinemachineVirtualCamera camera1, camera2;
    private PlayerMovementNew playerMovementNew;
    private PlayerControllerNew playerController;
    private Animator animatorVideo;
    private float progress;
    private Material playerMaterial;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerController = FindAnyObjectByType<PlayerControllerNew>();
        animatorVideo = videoPlayerPlane.GetComponent<Animator>();
        videoPlayer.loopPointReached += OnVideoEnd;
        solidObjectCol = transform.GetChild(0).gameObject.GetComponent<Collider2D>();

        playerMaterial = playerMovementNew.GetComponent<SpriteRenderer>().material;

    }
    private void Update()
    {
        //if(Input.GetKeyUp(KeyCode.L))
        //{
        //    videoPlayer.Stop();
        //    OnVideoEnd(videoPlayer);

        //}

    }
    public void SkipVideo()
    {

        videoPlayer.Stop();
        OnVideoEnd(videoPlayer);
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        CameraManager.instance.SingleSwapCamera(camera1,2f);
        playerMovementNew.inputsEnabled = true;

    }
    public void ShowVideo()
    {
        CameraManager.instance.SingleSwapCamera(camera2, 2);
        playerMovementNew.isMoving = false;
        //playerMovementNew.inputsEnabled = false;
        playerMovementNew.targetPosition = playerMovementNew.transform.position;
        playerMovementNew.anim.SetBool("SlowWalk", false);
        playerMovementNew.anim.SetBool("Turn", true);
        StartCoroutine(ShowVideoPanel());
        videoPlayer.Play();

    }
    private IEnumerator ShowVideoPanel()
    {
        //GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        animatorVideo.enabled = true;
        videoPlayerPlane.SetActive(true);
        yield return new WaitForSeconds(.5f);
        videoPlayer.Play();
    }


    public void SelectDimension()
    {
        playerMovementNew.isMoving = false;
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.targetPosition = playerMovementNew.transform.position;
        SkipVideo();
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
       // playerMovementNew.isMoving = false;
        //playerMovementNew.canMove = false;
        //FogTransicion
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(PlayerDisolve());

        yield return new WaitForSecondsRealtime(0.5f);
        LevelManager.isFogTransition = true;
        canvasFog.SetActive(true);
        canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");

        AsyncOperation asyncOperation;
        asyncOperation = SceneManager.LoadSceneAsync(nivel);
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
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //solidObjectCol.enabled = false;
            //playerMovementNew.inputsEnabled = false;
            //playerMovementNew.anim.SetBool("SlowWalk", false);
            //playerMovementNew.targetPosition = playerController.transform.position;
            //CameraManager.instance.SingleSwapCamera(camera2, 2);

            ////if (LimboManager.countVideosWatched != 3)
            ////{
            ////    StartCoroutine(ShowVideoPanel());
            ////    videoPlayer.Play();
            ////}
            ////else if (LimboManager.countVideosWatched >= 3)
            ////{
            ////    videoPlayerPlane.SetActive(true);
            ////    videoPlayerPlane.GetComponent<Animator>().enabled = true;
            ////    videoPlayerPlane.GetComponent<Animator>().Play("IdlePlayer");
            ////    videoPlayer.Play();
            ////}
            viajarBtn.SetActive(true);
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       // solidObjectCol.enabled = true;
        if (collision.tag == "Player")
        {

            viajarBtn.SetActive(false);
        }

    }
}

