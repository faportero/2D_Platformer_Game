using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInputCollider : MonoBehaviour
{
    [SerializeField] private bool blockInput;
private PlayerMovementNew playerMovementNew;

    private void Start()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            BlockInput(blockInput);
        }
    }
    
    private void BlockInput (bool value)
    {
        playerMovementNew.inputsEnabled = value;

    }
}
