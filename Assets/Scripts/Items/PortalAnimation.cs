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
           // Time.timeScale = 0.5f;
            PortalAnimatorObject.SetActive(true);
            AudioManager.Instance.PlaySfx("Portal_Appear");
            //gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.tag == "Player")
        //{
        //    Time.timeScale = 1;
          
        //}
    }
}
