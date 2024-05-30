using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private enum HealthType
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
    [SerializeField] private HealthType healthType;
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
        switch (healthType)
        {
            case HealthType.Futbol:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
                break;

            case HealthType.Gym:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];
                break;
            case HealthType.Trotar:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];
                break;
            case HealthType.No:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];
                break;
            case HealthType.SoloHoy:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[4];
                break;
            case HealthType.Meditacion:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[5];
                break;
            case HealthType.Agua:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[6];
                break;
            case HealthType.Manzana:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[7];
                break;
            case HealthType.Pescado:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[8];
                break;
        }
    }
}
