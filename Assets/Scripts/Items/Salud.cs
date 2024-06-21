using UnityEngine;
using System.Collections.Generic;

public class Salud : MonoBehaviour
{
    public enum HealthType
    {
        Futbol,
        Gym,
        Trotar,
        No,
        SoloHoy,
        Meditacion,
        Agua,
        Manzana,
        Pescado
    }

    public HealthType healthType;
    public List<Sprite> spriteRenderers = new List<Sprite>();
    private PlayerControllerNew playerController;
    [HideInInspector] public int spriteIndex;

    private void OnValidate()
    {
        AssignSprite();
    }
    private void Start()
    {
       playerController = FindAnyObjectByType<PlayerControllerNew>();
    }    
    public void AssignSprite()
    {
        switch (healthType)
        {
            case HealthType.Futbol:               
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
               spriteIndex = 0;
                break;
            case HealthType.Gym:               
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];
               spriteIndex = 1;
                break;
            case HealthType.Trotar:
             
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];
               spriteIndex = 2;
                break;
            case HealthType.No:               
               spriteIndex = 3;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];
                break;
            case HealthType.SoloHoy:                
               spriteIndex = 4;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[4];
                break;
            case HealthType.Meditacion:                
             spriteIndex = 5;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[5];
                break;
            case HealthType.Agua:               
            spriteIndex = 6;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[6];
                break;
            case HealthType.Manzana:               
              spriteIndex = 7;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[7];
                break;
            case HealthType.Pescado:               
              spriteIndex = 8;
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[8];
                break;
        }
    }   
    public void HealthDie()
    {
        Destroy(gameObject, .2f);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision != null && collision.tag == ("Player"))
    //    {
    //       // HealthDie();
    //    }

    //}
}
