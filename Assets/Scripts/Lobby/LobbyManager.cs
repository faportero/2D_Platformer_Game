using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject panelHUD;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioPause audioPause;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject tapPanel;
    [SerializeField] private GameObject RelojPanel;
    [SerializeField] private Transform newPlayerPos;

    [SerializeField] private SwipeDetector swipeDetector;
    [SerializeField] private CinemachineVirtualCamera camera2;

    private PlayerControllerNew playerController;
    private PlayerMovementNew playerMovementNew;
    private Material playerMaterial;


    public float elevationDistance = 2f; // Distancia de elevación
    public float elevationDuration = 2f; // Duración de la elevación y cambio de opacidad
    public float rotationDuration = 1f;  // Duración de la rotación
    private SpriteRenderer spriteRenderer;
    public Transform pivot;
    public Transform portalPos;

    public static bool intro;
    private void Awake()
    {
        PlayerPrefs.GetInt("Intro");
        if(PlayerPrefs.GetInt("Intro") != 0)
        {
            intro = false;
        }
        else
        {
            intro = true;
        }

    }
    void Start()
    {

        playerController = FindAnyObjectByType<PlayerControllerNew>();
        playerMaterial = playerController.GetComponent<SpriteRenderer>().material;
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        spriteRenderer = playerController.GetComponent<SpriteRenderer>();
        audioPause.Pause(true);
        // Obtener el componente VideoPlayer del objeto actual


        if (intro == false)
        {


            if (videoPlayer != null)
            {
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
            playerMovementNew.targetPosition = new Vector3(portalPos.transform.position.x, portalPos.transform.position.y, 0);
            playerController.transform.position = new Vector3 (portalPos.transform.position.x, portalPos.transform.position.y, 0);
            playerMovementNew.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
    private void Update()
    {
        if (intro == false)
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
                if (swipeDetector.TapPerformed == true)
                {
                    tapPanel.SetActive(false);
                }

            }
        }
    }

    // Método llamado cuando el video termina
    void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        if (intro == false) StartGame();
    }

    void StartGame()
    {
        // Aquí puedes agregar la lógica para iniciar tu juego
        // Por ejemplo, puedes desactivar el objeto VideoPlayerObject y activar otro objeto del juego
        StartCoroutine(AnimatePlayer());
        panelVideo.SetActive(false);
        audioPause.Pause(false);
        

    }

    
    public void PaneoCamera()
    {
        
        StartCoroutine(ChangePlayerPosition());
        
    }

    private IEnumerator AnimatePlayer()
    {

        playerMovementNew.enabled = false;
        playerMovementNew.anim.enabled = false;
        //playerMovementNew.anim.SetBool("SlowWalk", false);
        // Configuración inicial
        playerController.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z);
        pivot.transform.rotation = Quaternion.Euler(0, 180, 90);
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
        playerController.transform.position = playerController.transform.position;
        spriteRenderer.color = new Color(1, 1, 1, 1); // Opacidad 1

        // Reiniciar el tiempo para la rotación
        elapsedTime = 0;

        // Rotación inicial y final
        Quaternion startRotation = pivot.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 180, 0);

        // Rotación
        while (elapsedTime < rotationDuration)
        {
            // Interpolación
            pivot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la rotación termine en el estado final exacto
        pivot.transform.rotation = endRotation;
        playerMovementNew.enabled = true;
        playerMovementNew.anim.enabled = true;
        playerMovementNew.targetPosition = playerController.transform.position;

        tapPanel.SetActive(true);


    }

    private IEnumerator ChangePlayerPosition()
    {
        StartCoroutine(PlayerDisolve());
        yield return new WaitForSeconds(1);
        RelojPanel.SetActive(true);
        CameraManager.instance.SingleSwapCamera(camera2);
        yield return new WaitForSeconds(5);
        //playerMovementNew.rb.position = newPlayerPos.position;
        playerMovementNew.transform.position = new Vector3(newPlayerPos.position.x, newPlayerPos.position.y, 0);
        playerMovementNew.targetPosition = playerController.transform.position;
        playerMovementNew.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerMovementNew.inputsEnabled = true;
        //playerMovementNew.anim.SetBool("SlowWalk", true);
        StartCoroutine(PlayerSolidify());
        panelHUD.SetActive(true);
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

    private IEnumerator PlayerSolidify()
    {
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
