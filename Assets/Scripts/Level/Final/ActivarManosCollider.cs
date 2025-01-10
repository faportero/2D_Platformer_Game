using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarManosCollider : MonoBehaviour
{
    [SerializeField]
    FinalManager finalManager;
    private PlayerMovementNew playerMovementNew;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(playerMovementNew.swipeDetector != null) playerMovementNew.swipeDetector.gameObject.SetActive(true);
            finalManager.StartFinal();
            GetComponent<BoxCollider2D>().enabled = false;
            return;
        }        
    }
}
