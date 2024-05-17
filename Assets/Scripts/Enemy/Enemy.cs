using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isAdict = false;
    private List<Enemy> e;

    public enum SustanceType
    {
        Cannabis,
        CocaExtMeta,
        Heroina,
        Psilocibina,
        Pegamento,
        Alcohol,
        Tabaco
    }
    public SustanceType sustanceType;
 
    private void Start()
    {
       // Effect();
        //SustanceType choise = SustanceType.Cannabis;
        e = new List<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
           if(e.Count == 0) e = collision.GetComponent<PlayerController>().enemies;
            isAdict = true;
            foreach (Enemy enemy in e)
            {
                if (enemy.sustanceType == sustanceType)
                {
                    collision.GetComponent<PlayerController>().TakeAdiccion(enemy);
                    print(enemy);
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
        //gameObject.SetActive(false);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject);
        isAdict = false;
    }

    public void Effect()
    {
        //sustanceType = SustanceType.Cannabis;
        switch (sustanceType)
        {
            case SustanceType.Cannabis:
                print("cannabisssss");
                break;
            case SustanceType.CocaExtMeta:
                print("CocaExtMetaaaaa");
                break;
            case SustanceType.Heroina:
                print("Heroinaaaa");
                break;
            case SustanceType.Psilocibina:
                print("Psilocibinaaaa");
                break;
            case SustanceType.Pegamento:
                print("Pegamentoooo");
                break;
            case SustanceType.Alcohol:
                print("Alcoholllll");
                break;
            case SustanceType.Tabaco:
                print("Tabacooooo");
                break;

        }
    }
}
