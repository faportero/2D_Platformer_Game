using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public GameObject rankingPresset, rankingPadre, panelMenu, panelRanking;
    public Sprite imgBest, imgGnrl;
    public Color colorTextBest, colorTextGnrl;
    public int bestPos = 3;
    bool getRanking;
    [System.Serializable]
    public struct RankingID
    {
        [System.Serializable]
        public struct Ranking
        {
            public static int posicion;
            public static string name;
            public static int cantidad;
        }
        public static Ranking[] rankings;
    }
    public static RankingID rankingID;

    //public void ObtenerRanking()
    //{
    //    ControlDatos.ObtenerRanking();
    //    StartCoroutine(EscribiendoDatos());
    //}

    public async void ObtenerRanking()
    {
        await ControlDatos.ObtenerRanking();
        await EscribiendoDatos();
    }

    private async UniTask EscribiendoDatos()
    {
        if (panelRanking == null || panelMenu == null || rankingPadre == null || rankingPresset == null)
        {
            Debug.Log("Uno o más objetos requeridos son null.");
            if (panelRanking == null) Debug.Log("panelRanking es nulo.");
            if (panelMenu == null) Debug.Log("panelMenu es nulo.");
            if (rankingPadre == null) Debug.Log("rankingPadre es nulo.");
            if (rankingPresset == null) Debug.Log("rankingPresset es nulo.");
            return;
        }

        panelRanking.SetActive(true);
        await UniTask.Delay(500);
        // panelMenu.SetActive(false);
        for (int i = 0; i < rankingPadre.transform.childCount; i++)
        {
            if (rankingPadre.transform.GetChild(i).gameObject.activeSelf) Destroy(rankingPadre.transform.GetChild(i).gameObject);
        }
        await UniTask.Delay(100);
        if (ControlDatos.respuestaRanking == null)
        {
            Debug.Log("Ranking Nullo.");
            return;
        }
        else
        {
            for (int i = 0; i < ControlDatos.respuestaRanking.response.lista.Count; i++)
            {
                if (ControlDatos.respuestaRanking.response.lista[i].objeto.cantidad != 0)
                {
                    GameObject inst = Instantiate(rankingPresset, rankingPadre.transform) as GameObject;
                    if (i < bestPos)
                    {
                        //inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().color = colorTextBest;
                        inst.transform.GetChild(1).GetComponent<Image>().sprite = imgBest;
                        inst.transform.GetChild(1).GetComponent<Image>().color = colorTextBest;
                        inst.transform.GetComponent<Image>().color = colorTextBest;
                    }
                    else
                    {
                        //inst.transform.GetChild(2).GetComponent<Text>().color = colorTextGnrl;
                        inst.transform.GetChild(1).GetComponent<Image>().sprite = imgGnrl;
                        inst.transform.GetComponent<Image>().color = colorTextGnrl;
                    }
                    inst.SetActive(true);
                    inst.transform.localScale = Vector3.one;
                    inst.transform.localEulerAngles = Vector3.zero;
                    inst.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
                    inst.transform.GetChild(2).GetComponent<Text>().text = ControlDatos.respuestaRanking.response.lista[i].usuario.nombre;
                    inst.transform.GetChild(3).GetComponent<Text>().text = ControlDatos.respuestaRanking.response.lista[i].objeto.cantidad.ToString();
                    inst.transform.GetChild(5).GetComponent<Text>().text = ControlDatos.respuestaRanking.response.lista[i].usuario.correo;
                }
                else
                {
                    return;
                }
            }
        }
    }
    public void QuitarBloqueoMedallas(GameObject padreMedallas)
    {
        if (ControlDatos._isWinnerLvl1) padreMedallas.transform.GetChild(0).gameObject.SetActive(false);
        else padreMedallas.transform.GetChild(0).gameObject.SetActive(true);
        if (ControlDatos._isWinnerLvl2) padreMedallas.transform.GetChild(1).gameObject.SetActive(false);
        else padreMedallas.transform.GetChild(1).gameObject.SetActive(true);
        if (ControlDatos._isWinnerLvl3) padreMedallas.transform.GetChild(2).gameObject.SetActive(false);
        else padreMedallas.transform.GetChild(2).gameObject.SetActive(true);
        if (ControlDatos._isWinnerLvl4) padreMedallas.transform.GetChild(3).gameObject.SetActive(false);
        else padreMedallas.transform.GetChild(3).gameObject.SetActive(true);
    }
}
