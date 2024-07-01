using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Espejo : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, p3, p4;
    [SerializeField] UI_Piezas piezasPanel;

    private static int countPiezas;
    private int maxPiezas = 4;
    public static bool isChecked;

    private void Start()
    {
       // CheckEspejoPieces();
        //CheckEspejoPiecesInit();
    }
    private void Update()
    {
        CheckEspejoPiecesInit();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CheckEspejoPieces();
            isChecked = true;
            if(countPiezas != maxPiezas)
            {
                //CheckEspejoPieces();
                int piezasRestantes = maxPiezas - countPiezas;
                print("Te faltan " + piezasRestantes);
            }
            else
            {
                print("Piezas complatas");
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.tag == "Player")
    //        isChecked = false;


    //}

    private void CheckEspejoPieces()
    {
        if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
        {
            //PlayerControllerNew.piezaA = false;
            p1.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaA.GetComponent<Image>().color = Color.black;           
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPA = true;
        }
        if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        {
            //PlayerControllerNew.piezaB = false;
            p2.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaB.GetComponent<Image>().color = Color.black;           
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPB = true;
        }
        if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        {
           // PlayerControllerNew.piezaC = false;
            p3.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaC.GetComponent<Image>().color = Color.black;           
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPC = true;
        }
        if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        {
           // PlayerControllerNew.piezaD = false;
            p4.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaD.GetComponent<Image>().color = Color.black;           
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPD = true;
        }
    } 
    private void CheckEspejoPiecesInit()
    {
        if (LevelManager.usedPA)
        {
            p1.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaA.GetComponent<Image>().color = Color.black;     
        }
        if (LevelManager.usedPB)
        {
            p2.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaB.GetComponent<Image>().color = Color.black;
        }
        if (LevelManager.usedPC)
        {
            p3.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaC.GetComponent<Image>().color = Color.black;
        }
        if (LevelManager.usedPD)
        {
            p4.GetComponent<SpriteRenderer>().color = Color.red;
            piezasPanel.piezaD.GetComponent<Image>().color = Color.black;
        }
    }
}
