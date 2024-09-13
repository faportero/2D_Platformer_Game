using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FinalManager : MonoBehaviour
{
    [SerializeField]
    PlayerMovementNew playerMovementNew;
    [SerializeField]
    GameObject explodeObject, explodeClock, obstacle;
    [SerializeField]
    ParticleSystem dustParticle, shineParticle;
    [SerializeField]
    GameObject manoIz, manoDe;
    [SerializeField]
    Animator manoIzAnim, manoDeAnim, minAnim, hourAnim;
    [SerializeField]
    GameObject blockManoIz, blockManoDe;
    [SerializeField]
    GameObject destello, destelloFinal;
    [SerializeField]
    Animator destelloAnim, destelloFinalAnim;
    [SerializeField]
    GameObject PanelFade, PanelVideo;

    [SerializeField]
    Coroutine finalCoroutine, finalShowCoroutine;

    [SerializeField] 
    GameObject[] objectsToShow, objectsToHide;


    [SerializeField]
    CinemachineVirtualCamera finalPanCamera;

    private LobbyManager lobbyManager;

    private void Awake()
    {
        lobbyManager = FindAnyObjectByType<LobbyManager>();
    }
    private void Update()
    {
     //   print( "min: " + minAnim.speed + ". hour: " + hourAnim.speed);

    }
    public void StartFinal()
    {


        if(finalShowCoroutine != null)
        {
            StopCoroutine(finalShowCoroutine);
        }
        finalShowCoroutine = StartCoroutine(FinalCoroutineAnim());

    }
    public void StartVideoFinal()
    {
        if (finalCoroutine != null)
        {
            StopCoroutine(finalCoroutine);
        }
        finalCoroutine = StartCoroutine(FinalShowVideoAnim());
    }

    private IEnumerator FinalCoroutineAnim()
    {
        // Bloqueo de input
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.isMoving = false;
        playerMovementNew.canMove = false;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
        playerMovementNew.anim.SetBool("SlowWalk", false);


        // Invertir reloj
        //minAnim.speed = -1;
        //hourAnim.speed = -1;



        // Espera de 0.5 segundos
        yield return new WaitForSeconds(5f);
        explodeClock.SetActive(true);
        //AudioManager.Instance.PlayMusic("Bg_Lobby2",0);
        AudioManager.Instance.PlaySfx("fn_ExplocionTorre", true);

        yield return new WaitForSeconds(.25f);
        minAnim.SetBool("isInverse", false);
        hourAnim.SetBool("isInverse", false);

        yield return new WaitForSeconds(3f);
        // Activar animación de manos
        manoIzAnim.enabled = true;
        manoDeAnim.enabled = true;
        AudioManager.Instance.PlaySfx("fn_Manos", false);

        // Obtener duración de la animación de la mano derecha
        float rightHandAnimationDuration = 0f;
        foreach (AnimationClip clip in manoDeAnim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "ManoDe") // Reemplaza con el nombre real de la animación de la mano derecha
            {
                rightHandAnimationDuration = clip.length;
                break;
            }
        }

        // Espera a que termine la animación de la mano derecha
        yield return new WaitForSeconds(rightHandAnimationDuration);

        // Explota el muro
        explodeObject.SetActive(true);
        dustParticle.Play();
        shineParticle.Play();
        AudioManager.Instance.PlaySfx("fn_ExplocionRocas", false);

        // Obtener duración de las partículas
        float explosionParticleDuration = dustParticle.main.duration;

        // Espera a que termine la animación de partículas de explosión
        //yield return new WaitForSeconds(explosionParticleDuration);
        yield return new WaitForSeconds(.1f);
        obstacle.SetActive(false);
        yield return new WaitForSeconds(.4f);

        // Se quitan colliders de bloqueo
        blockManoIz.SetActive(false);
        blockManoDe.SetActive(false);

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeDInDestello());
        destelloAnim.enabled = true;
        destelloAnim.Play("Guide");

        HideObjects();
        ShowObjects();


        yield return new WaitForSeconds(1f);
        // Se desbloquea input
        explodeObject.SetActive(false);
        playerMovementNew.inputsEnabled = true;
        playerMovementNew.isMoving = true;
        playerMovementNew.canMove = true;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Dynamic;
        playerMovementNew.anim.SetBool("SlowWalkS", true);
        explodeClock.SetActive(false);

        Invoke("DeactivateDestello", 9);
    }


    private IEnumerator FinalShowVideoAnim()
    {

        // Bloqueo de input
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.isMoving = false;
        playerMovementNew.canMove = false;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
        playerMovementNew.anim.SetBool("SlowWalk", false);
        lobbyManager.StartCoroutine(lobbyManager.PlayerDisolve()); 

        // Espera de 0.5 segundos
        yield return new WaitForSeconds(1.5f);

        // Mostrar destello
        //destelloAnim.enabled = true;
        AudioManager.Instance.PlaySfx("fn_Woosh", false);

        destelloFinal.GetComponent<Animator>().enabled = true;
        destelloFinalAnim.Play("Expand0");
        yield return new WaitForSeconds(1f);
        PanelFade.SetActive(true);
        Animator panelFadeAnim = PanelFade.transform.GetChild(0).GetComponent<Animator>();
        panelFadeAnim.enabled = true;
        panelFadeAnim.SetBool("isInverse", true);
       
        // Esperar 2 segundos después de que comienza la animación de destello
        yield return new WaitForSeconds(1f);

        // Cambiar de cámara
        CameraManager.instance.SingleSwapCamera(finalPanCamera, 6f);

        // Esperar 3 segundos para asegurar que quedan 2 segundos de cambio de cámara cuando activamos el video
        yield return new WaitForSeconds(5f);

        // Activar el video
        PanelFade.SetActive(false);
        PanelVideo.SetActive(true);
        PanelVideo.transform.GetChild(0).GetComponent<VideoPlayer>().Play();

        AudioManager.Instance.ToggleMusic(false);
        AudioManager.Instance.ToggleSFX();
        StopAllCoroutines();
    }
    private void ShowObjects()
    {
        foreach (GameObject obj in objectsToShow)
        {
            obj.SetActive(true);
        }
    }
    private void HideObjects()
    {
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }
    }
    private void DeactivateDestello()
    {
        destello.SetActive(false);
    }

    private IEnumerator FadeDInDestello()
    {
        AudioManager.Instance.PlaySfx("fn_Shine", false);

        destello.SetActive(true);
        Material material = destello.GetComponent<SpriteRenderer>().material;
        float startOpacity = 0f;
        float endOpacity = 3f;
        material.SetFloat("_Opacity", startOpacity);
        float timeElapsed = 0f;
        while (timeElapsed < 1)
        {
            float t = timeElapsed / 1;
            float currentOpacity = Mathf.Lerp(startOpacity, endOpacity, t);
            material.SetFloat("_Opacity", currentOpacity);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        material.SetFloat("_Opacity", endOpacity);
    }
}
