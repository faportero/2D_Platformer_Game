using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Health;

public class UI_FeedbackSalud : MonoBehaviour
{
    [SerializeField] private List<Sprite> feedbackSprites = new List<Sprite>();
    private Health health;
    public Image image; 

    private void Awake()
    {
       health = FindAnyObjectByType<Health>();
       //health = GameObject.FindGameObjectWithTag("Salud");
        //image = gameObject.transform.GetChild(0).GetComponent<Image>();
        image = GetComponent<Image>();
    }
    private void Update()
    {
        //AssignFeedbackSprite();
    }
    public void AssignFeedbackSprite()
    {
        switch (health.GetComponent<Health>().healthType)
        {
            case HealthType.Futbol:
                image.sprite = feedbackSprites[0];
                break;
            case HealthType.Gym:
                image.sprite = feedbackSprites[1];
                break;
            case HealthType.Trotar:
                image.sprite = feedbackSprites[2];
                break;
            case HealthType.No:
                image.sprite = feedbackSprites[3];
                break;
            case HealthType.SoloHoy:
                image.sprite = feedbackSprites[4];
                break;
            case HealthType.Meditacion:
                image.sprite = feedbackSprites[5];
                break;
            case HealthType.Agua:
                image.sprite = feedbackSprites[6];
                break;
            case HealthType.Manzana:
                image.sprite = feedbackSprites[7];
                break;
            case HealthType.Pescado:
                image.sprite = feedbackSprites[8];
                break;
        }
    }
}
