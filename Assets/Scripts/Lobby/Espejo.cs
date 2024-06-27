using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espejo : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, p3, p4;

    private void Start()
    {
        if (PlayerControllerNew.piezaA)
        {
            p1.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (PlayerControllerNew.piezaB)
        {
            p1.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (PlayerControllerNew.piezaC)
        {
            p1.GetComponent<SpriteRenderer>().color = Color.red;
        }
        if (PlayerControllerNew.piezaD)
        {
            p1.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
