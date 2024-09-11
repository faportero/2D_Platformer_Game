using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject canvasFade;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioPause audioPause;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject tapPanel;
    [SerializeField] private GameObject RelojPanel;
    [SerializeField] private Transform newPlayerPos;
    [SerializeField] private GameObject portalInicio;
    [SerializeField] private GameObject playerGuide1, playerGuide2, playerGuide3;

    [SerializeField] private SwipeDetector swipeDetector;
    [SerializeField] private CinemachineVirtualCamera cameraInit,camera2;

    private PlayerControllerNew playerController;
    private PlayerMovementNew playerMovementNew;
    private Material playerMaterial;


    public float elevationDistance = 4f; // Distancia de elevación
    public float elevationDuration = 4f; // Duración de la elevación y cambio de opacidad
    public float rotationDuration = 1f;  // Duración de la rotación
    private SpriteRenderer spriteRenderer;

    public Transform portalPos;

    public static bool pasoIntro;



    [SerializeField] private bool completoN1, completoN2, completoN3;
    private void Awake()
    {
        //if (PlayerPrefs.GetInt("pasoIntro") == 0) PlayerPrefs.SetInt("pasoIntro", 0);
        //PlayerPrefs.SetInt("pasoIntro", 0);
        PlayerPrefs.GetInt("pasoIntro");
        // print("PasoIntro: " + PlayerPrefs.GetInt("pasoIntro"));
        //if (PlayerPrefs.GetInt("pasoIntro") != 0)
        //if (pasoIntro)
        //{
        //    pasoIntro = true;
        //}
        //else
        //{
        //    pasoIntro = false;
        //}
        //UserData.completoNivel1 = completoN1;
        //UserData.completoNivel2 = completoN2;
        //UserData.completoNivel3 = completoN3;
    }
    void Start()
    {

        playerController = FindAnyObjectByType<PlayerControllerNew>();
        playerMaterial = playerController.GetComponent<SpriteRenderer>().material;
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        spriteRenderer = playerController.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
        playerMaterial.SetFloat("_DissolveAmmount", 0);

        //if(!UserData.terminoLobby)playerController.GetComponent<GhostController>().enabled = false;
        //else playerController.GetComponent<GhostController>().enabled = true;
        

        // Obtener el componente VideoPlayer del objeto actual


        //if (pasoIntro == false)
        if (!UserData.terminoLobby)
        {
            audioPause.Pause(true);
            playerMovementNew.inputsEnabled = false;
            if (videoPlayer != null)
            {
                panelVideo.SetActive(true);
                
                string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Introtest.mp4");
                videoPlayer.url = videoPath;
                videoPlayer.Play();

                // Suscribirse al evento loopPointReached para saber cuándo termina el video
                videoPlayer.loopPointReached += OnVideoEnd;
            }
            else
            {
                Debug.LogError("VideoPlayer component not found on this GameObject.");
            }

        }
        else
        {
            playerMovementNew.inputsEnabled = true;
            panelHUD.SetActive(true);   
            panelVideo.SetActive(false);
            if(portalInicio != null) portalInicio.SetActive(true);

        }

        //else if(pasoIntro == true)
        //{
        //    audioPause.Pause(false);
        //    panelHUD.SetActive(true);
        //    playerMovementNew.targetPosition = new Vector3(portalPos.transform.position.x, portalPos.transform.position.y, 0);
        //    playerController.transform.position = new Vector3 (portalPos.transform.position.x, portalPos.transform.position.y, 0);
        //    playerMovementNew.transform.rotation = Quaternion.Euler(0, 0, 0);

        //    //playerController.GetComponent<SpriteRenderer>().color = Color.white;

        //}
        
        if (!UserData.playerGuide1) 
        {
            playerGuide1.SetActive(true);
            UserData.playerGuide1 = true;
        } 
        else
        {
            playerGuide1.SetActive(false);
        }

        if (!UserData.playerGuide2) 
        {
            playerGuide2.SetActive(true);
            UserData.playerGuide2 = true;
        } 
        else
        {
            playerGuide2.SetActive(false);        
        }

        if (!UserData.playerGuide3 && UserData.completoNivel1) 
        {
            playerGuide3.SetActive(true);
            UserData.playerGuide3 = true;
        } 
        else
        {
            playerGuide3.SetActive(false);
        }
    }
    public void SkipInitVideo()
    {
        videoPlayer.Stop();
        OnVideoEnd(videoPlayer);
    }
    private void Update()
    {


        //print("N1: " + UserData.completoNivel1 + ". N2: " + UserData.completoNivel2 + ". N3: " + UserData.completoNivel3);
        //if(Input.GetKeyDown(KeyCode.K))
        //{
        //    OnVideoEnd(videoPlayer);
        //}
        //if (pasoIntro == false)
        if (!UserData.terminoLobby)
        {

            if (InputManager.isPC)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    tapPanel.SetActive(false);
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        tapPanel.SetActive(false);
                    }
                }
            }

        }
    }
    

    // Método llamado cuando el video termina
    void OnVideoEnd(VideoPlayer vp)
    {

        // Lógica para cambiar al juego
        //if (pasoIntro == false)
        if (!UserData.terminoLobby)
        {
            videoPlayer.enabled = false;
            panelVideo.SetActive(false);
            StartGame();
            panelHUD.SetActive(true);
        }
    }

    void StartGame()
    {
        // Aquí puedes agregar la lógica para iniciar tu juego
        // Por ejemplo, puedes desactivar el objeto VideoPlayerObject y activar otro objeto del juego
        PaneoCameraInit();
       // StartCoroutine(AnimatePlayer());
        audioPause.Pause(false);
        AudioManager.Instance.PlayMusic("Bg_Lobby", 0);
    }

    
    public void PaneoCamera()
    {
        
        StartCoroutine( ChangePlayerPosition());
        
    }
    public void PaneoCameraInit()
    {
        //CameraManager.instance.SingleSwapCamera(cameraInit);
        spriteRenderer.color = new Color(1, 1, 1, 0);
       // playerController.AdjustLuminance(0);
        canvasFade.SetActive(true);
        StartCoroutine(PaneoCameraInitAnim());
    }
    private IEnumerator PaneoCameraInitAnim()
    {
        //CameraManager.instance.SingleSwapCamera(cameraInit);
        //yield return new WaitForSeconds(5);
        //panelHUD.SetActive(true);



        float shakeMagnitude = 60;
        float inertiaDuration = .2f;
        float returnDuration = 15f;
        Transform cameraTarget;
        Vector3 originalCameraPosition;
        bool isShaking = false;
        cameraTarget = CameraManager.instance.currentCamera.Follow;
        originalCameraPosition = cameraTarget.position;
        // Inertia movement
        Vector3 inertiaTargetPosition = cameraTarget.position + (Vector3)transform.up * shakeMagnitude;
        float elapsed = 0f;

        //while (elapsed < inertiaDuration)
        //{
        //    cameraTarget.position = Vector3.Lerp(originalCameraPosition, inertiaTargetPosition, elapsed / inertiaDuration);
        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}



        cameraTarget.position = originalCameraPosition;
        isShaking = false;

        // Return to player
        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            cameraTarget.position = Vector3.Lerp(inertiaTargetPosition, originalCameraPosition, elapsed / returnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTarget.position = originalCameraPosition;
        canvasFade.SetActive(false);
        StartCoroutine(AnimatePlayer());

    }
    private IEnumerator AnimatePlayer()
    {

        playerController.GetComponent<GhostController>().enabled = false;
        playerMovementNew.enabled = false;
        playerMovementNew.anim.enabled = false;
        Rigidbody2D playerRigidbody = playerController.GetComponent<Rigidbody2D>();

        if (playerRigidbody != null)
        {
            playerRigidbody.simulated = false;
        }

        // Configuración inicial
        playerController.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z);
        playerController.transform.rotation = Quaternion.Euler(0, 180, 90);
        spriteRenderer.color = new Color(1, 1, 1, 0); // Opacidad 0

        // Variables de tiempo
        float elapsedTime = 0;

        // Posición inicial y final
        Vector3 startPosition = playerController.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, elevationDistance, 0);

        // Elevación y cambio de opacidad simultáneos
        while (elapsedTime < elevationDuration)
        {
            // Interpolación
            playerController.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / elevationDuration);
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, elapsedTime / elevationDuration));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la elevación y opacidad terminen en el estado final exacto
        playerController.transform.position = endPosition;
        spriteRenderer.color = new Color(1, 1, 1, 1); // Opacidad 1

        // Esperar un tiempo en la posición elevada y aplicar el efecto de disolución
        yield return StartCoroutine(PlayerDisolve());
        // Ajustar la posición al descender una unidad en el eje Y
        playerController.transform.position = endPosition - new Vector3(0, 3.5f, 0);
        // Aplicar la rotación final instantáneamente
        playerController.transform.rotation = Quaternion.Euler(0, -180, 0);

        // Mantener la posición elevada durante un tiempo
        yield return new WaitForSeconds(1);

        // Volver a aparecer con el método de solidificación
        yield return StartCoroutine(PlayerSolidify());


        spriteRenderer.color = new Color(1, 1, 1, 1); // Opacidad 1

        playerMovementNew.enabled = true;
        playerMovementNew.anim.enabled = true;
        playerMovementNew.targetPosition = playerController.transform.position;
        playerController.GetComponent<GhostController>().enabled = true;
        playerMovementNew.inputsEnabled = true;

        if (playerRigidbody != null)
        {
            playerRigidbody.simulated = true;
        }

        tapPanel.SetActive(true);
    }




    private IEnumerator ChangePlayerPosition()
    {
        AudioManager.Instance.PlaySfx("HourGlassTransition");
        StartCoroutine(PlayerDisolve());
        yield return new WaitForSeconds(1);
        RelojPanel.SetActive(true);
        CameraManager.instance.SingleSwapCamera(camera2, 5);
        yield return new WaitForSeconds(5);
        //playerMovementNew.rb.position = newPlayerPos.position;
        playerMovementNew.transform.position = new Vector3(newPlayerPos.position.x, newPlayerPos.position.y, 0);
        playerMovementNew.targetPosition = playerController.transform.position;
        playerMovementNew.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerMovementNew.inputsEnabled = true;
        //playerMovementNew.anim.SetBool("SlowWalk", true);
        StartCoroutine(PlayerSolidify());
      //  panelHUD.SetActive(true);

        PlayerPrefs.SetInt("pasoIntro", 1);
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
        spriteRenderer.sortingOrder = 2;

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

}
