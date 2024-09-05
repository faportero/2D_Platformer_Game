using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Gargola : MonoBehaviour
{
    public GameObject videoPlayerPlane, viajarBtn, skipBtn, canvasFog, vortexRays, vortexCircle;
    private Collider2D solidObjectCol;
    public VideoPlayer videoPlayer;
    [SerializeField] private string nivel;
    [SerializeField] private CinemachineVirtualCamera camera1, camera2;
    private PlayerMovementNew playerMovementNew;
    private PlayerControllerNew playerController;
    private Animator animatorVideo;
    private float progress;
    private Material playerMaterial;
    private SwipeDetector swipeDetector;
    public enum VortexType
    {
        Vortex1,
        Vortex2,
        Vortex3
    }

    public VortexType vortexType;

    private void Start()
    {


        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerController = FindAnyObjectByType<PlayerControllerNew>();
        animatorVideo = videoPlayerPlane.GetComponent<Animator>();
        solidObjectCol = transform.GetChild(0).gameObject.GetComponent<Collider2D>();

        playerMaterial = playerMovementNew.GetComponent<SpriteRenderer>().material;
        swipeDetector = playerMovementNew.swipeDetector;

        // Suscribirse al evento prepareCompleted
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoEnd;

        // Preparar el video
        videoPlayer.Prepare();

        AssignVortexType();
    }

    private void Update()
    {
        // Lógica de depuración opcional aquí
    }

    //public void AssignVortexType()
    //{
    //    switch (vortexType)
    //    {
    //        case VortexType.Vortex1:
    //            viajarBtn.GetComponent<Button>().interactable = true;
    //            if (UserData.completoNivel1)
    //            {
    //                vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .23f);
    //                vortexCircle.SetActive(false);
    //            }

    //            break;
    //        case VortexType.Vortex2:
    //            if (UserData.completoNivel1)
    //            {
    //                viajarBtn.GetComponent<Button>().interactable = true;
    //                vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .35f);
    //                vortexCircle.SetActive(true);
    //            }
    //            else if (UserData.completoNivel2)
    //            {
    //                vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .23f);
    //                vortexCircle.SetActive(false);
    //            }

    //            break;
    //        case VortexType.Vortex3:
    //            if (UserData.completoNivel2)
    //            {
    //                viajarBtn.GetComponent<Button>().interactable = true;
    //                vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .35f);
    //                vortexCircle.SetActive(true);
    //            }
    //            else if (UserData.completoNivel3)
    //            {
    //                vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .23f);
    //                vortexCircle.SetActive(false);
    //            }
    //            break;
    //    }
    //}

    public void AssignVortexType()
    {
        // Desactivamos todos los vortex states antes de aplicar la lógica específica
        if (UserData.completoNivel1 && UserData.completoNivel2 && UserData.completoNivel3)
        {
            DeactivateAllVortexStates();
            viajarBtn.GetComponent<Button>().interactable = true;
        }

        // Aplicamos la lógica específica para el vortex actual basado en vortexType y el progreso de niveles
        switch (vortexType)
        {
            case VortexType.Vortex1:
                // Estado inicial para Vortex1
                if (!UserData.completoNivel1)
                {
                    SetVortexState(true);  // Vortex1 en estado normal si Nivel 1 no está completado
                }
                else
                {
                    SetVortexState(false);  // Vortex1 en estado normal si Nivel 1 no está completado
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                if (UserData.completoNivel1 && UserData.completoNivel2 && UserData.completoNivel3)
                {
                    DeactivateAllVortexStates();
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                break;

            case VortexType.Vortex2:
                // Estado al completar Nivel 1
                if (UserData.completoNivel1 && !UserData.completoNivel2 && !UserData.completoNivel3)
                {
                    SetVortexState(true);  // Vortex2 en estado normal si Nivel 1 está completado y Nivel 2 no
                     viajarBtn.GetComponent<Button>().interactable = true;
                }
                if (UserData.completoNivel1 && UserData.completoNivel2)
                {
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                if (UserData.completoNivel1 && UserData.completoNivel2 && UserData.completoNivel3)
                {
                    DeactivateAllVortexStates();
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                break;

            case VortexType.Vortex3:
                // Estado al completar Nivel 2
                if (UserData.completoNivel1 && UserData.completoNivel2 && !UserData.completoNivel3)
                {
                    SetVortexState(true);  // Vortex3 en estado normal si Nivel 2 está completado y Nivel 3 no
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                if (UserData.completoNivel1 && UserData.completoNivel2 && UserData.completoNivel3)
                {
                    DeactivateAllVortexStates();
                    viajarBtn.GetComponent<Button>().interactable = true;
                }
                break;
        }
    }

    // Método para establecer el estado del vortex
    private void SetVortexState(bool isActive)
    {
        if (isActive)
        {
            viajarBtn.GetComponent<Button>().interactable = true;
            vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .35f);
            vortexCircle.SetActive(true);
        }
        else
        {
            viajarBtn.GetComponent<Button>().interactable = false;
            vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .23f);
            vortexCircle.SetActive(false);
        }
    }

    // Método para desactivar todos los vortex states
    private void DeactivateAllVortexStates()
    {
        // Desactivar todos los vortex por defecto
        viajarBtn.GetComponent<Button>().interactable = false;
        vortexRays.GetComponent<SpriteRenderer>().material.SetFloat("_DissolveAmount", .23f);
        vortexCircle.SetActive(false);
    }


    public void SkipVideo()
    {
        videoPlayer.Stop();
        OnVideoEnd(videoPlayer);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        if (vortexType == VortexType.Vortex1) UserData.terminoVideoVortex1 = true;
        if (vortexType == VortexType.Vortex2) UserData.terminoVideoVortex2 = true;
        if (vortexType == VortexType.Vortex3) UserData.terminoVideoVortex3 = true;

        CameraManager.instance.SingleSwapCamera(camera1, 2f);

        if (UserData.terminoVideoVortex1) SelectDimension();

        if (!InputManager.isPC) swipeDetector.enabled = true;
    }

    public void ShowVideo()
    {
        if (vortexType == VortexType.Vortex1 && UserData.terminoVideoVortex1) skipBtn.SetActive(true);
        if (vortexType == VortexType.Vortex2 && UserData.terminoVideoVortex2) skipBtn.SetActive(true);
        if (vortexType == VortexType.Vortex3 && UserData.terminoVideoVortex3) skipBtn.SetActive(true);

        CameraManager.instance.SingleSwapCamera(camera2, 2);
        if (!InputManager.isPC) swipeDetector.enabled = false;

        playerMovementNew.isMoving = false;
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.canMove = false;
        playerMovementNew.targetPosition = playerMovementNew.transform.position;
        playerMovementNew.anim.SetBool("SlowWalk", false);
        playerMovementNew.anim.SetBool("Turn", true);

        StartCoroutine(ShowVideoPanel());
    }

    private IEnumerator ShowVideoPanel()
    {
        GetComponent<VortexAnimation>().enabled = true;
        yield return new WaitForSeconds(2);
        animatorVideo.enabled = true;
        videoPlayerPlane.SetActive(true);
        AudioManager.Instance.ToggleMusic(false);

        // Ahora esperamos un segundo para asegurarnos de que todo esté listo
        yield return new WaitForSeconds(.5f);

        // Aquí comenzamos la reproducción del video si ya está preparado
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            // De lo contrario, prepararlo y luego reproducir
            videoPlayer.Prepare();
        }
    }

    private void OnVideoPrepared(VideoPlayer vp)
    {
        // Esta función se llama cuando el video está completamente preparado.
        videoPlayerPlane.SetActive(true);
        videoPlayer.Play(); // Reproducir el video solo cuando esté preparado
    }

    public void SelectDimension()
    {
        videoPlayer.Stop();
        StopAllCoroutines();
        StartCoroutine(SwitchScene());
        Espejo.isChecked = false;
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

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(PlayerDisolve());

        yield return new WaitForSecondsRealtime(0.5f);
        LevelManager.isFogTransition = true;
        canvasFog.SetActive(true);
        canvasFog.transform.GetChild(0).gameObject.SetActive(true);
        canvasFog.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        canvasFog.transform.GetChild(0).GetComponent<Animator>().Play("FogTransition");
        AudioManager.Instance.PlaySfx("Fog_Transition");

        AsyncOperation asyncOperation;
        asyncOperation = SceneManager.LoadSceneAsync(nivel);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            progress = asyncOperation.progress;
            if (progress == 0.9f)
            {
                yield return new WaitForSecondsRealtime(3f);
                asyncOperation.allowSceneActivation = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            viajarBtn.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            viajarBtn.SetActive(false);
        }
    }
}
