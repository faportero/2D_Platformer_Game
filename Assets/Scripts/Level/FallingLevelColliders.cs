using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLevelColliders : MonoBehaviour
{
    public bool isFallColliding;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            isFallColliding = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Player")
        {
            isFallColliding = false;
        }
    }
}
