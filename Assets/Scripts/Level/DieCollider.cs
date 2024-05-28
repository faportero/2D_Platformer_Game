using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCollider : MonoBehaviour
{

    [SerializeField] private PlayerMovementNew playerMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            print(collision.gameObject);
            playerMovement.canMove = false;
            playerMovement.rb.velocity = Vector3.zero;
            playerMovement.rb.gravityScale = 0;
            playerMovement.Die();
        }
    }
}
