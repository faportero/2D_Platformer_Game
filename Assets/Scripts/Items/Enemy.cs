using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public GameObject effectPanel;
    public GameObject effectPanelMensaje;
    private TextMeshProUGUI effectText;
    private TextMeshProUGUI effectTextMensajeTitulo;
    private TextMeshProUGUI effectTextMensajeDesc;

    public bool isAdict = false;
    private List<Enemy> e;
    [SerializeField] private List <Sprite> spriteRenderers = new List<Sprite>();
    private PlayerController playerController;
    private PlayerMovementNew playerMovement;

    public enum SustanceType
    {
        Cannabis,
        Cocaina,
        Extasis,
        Metanfetamina,
        Heroina,
        Psilocibina,
        Alcohol,
        Tabaco
    }
    public SustanceType sustanceType;

    private void Awake()
    {
        AssignSprite();

    }
    private void OnValidate()
    {
        AssignSprite();
    }
    private void Start()
    {
        e = new List<Enemy>();
        effectText = effectPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        effectTextMensajeTitulo = effectPanelMensaje.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        effectTextMensajeDesc = effectPanelMensaje.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            if (e.Count == 0) e = collision.GetComponent<PlayerController>().enemies;
            playerController = collision.GetComponent<PlayerController>();
            playerMovement = collision.GetComponent<PlayerMovementNew>();
            isAdict = true;
            foreach (Enemy enemy in e)
            {
                if (enemy.sustanceType == sustanceType)
                {
                    if (!playerMovement.doingSmash && !playerController.escudo)
                    {
                        collision.GetComponent<PlayerController>().TakeAdiccion(enemy);
                        collision.GetComponent<PlayerController>().SaludAmount = 0;
                        collision.GetComponent<PlayerController>().uiSalud.saludCount = 0;
                        collision.GetComponent<PlayerMovementNew>().canSmash = false;
                        collision.GetComponent<PlayerController>().uiSalud.UpdateSalud(0);
                        collision.GetComponent<PlayerController>().LoseLife();
                        Effect();
                    }
                    else
                    {
                        EnemyDie();
                    }
                }
            }

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isAdict = false;
    }
    public void EnemyDie()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);
        isAdict = false;
    }

    public void Effect()
    {

        //sustanceType = SustanceType.Cannabis;
        switch (sustanceType)
        {
            case SustanceType.Cannabis:
                print("cannabisssss");
                playerController.isCannabis = true;
                effectPanel.GetComponent<Image>().color = new Color(0, 1, 0, .25f);
                effectText.text = "Cannabis";
                effectTextMensajeTitulo.text = "Cannabis";
                effectTextMensajeDesc.text = "El cannabis te hace mover mas lento";
                break;

            case SustanceType.Cocaina:
                print("Cocaa");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Cocaina";
                effectTextMensajeTitulo.text = "Cocaina";
                effectTextMensajeDesc.text = "La cocaina te hace saltar como loco";
                break;
            case SustanceType.Extasis:
                print("Exta");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Éxtasis";
                effectTextMensajeTitulo.text = "Extasis";
                effectTextMensajeDesc.text = "El extasis te hace saltar como loco";
                break;
            case SustanceType.Metanfetamina:
                print("Metanfetamina");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Metanfetamina";
                effectTextMensajeTitulo.text = "Metanfetamina";
                effectTextMensajeDesc.text = "La metanfetamina te hace saltar como loco";
                break;
            case SustanceType.Heroina:
                print("Heroinaaaa");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Heroina";
                effectTextMensajeTitulo.text = "Heroina";
                effectTextMensajeDesc.text = "La heroina te hace saltar como loco";
                break;

            case SustanceType.Psilocibina:
                print("Psilocibinaaaa");
                playerController.isPsilo = true;
                effectPanel.GetComponent<Image>().color = new Color(.5f, 0, .75f, .25f);
                effectText.text = "Psilocibina";
                effectTextMensajeTitulo.text = "Psilocibina";
                effectTextMensajeDesc.text = "La psilocibina te altera la persepcion de la realidad";
                break;

            case SustanceType.Alcohol:
                print("Alcoholllll");
                playerController.isAlcohol = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 0, .25f);
                effectText.text = "Alcohol";
                effectTextMensajeTitulo.text = "Alcohol";
                effectTextMensajeDesc.text = "El alcohol te vuelve torpe";
                break;
            case SustanceType.Tabaco:
                print("Tabacooooo");
                playerController.isTabaco = true;
                effectPanel.GetComponent<Image>().color = new Color(0, 0, 1, .25f);
                effectText.text = "Tabaco";
                effectTextMensajeTitulo.text = "Tabaco";
                effectTextMensajeDesc.text = "El tabaco te hace toser fuerte";
                break;

        }
    }

    private void AssignSprite()
    {
        switch (sustanceType)
        {
            case SustanceType.Cannabis:

                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[0];
                break;

            case SustanceType.Cocaina:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[1];

                break;
            case SustanceType.Extasis:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[2];

                break;
            case SustanceType.Metanfetamina:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[3];

                break;
            case SustanceType.Heroina:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[4];

                break;

            case SustanceType.Psilocibina:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[5];

                break;

            case SustanceType.Alcohol:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[6];

                break;
            case SustanceType.Tabaco:
                gameObject.GetComponent<SpriteRenderer>().sprite = spriteRenderers[7];

                break;

        }
    }
}
