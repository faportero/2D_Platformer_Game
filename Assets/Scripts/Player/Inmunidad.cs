using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    private int hitsCountEscudo;
    [SerializeField] private int maxHits;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().EnemyDie();
            hitsCountEscudo += 1;

            if (hitsCountEscudo == maxHits)
            {
                gameObject.SetActive(false);
                collision.enabled = false;
            }
        }
    }
}
