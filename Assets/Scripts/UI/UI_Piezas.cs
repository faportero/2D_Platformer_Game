using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Piezas : MonoBehaviour
{
    public GameObject piezaA, piezaB, piezaC, piezaD;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();

        //UserData.nivelActual = 1;

        //UserData.piezaA_N1 = true;
        //UserData.piezaC_N1 = true;
        //UserData.piezaD_N1 = true;
        //UserData.piezaB_N1 = true;




        ShowPanelPieces();
        print( "Nivel: " + UserData.nivelActual);

    }
    private void Update()
    {
        ShowPanelPieces();
        print("PiezaA: " + UserData.piezaA_N1 + "PiezaB: " + UserData.piezaC_N1+ "PiezaC: " + UserData.piezaD_N1+"PiezaD: " + UserData.piezaB_N1);
    }
    private void ShowPanelPieces()
    {
        // Determina el nivel actual usando el enum
        switch (levelManager.currentScene)
        {
            case LevelManager.CurrentScene.Nivel1:
                UserData.nivelActual = 1;
                UpdatePiezaState(UserData.piezaA_N1, UserData.usedPiezaA_N1, piezaA);
                UpdatePiezaState(UserData.piezaB_N1, UserData.usedPiezaB_N1, piezaB);
                UpdatePiezaState(UserData.piezaC_N1, UserData.usedPiezaC_N1, piezaC);
                UpdatePiezaState(UserData.piezaD_N1, UserData.usedPiezaD_N1, piezaD);
                break;

            case LevelManager.CurrentScene.Nivel2:
                UserData.nivelActual = 2;
                UpdatePiezaState(UserData.piezaA_N2, UserData.usedPiezaA_N2, piezaA);
                UpdatePiezaState(UserData.piezaB_N2, UserData.usedPiezaB_N2, piezaB);
                UpdatePiezaState(UserData.piezaC_N2, UserData.usedPiezaC_N2, piezaC);
                UpdatePiezaState(UserData.piezaD_N2, UserData.usedPiezaD_N2, piezaD);
                break;

            case LevelManager.CurrentScene.Nivel3:
                UserData.nivelActual = 3;
                UpdatePiezaState(UserData.piezaA_N3, UserData.usedPiezaA_N3, piezaA);
                UpdatePiezaState(UserData.piezaB_N3, UserData.usedPiezaB_N3, piezaB);
                UpdatePiezaState(UserData.piezaC_N3, UserData.usedPiezaC_N3, piezaC);
                UpdatePiezaState(UserData.piezaD_N3, UserData.usedPiezaD_N3, piezaD);
                break;   
            case LevelManager.CurrentScene.Limbo:
                if (UserData.nivelActual == 1)
                {
                    UpdatePiezaState(UserData.piezaA_N1, UserData.usedPiezaA_N1, piezaA);
                    UpdatePiezaState(UserData.piezaB_N1, UserData.usedPiezaB_N1, piezaB);
                    UpdatePiezaState(UserData.piezaC_N1, UserData.usedPiezaC_N1, piezaC);
                    UpdatePiezaState(UserData.piezaD_N1, UserData.usedPiezaD_N1, piezaD);
                }
                if (UserData.nivelActual == 2)
                {
                    UpdatePiezaState(UserData.piezaA_N2, UserData.usedPiezaA_N2, piezaA);
                    UpdatePiezaState(UserData.piezaB_N2, UserData.usedPiezaB_N2, piezaB);
                    UpdatePiezaState(UserData.piezaC_N2, UserData.usedPiezaC_N2, piezaC);
                    UpdatePiezaState(UserData.piezaD_N2, UserData.usedPiezaD_N2, piezaD);
                }
                if (UserData.nivelActual == 3)
                {
                    UpdatePiezaState(UserData.piezaA_N3, UserData.usedPiezaA_N3, piezaA);
                    UpdatePiezaState(UserData.piezaB_N3, UserData.usedPiezaB_N3, piezaB);
                    UpdatePiezaState(UserData.piezaC_N3, UserData.usedPiezaC_N3, piezaC);
                    UpdatePiezaState(UserData.piezaD_N3, UserData.usedPiezaD_N3, piezaD);
                }

                break;
        }
    }

    private void UpdatePiezaState(bool collected, bool used, GameObject pieza)
    {

        if (collected)
        {
            print("Nombre: " + pieza.name + "Nivel: " + UserData.nivelActual);
            pieza.SetActive(true);
            if (used)
            {
              //  print("Nombre: " + pieza.name);
                pieza.GetComponent<SwitchSprite>().SwitchNewSprite(); // Cambia a blanco y negro
            }
        }
        else
        {
            pieza.SetActive(false);
        }
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class UI_Piezas : MonoBehaviour
//{
//    public GameObject piezaA, piezaB, piezaC, piezaD;
//    private PlayerControllerNew playerControllerNew;
//    private LevelManager levelManager;

//    private void Start()
//    {
//       playerControllerNew = FindAnyObjectByType<PlayerControllerNew>();
//       levelManager = FindAnyObjectByType<LevelManager>();
//    }
//    private void Update()
//    {
//      // CheckPieces();
//       //CheckEspejoPiecesInit();

//        //print("PlayerControllerNew.piezaA: " + PlayerControllerNew.piezaA + ". LevelManager.usedPA: " + LevelManager.usedPA);
//        //print("PlayerControllerNew.piezaB: " + PlayerControllerNew.piezaB + ". LevelManager.usedPA: " + LevelManager.usedPB);
//        //print("PlayerControllerNew.piezaC: " + PlayerControllerNew.piezaC + ". LevelManager.usedPA: " + LevelManager.usedPC);
//        //print("PlayerControllerNew.piezaD: " + PlayerControllerNew.piezaD + ". LevelManager.usedPA: " + LevelManager.usedPD);
//    }
//    private void CheckPieces()
//    {
//        Invoke("ShowPanelPieces", .75f);
//    }

//        private void ShowPanelPieces()
//    {

//        //switch (levelManager.currentScene)
//        //{
//        //    case LevelManager.CurrentScene.Nivel1:
//        //        if(UserData.piezaA_N1 && !UserData.usedPiezaA_N1)
//        //        {
//        //            piezaA.SetActive(true);
//        //        }
//        //        if (UserData.piezaB_N1 && !UserData.usedPiezaB_N1)
//        //        {
//        //            piezaB.SetActive(true);
//        //        }
//        //        if (UserData.piezaC_N1 && !UserData.usedPiezaC_N1)
//        //        {
//        //            piezaC.SetActive(true);
//        //        }
//        //        if (UserData.piezaD_N1 && !UserData.usedPiezaD_N1)
//        //        {
//        //            piezaD.SetActive(true);
//        //        }
//        //        break;
//        //    case LevelManager.CurrentScene.Nivel2:
//        //        if (UserData.piezaA_N2 && !UserData.usedPiezaA_N2)
//        //        {
//        //            piezaA.SetActive(true);
//        //        }
//        //        if (UserData.piezaB_N2 && !UserData.usedPiezaB_N2)
//        //        {
//        //            piezaB.SetActive(true);
//        //        }
//        //        if (UserData.piezaC_N2 && !UserData.usedPiezaC_N2)
//        //        {
//        //            piezaC.SetActive(true);
//        //        }
//        //        if (UserData.piezaD_N2 && !UserData.usedPiezaD_N2)
//        //        {
//        //            piezaD.SetActive(true);
//        //        }
//        //        break;
//        //    case LevelManager.CurrentScene.Nivel3:
//        //        if (UserData.piezaA_N3 && !UserData.usedPiezaA_N3)
//        //        {
//        //            piezaA.SetActive(true);
//        //        }
//        //        if (UserData.piezaB_N3 && !UserData.usedPiezaB_N3)
//        //        {
//        //            piezaB.SetActive(true);
//        //        }
//        //        if (UserData.piezaC_N3 && !UserData.usedPiezaC_N3)
//        //        {
//        //            piezaC.SetActive(true);
//        //        }
//        //        if (UserData.piezaD_N3 && !UserData.usedPiezaD_N3)
//        //        {
//        //            piezaD.SetActive(true);
//        //        }
//        //        break;
//        //}


//        //if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
//        //{
//        //    // piezaA.GetComponent<Image>().color = Color.red;
//        //    //playerControllerNew.TakePiece();
//        //    piezaA.SetActive(true);
//        //    //cPiezaA.GetComponent<UI_Animation>().StartAnimation();
//        //}
//        //if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
//        //{
//        //    //piezaB.GetComponent<Image>().color = Color.red;
//        //    piezaB.SetActive(true);
//        //    //cPiezaB.GetComponent<UI_Animation>().StartAnimation();
//        //}
//        //if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
//        //{
//        //    //piezaC.GetComponent<Image>().color = Color.red;
//        //    piezaC.SetActive(true);
//        //   // cPiezaC.GetComponent<UI_Animation>().StartAnimation();
//        //}
//        //if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
//        //{
//        //    //piezaD.GetComponent<Image>().color = Color.red;
//        //    piezaD.SetActive(true);
//        //   // cPiezaD.GetComponent<UI_Animation>().StartAnimation();
//        //}
//    }
//    private void CheckEspejoPiecesInit()
//    {


//        //if (levelManager.currentScene == LevelManager.CurrentScene.Nivel1 || levelManager.currentScene == LevelManager.CurrentScene.Limbo)
//        //{
//        //    if (UserData.usedPiezaA_N1)
//        //    {
//        //        piezaA.SetActive(true);
//        //        piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
//        //    }
//        //    if (UserData.usedPiezaB_N1)
//        //    {
//        //        piezaB.SetActive(true);
//        //        piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
//        //    }
//        //    if (UserData.usedPiezaC_N1)
//        //    {
//        //        piezaC.SetActive(true);
//        //        piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
//        //    }
//        //    if (UserData.usedPiezaD_N1)
//        //    {
//        //        piezaD.SetActive(true);
//        //        piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
//        //    }

//        //}

//    //    else if (levelManager.currentScene == LevelManager.CurrentScene.Nivel2 || levelManager.currentScene == LevelManager.CurrentScene.Limbo)
//    //    {
//    //        if (UserData.usedPiezaA_N2)

//    //        {
//    //            piezaA.SetActive(true);
//    //            piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaB_N2)
//    //        {
//    //            piezaB.SetActive(true);
//    //            piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaC_N2)
//    //        {
//    //            piezaC.SetActive(true);
//    //            piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaD_N2)
//    //        {
//    //            piezaD.SetActive(true);
//    //            piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }

//    //    }

//    //    else if (levelManager.currentScene == LevelManager.CurrentScene.Nivel3 || levelManager.currentScene == LevelManager.CurrentScene.Limbo)
//    //    {

//    //        if (UserData.usedPiezaA_N3)
//    //        {
//    //            piezaA.SetActive(true);
//    //            piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaB_N3)
//    //        {
//    //            piezaB.SetActive(true);
//    //            piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaC_N3)
//    //        {
//    //            piezaC.SetActive(true);
//    //            piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }
//    //        if (UserData.usedPiezaD_N3)
//    //        {
//    //            piezaD.SetActive(true);
//    //            piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
//    //        }

//    //    }
//    //}
//        //if (LevelManager.usedPA)
//        //{
//        //    piezaA.SetActive(true);

//        //    piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();

//        //}
//        ////else piezaA.SetActive(false);

//        //if (LevelManager.usedPB)
//        //{
//        //    piezaB.SetActive(true);

//        //    piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();


//        //}

//        ////else piezaB.SetActive(false);

//        //if (LevelManager.usedPC)
//        //{
//        //    piezaC.SetActive(true);

//        //    piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();


//        //}
//        ////else piezaC.SetActive(false);

//        //if (LevelManager.usedPD)
//        //{
//        //    piezaD.SetActive(true);

//        //    piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();

//        }
//        ////else piezaD.SetActive(false);
//    }


