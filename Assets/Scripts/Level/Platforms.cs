using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platforms : MonoBehaviour
{
    [SerializeField] private PlayerController pController;
    private Collider2D collision;

    private void Start()
    {
        collision = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (pController.transform.position.y - 1.62 > transform.position.y)
        {
            collision.isTrigger = false;
        }
        else 
        {
            collision.isTrigger = true;
        }

    }

}
