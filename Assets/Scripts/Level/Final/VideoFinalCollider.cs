using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoFinalCollider : MonoBehaviour
{
    [SerializeField]
    FinalManager finalManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            finalManager.StartVideoFinal();
            return;
        }
    }
}
