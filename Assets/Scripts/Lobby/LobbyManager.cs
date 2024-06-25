using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LobbyManager : MonoBehaviour
{
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


    void Start()
    {

        playerController = FindAnyObjectByType<PlayerControllerNew>();
        playerMaterial = playerController.GetComponent<SpriteRenderer>().material;
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();

        audioPause.Pause(true);
        // Obtener el componente VideoPlayer del objeto actual
       

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
    private void Update()
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

    // Método llamado cuando el video termina
    void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        StartGame();
    }

    void StartGame()
    {
        // Aquí puedes agregar la lógica para iniciar tu juego
        // Por ejemplo, puedes desactivar el objeto VideoPlayerObject y activar otro objeto del juego
        panelVideo.SetActive(false);
        audioPause.Pause(false);
        tapPanel.SetActive(true);

    }
    public void PaneoCamera()
    {
        
        StartCoroutine(ChangePlayerPosition());
        
    }

    private IEnumerator ChangePlayerPosition()
    {
        StartCoroutine(PlayerDisolve());
        yield return new WaitForSeconds(1);
        RelojPanel.SetActive(true);
        CameraManager.instance.SingleSwapCamera(camera2);
        yield return new WaitForSeconds(5);
        playerMovementNew.rb.position = newPlayerPos.position;
        //playerMovementNew.transform.position = newPlayerPos.position;
        //playerMovementNew.inputsEnabled = true;
        //playerMovementNew.anim.SetBool("SlowWalk", true);
        StartCoroutine(PlayerSolidify());
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
