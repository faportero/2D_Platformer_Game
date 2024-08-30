using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [SerializeField] private GameObject armSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            armSpawner.SetActive(true);
            armSpawner.GetComponent<ArmSpawner>().enabled = true;
            BossCollider.isBossLevel = true;
        }
    }

}
