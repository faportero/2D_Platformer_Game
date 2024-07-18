using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimation : MonoBehaviour
{
    [SerializeField] GameObject PortalAnimatorObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PortalAnimatorObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
