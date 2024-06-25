using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ente : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    private PlayerMovementNew playerMovement;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            dialoguePanel.SetActive(true);
        }
    }
}
