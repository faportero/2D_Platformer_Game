using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerMovementNew>().doingSmash == true)
            {
                print(collision.gameObject.GetComponent<PlayerMovementNew>().doingSmash);
                GetComponent<Collider2D>().enabled = false;
            }
            else
            {
                collision.gameObject.GetComponent<PlayerMovementNew>().canMove = false;
                collision.gameObject.GetComponent<PlayerMovementNew>().Die();
            }
        }
    }
}
