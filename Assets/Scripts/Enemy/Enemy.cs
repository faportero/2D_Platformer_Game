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
    private TextMeshProUGUI effectText;

    public bool isAdict = false;
    private List<Enemy> e;
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
 
    private void Start()
    {
       // Effect();
        //SustanceType choise = SustanceType.Cannabis;
        e = new List<Enemy>();
        effectText = effectPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
           if(e.Count == 0) e = collision.GetComponent<PlayerController>().enemies;
            playerController = collision.GetComponent<PlayerController>();
            playerMovement = collision.GetComponent<PlayerMovementNew>();
            isAdict = true;
            foreach (Enemy enemy in e)
            {
                if (enemy.sustanceType == sustanceType)
                {
                    if (!playerMovement.doingSmash)
                    {
                    collision.GetComponent<PlayerController>().TakeAdiccion(enemy);
                    //print(collision.GetComponent<PlayerController>().currentAdiction);
                    collision.GetComponent<PlayerController>().SaludAmount = 0;
                    collision.GetComponent<PlayerController>().uiSalud.saludCount = 0;
                    collision.GetComponent<PlayerMovementNew>().canSmash = false;
                    collision.GetComponent<PlayerController>().uiSalud.UpdateSalud(0);
                    collision.GetComponent<PlayerController>().LoseLife();
                    Effect();
                    //print(collision.GetComponent<PlayerController>().SaludAmount);
                    //print(collision.GetComponent<PlayerController>().uiSalud.saludCount);
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
        //D
        //estroy(gameObject,1);
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
                effectPanel.GetComponent<Image>().color = new Color (0, 1, 0, .25f);
                effectText.text = "Cannabis";
                break;

            case SustanceType.Cocaina:
                print("Cocaa");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Cocaina";
                break;
            case SustanceType.Extasis:
                print("Exta");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Éxtasis";
                break;
            case SustanceType.Metanfetamina:
                print("Metanfetamina");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Metanfetamina";
                break;
            case SustanceType.Heroina:
                print("Heroinaaaa");
                playerController.isCocaMetaHero = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 1, .25f);
                effectText.text = "Heroina";
                break;

            case SustanceType.Psilocibina:
                print("Psilocibinaaaa");
                playerController.isPsilo = true;
                effectPanel.GetComponent<Image>().color = new Color(.5f, 0, .75f, .25f);
                effectText.text = "Psilocibina";
                break;

            case SustanceType.Alcohol:
                print("Alcoholllll");
                playerController.isAlcohol = true;
                effectPanel.GetComponent<Image>().color = new Color(1, 1, 0, .25f);
                effectText.text = "Alcohol";
                break;
            case SustanceType.Tabaco:
                print("Tabacooooo");
                playerController.isTabaco = true;
                effectPanel.GetComponent<Image>().color = new Color(0, 0, 1, .25f);
                effectText.text = "Tabaco";
                break;

        }
    }
}
