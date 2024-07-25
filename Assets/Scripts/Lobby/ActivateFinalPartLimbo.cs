using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFinalPartLimbo : MonoBehaviour
{
    [SerializeField] private Espejo espejo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            espejo.StartCoroutine(espejo.ShowVideoPanel()); 
        }
    }
}
