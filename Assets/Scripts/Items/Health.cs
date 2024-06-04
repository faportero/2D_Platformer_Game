using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
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
    [SerializeField] private bool isRandom;
    public List<Sprite> spriteRenderers = new List<Sprite>();
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
       
        if (isRandom) 
            EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        if (isRandom) 
            EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
        if (!Application.isPlaying && transform.position != lastPosition)
        {
            UpdateSprite();
            lastPosition = transform.position;
        }
    }
#endif

    void OnValidate()
    {     
        if(!isRandom) AssignSprite();
    }

    private void UpdateSprite()
    {
        if (spriteRenderers.Count == 0)
        {
            Debug.LogWarning("La lista de sprites está vacía.");
            return;
        }

        if (transform.position != Vector3.zero)
        {
            int randomIndex = Random.Range(0, spriteRenderers.Count);
            spriteRenderer.sprite = spriteRenderers[randomIndex];
        }
        else
        {
            spriteRenderer.sprite = spriteRenderers[0];
        }

#if UNITY_EDITOR
        // Marcar el objeto como modificado para asegurarse de que los cambios se reflejen en el editor
        EditorUtility.SetDirty(spriteRenderer);
#endif
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

    private void HealthDie()
    {
        Destroy(gameObject, .2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            HealthDie();
        }
    }
}
