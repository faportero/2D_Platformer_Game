using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resorte : MonoBehaviour
{
    [SerializeField]private Rigidbody2D rb;
    [SerializeField]private PlayerMovementNew pMovement;
    [SerializeField] private float impulseAmmount = 75;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.tag == "Player" && !pMovement.doingRoll)
        {
            Impulse(impulseAmmount);
            return;
        }
        else if (collision.tag == "Player" && pMovement.doingRoll)
        {
            Impulse(impulseAmmount);
        }
    }

    private void Impulse(float ammount)
    {
        animator.enabled = true;
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * ammount, ForceMode2D.Impulse);
    }

    public void SpringSound()
    {
        AudioManager.Instance.PlaySfx("Resorte");
    }
}
