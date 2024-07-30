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
       CheckEspejoPiecesInit();

        //print("PlayerControllerNew.piezaA: " + PlayerControllerNew.piezaA + ". LevelManager.usedPA: " + LevelManager.usedPA);
        //print("PlayerControllerNew.piezaB: " + PlayerControllerNew.piezaB + ". LevelManager.usedPA: " + LevelManager.usedPB);
        //print("PlayerControllerNew.piezaC: " + PlayerControllerNew.piezaC + ". LevelManager.usedPA: " + LevelManager.usedPC);
        //print("PlayerControllerNew.piezaD: " + PlayerControllerNew.piezaD + ". LevelManager.usedPA: " + LevelManager.usedPD);
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
            //cPiezaA.GetComponent<UI_Animation>().StartAnimation();
        }
        if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        {
            //piezaB.GetComponent<Image>().color = Color.red;
            piezaB.SetActive(true);
            //cPiezaB.GetComponent<UI_Animation>().StartAnimation();
        }
        if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        {
            //piezaC.GetComponent<Image>().color = Color.red;
            piezaC.SetActive(true);
           // cPiezaC.GetComponent<UI_Animation>().StartAnimation();
        }
        if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        {
            //piezaD.GetComponent<Image>().color = Color.red;
            piezaD.SetActive(true);
           // cPiezaD.GetComponent<UI_Animation>().StartAnimation();
        }
    }
    private void CheckEspejoPiecesInit()
    {
        if (LevelManager.usedPA)
        {
            piezaA.SetActive(true);

            piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();

        }
        if (LevelManager.usedPB)
        {
            piezaB.SetActive(true);

            piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();


        }
        if (LevelManager.usedPC)
        {
            piezaC.SetActive(true);

            piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();

         
        }
        if (LevelManager.usedPD)
        {
            piezaD.SetActive(true);

            piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();

        }
    }

}
