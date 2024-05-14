using UnityEngine;

public class FlappyCollider : MonoBehaviour
{   
    public bool isFlappy = false; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            isFlappy = true;
        }

    }
}
