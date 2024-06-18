using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    private int hitsCountEscudo;
    [SerializeField] private int maxHits;
    private PlayerController playerController;
    private void Start()
    {
        //Physics2D.IgnoreCollision(,8);  
        //playerController = FindAnyObjectByType<PlayerController>();
    }
    private void Update()
    {
        //if (isActiveAndEnabled)
        //{
        //    transform.position = playerController.transform.position;
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {     
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().EnemyDie();
            hitsCountEscudo += 1;

            if (hitsCountEscudo == maxHits)
            {
                gameObject.SetActive(false);
                collision.enabled = false;
                //playerController.escudo = false;
            }
        }
    }
}
