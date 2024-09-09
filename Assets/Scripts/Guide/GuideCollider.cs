using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCollider : MonoBehaviour
{
    [SerializeField] PlayerGuide playerGuide;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerGuide != null && collision.CompareTag("Player")) 
        {
            if (!playerGuide.isFacingRight)
            {
                playerGuide.playerGuideSprite.flipX = false;
                playerGuide.playerGlowSprite.flipX = false;
                //playerGuide.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                playerGuide.playerGuideSprite.flipX = true;
                playerGuide.playerGlowSprite.flipX = true;
                //playerGuide.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            }
            playerGuide.ShowPlayerGuide();
        }
    }

}
