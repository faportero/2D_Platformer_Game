using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMovementNew;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public static Vector3 lastPosition;

    [SerializeField] private float adictionAmount;
    public float currentAdiction;
    [SerializeField] public int coinsAmount = 0;
    [SerializeField] public int SaludAmount = 0;
    public int lifesAmount;
    [SerializeField] private UI_AdiccionBar adiccionBar;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;
    [SerializeField] public UI_Salud uiSalud;
    [HideInInspector] public List <Enemy> enemies;
    [HideInInspector] public List <Ability> abilities;
    public GameObject effectPanel;    
    private PlayerMovementNew playerMovement;
   

    public bool isDie = false;

    public bool isCannabis, isCocaMetaHero, isPsilo, isAlcohol, isTabaco;
    private bool isEnemy;

    private void Awake()
    {
        if (lastPosition != Vector3.zero) 
        {
            transform.position = lastPosition;
            //playerMovement.movementMode = MovementMode.RunnerMode;
        } 
}
    private void Start()
    {
        currentAdiction = adiccionBar.currentAdiccion;

        enemies = new List<Enemy>();
        abilities = new List<Ability>();
        playerMovement = GetComponent<PlayerMovementNew>();
        FindEnemies();
        FindAbilities();
    }

    private void FindEnemies()
    {
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");  
        enemies.Clear();

        foreach (GameObject enemyObject in foundEnemies)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemies.Add(enemy);

            }
        }
    }

    private void FindAbilities()
    {
        GameObject[] foundAbilities = GameObject.FindGameObjectsWithTag("Ability");
        abilities.Clear();

        foreach (GameObject abilityObject in foundAbilities)
        {
            Ability ability = abilityObject.GetComponent<Ability>();
            if (ability != null)
            {
                abilities.Add(ability);

            }
        }
    }

    void Update()
    {
        //print(adiccionBar.currentAdiccion);
       EnemyEffect();
    }

    public void TakeAdiccion(Enemy enemy)
    {
        if(enemy.isAdict)
        {
            //print(adiccionBar.currentAdiccion);
            adiccionBar.UpdateAdiccion(adictionAmount);
            enemy.Effect();
            enemy.EnemyDie();
            currentAdiction = adiccionBar.currentAdiccion;           

        }
       
    }
    public void TakeAbility(Ability ability)
    {     
        //print(adiccionBar.currentAdiccion);
        adiccionBar.UpdateAdiccion(adictionAmount);
        ability.NewAbility();
        ability.AbilityDie();
        currentAdiction = adiccionBar.currentAdiccion;

    }
    public void TakeUICoin()
    {
       uiCoins.UpdateCoins();
    }

    public void TakeSalud()
    {
        SaludAmount += 1;
        SaludAmount = Mathf.Clamp(SaludAmount, 0, 3);
        if(SaludAmount == 3)
        {
            playerMovement.canSmash = true;
            //uiSalud.saludCount = 0;
            //SaludAmount = 0;         
            
        }
        else
        {
            playerMovement.canSmash = false;

        }

    }
    public void LoseLife()
    {
        if (lifesAmount > 0)
        {
            //print(lifesAmount);
            lifesAmount -= 1;
            //   lifesAmount = Mathf.Clamp(lifesAmount, 0, maxLifes);            
            uiLifes.UpdateLife();

        }
        else
        {
            playerMovement.canMove = false;
            playerMovement.Die();
        }
    }

    private void EnemyEffect()
    {
        if (!isDie)
        {

            if(isCannabis)
            {

                StartCoroutine(CurrentEffect(5));
       
            }
            else if(isCocaMetaHero)
            {
                StartCoroutine(CurrentEffect(5));
            
            }
            else if(isAlcohol)
            {

                StartCoroutine(CurrentEffect(5));
            
            }
            else if(isTabaco)
            {
                StartCoroutine(CurrentEffect(5));     
                
            }
        }
    }
    private IEnumerator CurrentEffect(float delay) 
    {
        //isCannabis = false;
        //isCocaMetaHero = false;
        //isPsilo = false;
        //isAlcohol = false;
        //isTabaco = false;
        effectPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        StopAllCoroutines();
        effectPanel.SetActive(false);
        isCannabis = false;
        isCocaMetaHero = false;
        isPsilo = false;
        isAlcohol = false;
        isTabaco = false;
        //if (isCannabis)
        //{
        //    isCannabis = false;           
        //}
        //else if (isCocaMetaHero)
        //{
        //    isCocaMetaHero = false;
        //}
        //else if (isPsilo)
        //{
        //    isPsilo = false;
        //}
        //else if (isAlcohol)
        //{
        //    isAlcohol = false;
        //}
        //else if (isTabaco)
        //{
        //    isTabaco = false;

        //}


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            print("badlayer" + collision.gameObject.name);
            LoseLife();

        }
        if(collision.tag == "Salud")
        {
            if(currentAdiction != 0)
            {
                currentAdiction -= adictionAmount;
                adiccionBar.UpdateAdiccion(-adictionAmount);
               // print(adiccionBar.adiccionFillBar.fillAmount);
            }
            if (currentAdiction <= 0)
            {
                uiSalud.UpdateSalud(1);
                TakeSalud();
                //print(currentAdiction);
            }

           
        }

        //if (collision.tag == "Enemy")
        //{            
        //    isEnemy = true;
        //}

        //if (collision.tag == "CheckPoint")
        //{
        //    print("cheeeeeeeeeeeeeeck");
        //    lastPosition = collision.transform.position;
        //}

    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Enemy")
        //{
        //    isEnemy = false;
        //}
       
    }

}
