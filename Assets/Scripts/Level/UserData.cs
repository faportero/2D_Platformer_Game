using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static int lifes = 3, health;
    public static float coins;
    public static bool escudo, saltoDoble, vidaExtra, paracaidas;


    public static bool terminoLobby, terminoLimbo, terminoNivel1, terminoTutorial, terminoTutorial2, terminoTutorial3, terminoWorldTutorial1, terminoWorldTutorial2, terminoWorldTutorial3;
    public static bool completoNivel1, completoNivel2, completoNivel3;
    public static bool terminoVideoInicio, terminoVideoVortex1, terminoVideoVortex2, terminoVideoVortex3;

    public static bool piezaA_N1, piezaA_N2, piezaA_N3;
    public static bool piezaB_N1, piezaB_N2, piezaB_N3;
    public static bool piezaC_N1, piezaC_N2, piezaC_N3;
    public static bool piezaD_N1, piezaD_N2, piezaD_N3;
    public static int nivelActual; 
    public static bool usedPiezaA_N1, usedPiezaA_N2, usedPiezaA_N3;
    public static bool usedPiezaB_N1, usedPiezaB_N2, usedPiezaB_N3;
    public static bool usedPiezaC_N1, usedPiezaC_N2, usedPiezaC_N3;
    public static bool usedPiezaD_N1, usedPiezaD_N2, usedPiezaD_N3;

    public static bool playerGuide1, playerGuide2, playerGuide3, playerGuide4, playerGuide5, playerGuide6;

    // Metodo para reiniciar piezas al morir
    public static void ResetPieces(int nivel)
    {
        switch (nivel)
        {
            case 1:
                if (!usedPiezaA_N1) piezaA_N1 = false;
                if (!usedPiezaB_N1) piezaB_N1 = false;
                if (!usedPiezaC_N1) piezaC_N1 = false;
                if (!usedPiezaD_N1) piezaD_N1 = false;
                break;
            case 2:
                if (!usedPiezaA_N2) piezaA_N2 = false;
                if (!usedPiezaB_N2) piezaB_N2 = false;
                if (!usedPiezaC_N2) piezaC_N2 = false;
                if (!usedPiezaD_N2) piezaD_N2 = false;
                break;
            case 3:
                if (!usedPiezaA_N3) piezaA_N3 = false;
                if (!usedPiezaB_N3) piezaB_N3 = false;
                if (!usedPiezaC_N3) piezaC_N3 = false;
                if (!usedPiezaD_N3) piezaD_N3 = false;
                break;
        }
    }
 
}
