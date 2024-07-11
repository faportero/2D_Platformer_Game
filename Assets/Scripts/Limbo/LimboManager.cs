using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboManager : MonoBehaviour
{
    public static int countVideosWatched;
    [SerializeField] private Transform newPosition;
    [SerializeField] private PlayerControllerNew playerController;
    [SerializeField] private PlayerMovementNew playerMovementNew;


    private void Start()
    {
        //if (countVideosWatched >= 3)
        //{
        //    playerController.transform.position = newPosition.position;
        //    playerMovementNew.targetPosition = playerController.transform.position;
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
         //   countVideosWatched++;

        }
    }
}
