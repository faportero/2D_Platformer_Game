using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if (collision.gameObject.GetComponent<PlayerMovementNew>().doingSmash == true)
    //        {
    //            print(collision.gameObject.GetComponent<PlayerMovementNew>().doingSmash);
    //            GetComponent<Collider2D>().enabled = false;
    //        }
    //        else
    //        {
    //            collision.gameObject.GetComponent<PlayerMovementNew>().canMove = false;                
    //            collision.gameObject.GetComponent<PlayerController>().LoseLife();
    //            //collision.gameObject.GetComponent<PlayerMovementNew>().Die();
    //        }
    //    }
    //}
    private PlayerMovementNew playerMovement;
   // private PlayerController playerController;
    private PlayerControllerNew playerController;
    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovementNew>();
        //playerController = FindObjectOfType<PlayerController>();
       playerController = FindObjectOfType<PlayerControllerNew>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !playerController.isAttack)
        {
            if (!playerMovement.doingSmash)
            {
                if (playerMovement.canMove)
                {
                playerController.StartBlinking(0);
               // playerController.LoseLife();
                }              
                //collision.gameObject.GetComponent<PlayerController>().LoseLife();
                //return;
            }
            else if (playerMovement.doingSmash)
            {
                WallDie();
            }
        }  
    }

    private void WallDie()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
    }
}
