using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LimboManager : MonoBehaviour
{
    public static int countVideosWatched;
    [SerializeField] private Espejo[] espejos;
    [SerializeField] private bool completoN1, completoN2, completoN3;
    private UI_Piezas ui_piezas;
    private void Awake()
    {
        //gargolas = FindObjectsOfType<Gargola>();

        ui_piezas = FindAnyObjectByType<UI_Piezas>();
        //UserData.completoNivel1 = completoN1;
        //UserData.completoNivel2 = completoN2;
        //UserData.completoNivel3 = completoN3;

    }
    private void Start()
    {


        if (UserData.completoNivel1 && UserData.terminoVideoVortex1)
        {
            espejos[0].enabled = false;
            espejos[0].GetComponent<BoxCollider2D>().enabled = false;
            espejos[0].panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            espejos[1].enabled = true;
            espejos[1].GetComponent<BoxCollider2D>().enabled = true;
            espejos[1].panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            ui_piezas.piezaA.SetActive(true);
            ui_piezas.piezaB.SetActive(true);
            ui_piezas.piezaC.SetActive(true);
            ui_piezas.piezaD.SetActive(true);

        }

        if (UserData.completoNivel2 && UserData.terminoVideoVortex2)
        {
            espejos[1].enabled = false;
            espejos[1].GetComponent<BoxCollider2D>().enabled = false;
            espejos[1].panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            espejos[2].enabled = true;
            espejos[2].GetComponent<BoxCollider2D>().enabled = true;
            espejos[2].panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            ui_piezas.piezaA.SetActive(true);
            ui_piezas.piezaB.SetActive(true);
            ui_piezas.piezaC.SetActive(true);
            ui_piezas.piezaD.SetActive(true);

        }
        if (UserData.completoNivel3 && UserData.terminoVideoVortex3)
        {
            espejos[2].enabled = false;
            espejos[2].GetComponent<BoxCollider2D>().enabled = false;
            espejos[2].panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            ui_piezas.piezaA.SetActive(true);
            ui_piezas.piezaB.SetActive(true);
            ui_piezas.piezaC.SetActive(true);
            ui_piezas.piezaD.SetActive(true);

        }
        //if (UserData.terminoLimbo)
        //{
        //    foreach (var g in gargolas)
        //    {
        //        g.videoPlayerPlane.SetActive(true);
        //        g.videoPlayerPlane.GetComponent<Animator>().enabled = true;
        //        ulong lastFrame = g.videoPlayer.frameCount - 1;
        //        g.videoPlayer.frame = (long)lastFrame;
        //        g.videoPlayer.Play();
        //        g.videoPlayer.Pause();
        //    }
        //}
    }

}
