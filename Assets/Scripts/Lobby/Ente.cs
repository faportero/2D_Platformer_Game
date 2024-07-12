using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ente : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    private PlayerMovementNew playerMovementNew;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerMovementNew.isMoving = false; // Detener el movimiento
            playerMovementNew.anim.SetBool("SlowWalk", false); // Desactivar animación de caminar
            dialoguePanel.SetActive(true);
        }
    }
}
