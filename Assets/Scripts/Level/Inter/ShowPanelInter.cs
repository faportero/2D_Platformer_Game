using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanelInter : MonoBehaviour
{
    [SerializeField]private GameObject feedbackPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            feedbackPanel.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            feedbackPanel.SetActive(true);
        }
    }
}

