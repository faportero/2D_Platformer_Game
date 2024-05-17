using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float adictionAmount;
    private float currentAdiction;
    [HideInInspector] public int coinsAmount = 0;
    public int lifesAmount;
    [SerializeField] private UI_AdiccionBar adiccionBar;
    [SerializeField] private UI_Coins uiCoins;
    [SerializeField] private UI_Lifes uiLifes;
    [HideInInspector] public List <Enemy> enemies;
    [HideInInspector] public List <Ability> abilities;
    private PlayerMovement playerMovement;
   

    public bool isDie = false;   

    private void Start()
    {
        currentAdiction = adiccionBar.currentAdiccion;

        enemies = new List<Enemy>();
        abilities = new List<Ability>();
        playerMovement = GetComponent<PlayerMovement>();
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
        print(adiccionBar.currentAdiccion);
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
    public void LoseLife()
    {
        if (lifesAmount > 0)
        {
            print(lifesAmount);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            print("badlayer" + collision.gameObject.name);
            LoseLife();

        }
    }

}
