using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_FeedbackSalud : MonoBehaviour
{
    public List<Sprite> feedbackSprites = new List<Sprite>();
    private Salud health;
    public Image image; 
    [SerializeField] private PlayerControllerNew playerControllerNew;
    private void Awake()
    {
        // health = FindAnyObjectByType<Salud>();
       //health = GameObject.FindGameObjectWithTag("Salud");
        //image = gameObject.transform.GetChild(0).GetComponent<Image>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
       // playerControllerNew = FindAnyObjectByType<PlayerControllerNew>();
    }
    private void Update()
    {
       // AssignFeedbackSprite();
     //  health = playerControllerNew.currentItemSalud;
    }
    public void AssignFeedbackSprite()
    {
        //switch (health.healthType)
        switch (playerControllerNew.currentItemSalud.healthType)
        {
            case Salud.HealthType.Futbol:
                image.sprite = feedbackSprites[0];
                break;
            case Salud.HealthType.Gym:
                image.sprite = feedbackSprites[1];
                break;
            case Salud.HealthType.Trotar:
                image.sprite = feedbackSprites[2];
                break;
            case Salud.HealthType.No:
                image.sprite = feedbackSprites[3];
                break;
            case Salud.HealthType.SoloHoy:
                image.sprite = feedbackSprites[4];
                break;
            case Salud.HealthType.Meditacion:
                image.sprite = feedbackSprites[5];
                break;
            case Salud.HealthType.Agua:
                image.sprite = feedbackSprites[6];
                break;
            case Salud.HealthType.Manzana:
                image.sprite = feedbackSprites[7];
                break;
            case Salud.HealthType.Pescado:
                image.sprite = feedbackSprites[8];
                break;
        }
    }
}
