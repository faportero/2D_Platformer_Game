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
    GameObject explodeObject;
    [SerializeField]
    ParticleSystem dustParticle, shineParticle;
    [SerializeField]
    GameObject manoIz, manoDe;
    [SerializeField]
    Animator manoIzAnim, manoDeAnim, minAnim, hourAnim;
    [SerializeField]
    GameObject blockManoIz, blockManoDe;
    [SerializeField]
    GameObject destello;
    [SerializeField]
    Animator destelloAnim;
    [SerializeField]
    GameObject PanelFade, PanelVideo;

    [SerializeField]
    Coroutine finalCoroutine, finalShowCoroutine;

    [SerializeField]
    CinemachineVirtualCamera finalPanCamera;
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

    //private IEnumerator FinalCoroutineAnim()
    //{
    //    //bloqueo input
    //    playerMovementNew.inputsEnabled = false;
    //    playerMovementNew.isMoving = false;
    //    playerMovementNew.canMove = false;
    //    playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
    //    playerMovementNew.anim.SetBool("SlowWalkSlowWalk", false);
    //    yield return null;
    //    // animacion de manos se unen para poder caminar
    //    manoIzAnim.enabled = true;
    //    manoDeAnim.enabled = true;
    //    //se invierte reloj
    //    minAnim.speed = .1f;
    //    hourAnim.speed = .1f;
    //    yield return null;
    //    //explota el muro
    //    explodeObject.SetActive(true);
    //    dustParticle.Play();
    //    shineParticle.Play();
    //    yield return null;
    //    //se quitan colliders de bloqueo
    //    blockManoIz.SetActive(false);
    //    blockManoDe.SetActive(false);
    //    //se deslbloquea input
    //    playerMovementNew.inputsEnabled = true;
    //    playerMovementNew.isMoving = true;
    //    playerMovementNew.canMove = true;
    //    playerMovementNew.rb.bodyType = RigidbodyType2D.Dynamic;
    //    playerMovementNew.anim.SetBool("SlowWalkSlowWalk", true);
    //}

    private IEnumerator FinalCoroutineAnim()
    {
        // Bloqueo de input
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.isMoving = false;
        playerMovementNew.canMove = false;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
        playerMovementNew.anim.SetBool("SlowWalkSlowWalk", false);
        playerMovementNew.anim.SetBool("Walk", false);

        // Espera de 0.5 segundos
        yield return new WaitForSeconds(5f);

        // Activar animación de manos
        manoIzAnim.enabled = true;
        manoDeAnim.enabled = true;

        // Invertir reloj
        minAnim.speed = .1f;
        hourAnim.speed = .1f;

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

        // Obtener duración de las partículas
        float explosionParticleDuration = dustParticle.main.duration;

        // Espera a que termine la animación de partículas de explosión
        yield return new WaitForSeconds(explosionParticleDuration);
        yield return new WaitForSeconds(1);
        explodeObject.SetActive(false);

        // Se quitan colliders de bloqueo
        blockManoIz.SetActive(false);
        blockManoDe.SetActive(false);

        // Se desbloquea input
        playerMovementNew.inputsEnabled = true;
        playerMovementNew.isMoving = true;
        playerMovementNew.canMove = true;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Dynamic;
        playerMovementNew.anim.SetBool("SlowWalkSlowWalk", true);
        destello.SetActive(true);
    }


    //private IEnumerator FinalShowVideoAnim()
    //{
    //    //bloqueo input
    //    playerMovementNew.inputsEnabled = false;
    //    playerMovementNew.isMoving = false;
    //    playerMovementNew.canMove = false;
    //    playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
    //    playerMovementNew.anim.SetBool("SlowWalkSlowWalk", false);
    //    yield return null;
    //    //se muestra destello
    //    destelloAnim.enabled = true;
    //    yield return null;       
    //    //Paneo camara
    //    CameraManager.instance.SingleSwapCamera(finalPanCamera, 5f);
    //    yield return null;
    //    //se activa video
    //    PanelVideo.SetActive(true);
    //    PanelVideo.GetComponent<VideoPlayer>().Play();
    //}

    private IEnumerator FinalShowVideoAnim()
    {
        // Bloqueo de input
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.isMoving = false;
        playerMovementNew.canMove = false;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
        playerMovementNew.anim.SetBool("SlowWalkSlowWalk", false);

        // Espera de 0.5 segundos
        yield return new WaitForSeconds(.5f);

        // Mostrar destello
        destelloAnim.enabled = true;

        // Esperar 2 segundos después de que comienza la animación de destello
        yield return new WaitForSeconds(2f);

        // Cambiar de cámara
        CameraManager.instance.SingleSwapCamera(finalPanCamera, 5f);

        // Esperar 3 segundos para asegurar que quedan 2 segundos de cambio de cámara cuando activamos el video
        yield return new WaitForSeconds(3f);

        // Activar el video
        PanelFade.SetActive(true);
        PanelFade.transform.GetChild(0).GetComponent<Animator>().speed = -1;
        PanelVideo.SetActive(true);
        PanelVideo.GetComponent<VideoPlayer>().Play();
        StopAllCoroutines();
    }


}
