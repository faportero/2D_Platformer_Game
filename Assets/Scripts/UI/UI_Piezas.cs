using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Piezas : MonoBehaviour
{
    public GameObject piezaA, piezaB, piezaC, piezaD;
    private PlayerControllerNew playerControllerNew;

    private void Start()
    {
       playerControllerNew = FindAnyObjectByType<PlayerControllerNew>();
    }
    private void Update()
    {
        CheckPieces();        
    }
    private void CheckPieces()
    {
        Invoke("ShowPanelPieces", .75f);
    }

    private void ShowPanelPieces()
    {
        if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
        {
            // piezaA.GetComponent<Image>().color = Color.red;
            //playerControllerNew.TakePiece();
            piezaA.SetActive(true);
        }
        if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        {
            //piezaB.GetComponent<Image>().color = Color.red;
            piezaB.SetActive(true);
        }
        if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        {
            //piezaC.GetComponent<Image>().color = Color.red;
            piezaC.SetActive(true);
        }
        if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        {
            //piezaD.GetComponent<Image>().color = Color.red;
            piezaD.SetActive(true);
        }
    }

}
