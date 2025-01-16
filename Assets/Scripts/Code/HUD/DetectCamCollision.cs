using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCamCollision : MonoBehaviour
{
    [SerializeField] private MoveCameraMap _move;
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _move._isCollision = true;
        transform.Translate(-(collision.transform.position - transform.position).normalized * .05f);
        //print("Entró a trigger la cam con: " + collision.gameObject.name);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        _move._isCollision = true;
        //print("Esta en trigger la cam con: " + collision.gameObject.name);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _move._isCollision = false;
        //print("Salió a trigger la cam con: " + collision.gameObject.name);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _move._isCollision = true;
        transform.Translate(-(collision.transform.position - transform.position).normalized * .05f);
        //print("Entró a colisión la cam con: " + collision.gameObject.name);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        _move._isCollision = true;
        //print("Está en colisión la cam con: " + collision.gameObject.name);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _move._isCollision = false;
        //print("Salió a colisión la cam con: " + collision.gameObject.name);
    }
}

