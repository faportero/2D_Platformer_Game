using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Espejo : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, p3, p4, pm1, pm2, pm3, pm4, panelFeedback, GargolaBuena, GargolaMala, explodeObject;
    [SerializeField] UI_Piezas piezasPanel;
    [SerializeField] TextMeshPro textPanel;
    private ParticleSystem explodePartycle;
    private static int countPiezas;
    private int maxPiezas = 4;
    [HideInInspector] public static int piezasRestantes;
    public static bool isChecked, isComplete;


    private void Start()
    {
        explodePartycle = explodeObject.GetComponent<ParticleSystem>();
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
           // if (!isComplete) StartCoroutine(AnimacionGargolas());

            // StartCoroutine(AnimacionGargolas());

            CheckEspejoPieces();
            isChecked = true;
            if(countPiezas != maxPiezas)
            {
                //CheckEspejoPieces();
                piezasRestantes = maxPiezas - countPiezas;
                textPanel.text = "Vuelve al lugar donde todo empezó, te faltan: " + piezasRestantes + " fragmentos";

                print("Te faltan " + piezasRestantes);
                Invoke("ShowFeedbackPanel", 2);
            }
            else
            {
                textPanel.text = "¡Fragmentos Complatos!";
                //panelFeedback.SetActive(false);
                print("Piezas complatas");
                if(!isComplete)StartCoroutine(AnimacionGargolas());
            }
        }
    }


    private void ShowFeedbackPanel()
    {
        panelFeedback.SetActive(true);
    }


    private void CheckEspejoPieces()
    {
        if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
        {
            //PlayerControllerNew.piezaA = false;
            //p1.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaA.GetComponent<Image>().color = Color.black;      
            p1.SetActive(true);
            //piezasPanel.piezaA.SetActive(false);
            pm1.SetActive(false);
            piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
            p1.GetComponent<Piezas>().ShowPiece(.1f);
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPA = true;
        }
        if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        {
            //PlayerControllerNew.piezaB = false;
            //p2.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaB.GetComponent<Image>().color = Color.black;           
            p2.SetActive(true);
            // piezasPanel.piezaB.SetActive(false);
            pm2.SetActive(false);

            piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
            p2.GetComponent<Piezas>().ShowPiece(.2f);
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPB = true;
        }
        if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        {
            // PlayerControllerNew.piezaC = false;
            //p3.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaC.GetComponent<Image>().color = Color.black;           
            p3.SetActive(true);
            pm3.SetActive(false);

            // piezasPanel.piezaC.SetActive(false);
            piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
            p3.GetComponent<Piezas>().ShowPiece(.3f);
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPC = true;
        }
        if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        {
            // PlayerControllerNew.piezaD = false;
            //p4.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaD.GetComponent<Image>().color = Color.black;           
            p4.SetActive(true);
            //piezasPanel.piezaD.SetActive(false);
            pm4.SetActive(false);

            piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
            p4.GetComponent<Piezas>().ShowPiece(.4f);
            countPiezas++;
            countPiezas = Mathf.Clamp(countPiezas, 0, 4);
            LevelManager.usedPD = true;
        }
    } 
    private void CheckEspejoPiecesInit()
    {
        if (LevelManager.usedPA)
        {
            //p1.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaA.GetComponent<Image>().color = Color.black;
            p1.SetActive(true);
            piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
            //piezasPanel.piezaA.SetActive(false);
        }
        if (LevelManager.usedPB)
        {
            //p2.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaB.GetComponent<Image>().color = Color.black;
            p2.SetActive(true);
            piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();

            // piezasPanel.piezaB.SetActive(false);
        }
        if (LevelManager.usedPC)
        {
            //p3.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaC.GetComponent<Image>().color = Color.black;
            p3.SetActive(true);
            piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();

            //piezasPanel.piezaC.SetActive(false);
        }
        if (LevelManager.usedPD)
        {
            //p4.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaD.GetComponent<Image>().color = Color.black;
            p4.SetActive(true);
            piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();

            //piezasPanel.piezaD.SetActive(false);
        }
    }
    IEnumerator AnimacionGargolas()
    {
        explodeObject.SetActive(true);
        explodePartycle.Play();
        // Escala inicial y final para la animación
        Vector3 scaleInicialMala = GargolaMala.transform.localScale;
        Vector3 scaleFinalMala = Vector3.zero;

        Vector3 scaleInicialBuena = Vector3.zero;
        Vector3 scaleFinalBuena = GargolaBuena.transform.localScale;

        // Duración total de la animación
        float duracion = 0.5f; // Por ejemplo, medio segundo

        // Animación de GargolaMala desapareciendo
        float tiempoInicio = Time.time;
        float tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            tiempoPasado = Time.time - tiempoInicio;
            float t = Mathf.Clamp01(tiempoPasado / duracion);
            GargolaMala.transform.localScale = Vector3.Lerp(scaleInicialMala, scaleFinalMala, t);
            yield return null;
        }

        // Asegurarse de que termine en escala cero
        GargolaMala.transform.localScale = scaleFinalMala;

        // Desactivar GargolaMala después de la animación de escala
        GargolaMala.SetActive(false);

        // Breve pausa antes de la siguiente animación
        yield return new WaitForSeconds(.8f);

        // Activar GargolaBuena antes de iniciar su animación de escala
        GargolaBuena.SetActive(true);

        // Animación de GargolaBuena apareciendo
        tiempoInicio = Time.time;
        tiempoPasado = 0f;

        while (tiempoPasado < duracion)
        {
            tiempoPasado = Time.time - tiempoInicio;
            float t = Mathf.Clamp01(tiempoPasado / duracion);
            GargolaBuena.transform.localScale = Vector3.Lerp(scaleInicialBuena, scaleFinalBuena, t);
            yield return null;
        }

        // Asegurarse de que termine en su escala final
        GargolaBuena.transform.localScale = scaleFinalBuena;
        explodeObject.SetActive(false);
        isComplete = true;
    }
}
