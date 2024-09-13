using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideColliderExit : MonoBehaviour
{
    [SerializeField] PlayerGuide playerGuide;
    //private GameObject swipe;

    //private void Awake()
    //{
    //    swipe = GameObject.FindGameObjectWithTag("Swipe");
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        if (playerGuide != null && collision.CompareTag("Player")) 
        {
            //swipe.SetActive(true);
            playerGuide.HidePlayerGuide();
        }
    }

}
