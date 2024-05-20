using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            collision.gameObject.transform.position = new Vector3(-140,-0.4f, 0);
        }
    }
}
