using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingCollider : MonoBehaviour
{


    public bool isFalling = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Player"))
        {
            isFalling = true;
        }

    }


    //// Start is called before the first frame update
    //public GameObject player;
    //public float fallingGravity = 1.0f;
    //private Rigidbody2D _rb;


    //private void Start()
    //{
    //    _rb = player.GetComponent<Rigidbody2D>();
        
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == ("Player"))
    //    {
    //        print("entro collider falling");
    //        _rb.gravityScale = fallingGravity;
    //        _rb.velocity += new Vector2(150,0);
                
    //    }
        
    //}

}
