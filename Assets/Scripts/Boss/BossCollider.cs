using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    [SerializeField] GameObject boss;
    PlayerMovementNew playerMovementNew;
    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(StopPlayer());
            boss.SetActive(true);
        }
    }

    private IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(.5f);
        playerMovementNew.inputsEnabled = false;
        playerMovementNew.isMoving = false;
        playerMovementNew.canMove = false;
        playerMovementNew.rb.bodyType = RigidbodyType2D.Static;
        playerMovementNew.anim.SetBool("SlowWalk", false); 
        playerMovementNew.anim.SetBool("Walk", false); 
    }
}
