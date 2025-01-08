using System.Collections;
using UnityEngine;

public class VortexCollider : MonoBehaviour
{
    private PlayerMovementNew playerMovementNew;
    private BoxCollider2D vortexCollider2D;
    private bool isCollision;

    private void Awake()
    {
        playerMovementNew = FindAnyObjectByType<PlayerMovementNew>();
    }

    private void Start()
    {
        vortexCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isCollision && playerMovementNew != null && playerMovementNew.DetectTap())
        {
            ResetCollider();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
           // playerMovementNew.swipeDetector.enabled = false;
            isCollision = true;
            collision.GetComponent<PlayerMovementNew>().anim.SetBool("Turn", true);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
           // playerMovementNew.swipeDetector.enabled = true;

            isCollision = false;
        }
    }

    private void ResetCollider()
    {
        // Reinicia el collider y la animación inmediatamente
        vortexCollider2D.enabled = false;
        playerMovementNew.anim.SetBool("Turn", false);
        playerMovementNew.anim.SetBool("SlowWalk", true);
        // Reinicia el collider después de un breve retraso para permitir que el jugador se mueva
        Invoke("EnableCollider", .75f); // Ajusta el tiempo según sea necesario
    }

    private void EnableCollider()
    {
        vortexCollider2D.enabled = true;
    }
}
