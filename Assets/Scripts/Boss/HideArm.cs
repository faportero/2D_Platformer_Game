using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideArm : MonoBehaviour
{
    [SerializeField] private GameObject armBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            armBoss.SetActive(false);
        }
    }
}
