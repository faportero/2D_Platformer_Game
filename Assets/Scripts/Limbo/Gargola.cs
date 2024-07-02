using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Video;

public class Gargola : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayerPlane;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private CinemachineVirtualCamera camera1, camera2;
    private PlayerMovementNew playerMovementNew;
    private PlayerControllerNew playerController;
    private Animator animator;


    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
        playerController = FindAnyObjectByType<PlayerControllerNew>();
        animator = videoPlayerPlane.GetComponent<Animator>();    
        videoPlayer.loopPointReached += OnVideoEnd;
    }
    private void Update()
    {


    }
    void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        CameraManager.instance.SingleSwapCamera(camera1);
        playerMovementNew.inputsEnabled = true;

    }
    private IEnumerator ShowVideoPanel()
    {
        //GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2);
        animator.enabled = true;
        videoPlayerPlane.SetActive(true);
        yield return new WaitForSeconds(.5f);
        videoPlayer.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerMovementNew.anim.SetBool("SlowWalk", false);
            //playerMovementNew.targetPosition = new Vector3(playerMovementNew.transform.position.x, playerMovementNew.transform.position.y, 0);
            playerMovementNew.targetPosition = playerController.transform.position;
            CameraManager.instance.SingleSwapCamera(camera2);
            
            if (LimboManager.countVideosWatched != 3)
            {
                StartCoroutine(ShowVideoPanel());
                videoPlayer.Play();
            }
            else if (LimboManager.countVideosWatched >= 3)
            {
                videoPlayerPlane.SetActive(true);
                videoPlayerPlane.GetComponent<Animator>().enabled = true;
                videoPlayerPlane.GetComponent<Animator>().Play("IdlePlayer");
                videoPlayer.Play();
            }
        }


        }
    }

