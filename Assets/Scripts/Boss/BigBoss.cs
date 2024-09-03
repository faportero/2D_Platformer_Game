using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
public class BigBoss : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera runnerCam;

    private PlayerMovementNew playerMovementNew;
    private Coroutine bossSolidify, bossDissolve;
    private Material material;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void OnEnable()
    {
        material = GetComponent<SpriteRenderer>().material;
        BossSolidify();
        CameraManager.instance.StartCameraShake(5);
        
    }
    private void OnDisable()
    {
        //material = GetComponent<SpriteRenderer>().material;
        //BossDisolve();
    }
    //private void Update()
    //{
    //    transform.position = new Vector3(playerMovementNew.transform.position.x + 8.4f,
    //                                  transform.position.y,
    //                                  0f);
    //}


    public float smoothTime = 0.3f; // Tiempo que tarda en alcanzar la posición objetivo
    private float velocityX = 0.0f; // Velocidad inicial en X (se modifica automáticamente)
    public bool isfollowPlayer = true;

    void Update()
    {
        if (isfollowPlayer)
        {

            float targetX = playerMovementNew.transform.position.x;
            float smoothX = Mathf.SmoothDamp(transform.position.x, targetX + 12.0f, ref velocityX, smoothTime);

            transform.position = new Vector3(smoothX, transform.position.y, 0f);
        }
    }

    public void BossSolidify()
    {
        if (bossSolidify != null)
        {
            StopCoroutine(bossSolidify);
        }
        bossSolidify = StartCoroutine(BossSolidifyAnim());
    }
    public void BossDisolve()
    {
        if (bossDissolve != null)
        {
            StopCoroutine(bossDissolve);
        }
        bossDissolve = StartCoroutine(BossDisolveAnim());
    }
    private IEnumerator BossSolidifyAnim()
    {
        AudioManager.Instance.PlaySfx("Solidify");

        float dissolveAmount = 0;
        float duration = 2f;  
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(1, 0, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        material.SetFloat("_DissolveAmmount", 0);
        StartCoroutine(StartRunner());
    }

    private IEnumerator BossDisolveAnim()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = .5f;  
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }
        material.SetFloat("_DissolveAmmount", 1);
        gameObject.SetActive(false);
    }

    private IEnumerator StartRunner()
    {
        CameraManager.instance.SingleSwapCamera(runnerCam, 2);
        yield return new WaitForSeconds(2);
        //CameraManager.instance.StartCameraShake(0);
        //CameraManager.instance.StopCameraShake();
        playerMovementNew.inputsEnabled = true;
        playerMovementNew.isMoving = true;
        playerMovementNew.canMove = true;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Dynamic;
        //playerMovementNew.anim.SetBool("SlowWalk", true);
        playerMovementNew.anim.SetBool("Walk", true);
    }
    

}
