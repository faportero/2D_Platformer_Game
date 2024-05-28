using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Coin : MonoBehaviour
{
    [SerializeField] private UI_Coins uiCoins;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))

        { 
            uiCoins.UpdateCoins(1);
            //CoinDie();
        }
    }

    public void CoinDie()
    {        
        Destroy(gameObject, .2f);
    }
}
