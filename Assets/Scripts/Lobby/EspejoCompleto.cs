using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static Gargola;

public class EspejoCompleto : MonoBehaviour
{
    [SerializeField] GameObject GargolaBuena, GargolaMala,PiezasRecuerdoBueno, PiezasRecuerdoMalo;

    private enum EspejoType
    {
        Espejo1,
        Espejo2,
        Espejo3
    }

    [SerializeField]private EspejoType espejoType;

    private void Start()
    {
        switch (espejoType)
        {
            case EspejoType.Espejo1:
                if (UserData.completoNivel1)
                {
                    GargolaMala.SetActive(false);
                    PiezasRecuerdoMalo.SetActive(false);
                    GargolaBuena.SetActive(true);
                    PiezasRecuerdoBueno.SetActive(true);
                }
                break;
            case EspejoType.Espejo2:
                if (UserData.completoNivel2)
                {
                    GargolaMala.SetActive(false);
                    PiezasRecuerdoMalo.SetActive(false);
                    GargolaBuena.SetActive(true);
                    PiezasRecuerdoBueno.SetActive(true);
                }
                break;
            case EspejoType.Espejo3:
                if (UserData.completoNivel3)
                {
                    GargolaMala.SetActive(false);
                    PiezasRecuerdoMalo.SetActive(false);
                    GargolaBuena.SetActive(true);
                    PiezasRecuerdoBueno.SetActive(true);
                }
                break;
        }
    }

}
