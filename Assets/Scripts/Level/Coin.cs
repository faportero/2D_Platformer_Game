using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))

        {
            //print("Coin" + UserData.coins);
            //UserData.coins += 1;
            playerController.coinsAmount += 1;
            playerController.TakeUICoin(1); 
            CoinDie();
        }
    }

    public void CoinDie()
    {        
        Destroy(gameObject, .2f);
    }
}
