using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float adictionAmount = .25f;
    [HideInInspector] public int coinsAmount = 0;
    [SerializeField] private UI_AdiccionBar adiccionBar;
    [SerializeField] private UI_Coins uiCoins;
    [HideInInspector] public List <Enemy> enemies;


    private void Start()
    {
        enemies = new List<Enemy>();
        FindEnemies();
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

    void Update()
    {
       
    }

    public void TakeAdiccion(Enemy enemy)
    {
        if(enemy.isAdict)
        {
            adiccionBar.UpdateAdiccion(adictionAmount);
            enemy.Effect();
            enemy.EnemyDie();            

        }
       
    }

    public void TakeCoin()
    {
       uiCoins.UpdateCoins();
    }
}
