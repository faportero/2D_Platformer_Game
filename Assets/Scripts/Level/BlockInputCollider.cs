using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInputCollider : MonoBehaviour
{
private PlayerMovementNew playerMovementNew;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerMovementNew.inputsEnabled = false;
        }
    }
}
