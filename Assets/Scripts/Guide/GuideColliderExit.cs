using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideColliderExit : MonoBehaviour
{
    [SerializeField] PlayerGuide playerGuide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerGuide != null && collision.CompareTag("Player")) 
        {
            playerGuide.HidePlayerGuide();
        }
    }

}
