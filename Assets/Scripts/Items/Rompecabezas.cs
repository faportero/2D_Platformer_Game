using System;
using System.Collections.Generic;
using UnityEngine;

public class Rompecabezas : MonoBehaviour
{
    private enum RompecabezasType
    {
        RompecabezasA,
        RompecabezasB,
        RompecabezasC,
        RompecabezasD

    }
    [SerializeField] private RompecabezasType rompecabezasType;
    [SerializeField] private List<Sprite> spriteRenderers = new List<Sprite>();

    private void Awake()
    {
        AssignSprite();

    }
    private void OnValidate()
    {
        AssignSprite();
    }

    private void AssignSprite()
    {
        switch (rompecabezasType)
        {
            case RompecabezasType.RompecabezasA:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
                break;

            case RompecabezasType.RompecabezasB:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];
                break;
            case RompecabezasType.RompecabezasC:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];
                break;
            case RompecabezasType.RompecabezasD:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.tag == "Player")
        {
            RompecabezasDie();       
        }
    }

    private void RompecabezasDie()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;   
    }
}
