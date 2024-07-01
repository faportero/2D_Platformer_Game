using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Piezas : MonoBehaviour
{
    public GameObject piezaA, piezaB, piezaC, piezaD;

    private void Start()
    {
       
    }
    private void Update()
    {
        CheckPieces();        
    }
    private void CheckPieces()
    {
        if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
        {
            piezaA.GetComponent<Image>().color = Color.red;
        }
        if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        {
            piezaB.GetComponent<Image>().color = Color.red;
        }
        if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        {
            piezaC.GetComponent<Image>().color = Color.red;
        }
        if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        {
            piezaD.GetComponent<Image>().color = Color.red;
        }
    }

}
