using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Espejo : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, p3, p4, pm1, pm2, pm3, pm4, panelFeedback, GargolaBuena, GargolaMala, explodeObject, PiezasRecuerdoMalo, PiezasRecuerdoBueno;

    public GameObject triggerParteFinal, panelHUD;
    [SerializeField] UI_Piezas piezasPanel;
    [SerializeField] TextMeshPro textPanel;
    public GameObject videoPlayerPlane;
    
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    public CinemachineVirtualCamera cameraPlayer, cameraVideo, cameraGeneral;

    private ParticleSystem explodePartycle;
    private static int countPiezas = 0;
    public int maxPiezas = 0, countVideoClips;
    [HideInInspector] public static int piezasRestantes;
    public static bool isChecked, isComplete;
    public GameObject panelDialogueGargolas;
    private PlayerControllerNew playerController;
    [HideInInspector]public PlayerMovementNew playerMovement;
    private bool isFacingRight = true;
    public bool terminoVideo;
    private bool isPlaying;
    private Vector3 targetPosition;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerControllerNew>();
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
       // dialogueGargolas = FindAnyObjectByType<DialogueGargolas>();
    }
    private void Start()
    {
        explodePartycle = explodeObject.GetComponent<ParticleSystem>();
        videoPlayer.loopPointReached += OnVideoEnd;

        // CheckEspejoPieces();
        CheckEspejoPiecesInit();
        if(countPiezas == maxPiezas) GetComponent<BoxCollider2D>().enabled = false;

    }
    private void Update()
    {
        if (isPlaying && !explodePartycle.isPlaying)
        {
            explodeObject.SetActive(false);
            isPlaying = false;
        }
    }
    public void OnVideoEnd(VideoPlayer vp)
    {
        // Lógica para cambiar al juego
        terminoVideo = true;
      //  countVideoClips++;
        //CameraManager.instance.SingleSwapCamera(cameraGeneral, 1f);
        playerMovement.inputsEnabled = true;


        //if (countVideoClips == 1) StartCoroutine(ActivateEnte());
        //if (countVideoClips > 1) 
        //{
        //   // panelDialogueGargolas.SetActive(true);
        //    panelDialogueGargolas.GetComponent<DialogueGargolas>().OnButtonDown();

        //}

    }

    public IEnumerator ShowVideoPanel()
    {
        //Desactiva Input
        isFacingRight = false;
        //playerMovement.transform.localScale = new Vector3(-playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);
        //playerMovement.inputsEnabled = false;
        yield return new WaitForSeconds(.5f);
        panelHUD.SetActive(false);//Desactiva HUD



        //Camina fuera del espejo
        //isFacingRight = false;
        //Vector3 targetPosition = playerMovement.transform.position + new Vector3(playerMovement.transform.position.x + 2000, playerMovement.transform.position.y, playerMovement.transform.position.z);

        //playerMovement.transform.position = Vector3.MoveTowards(playerMovement.transform.position, targetPosition, 2f * Time.deltaTime);
        //playerMovement.anim.SetBool("SlowWalk", true);
        //yield return new WaitWhile(() => playerMovement.transform.position.x == targetPosition.x);
        //targetPosition = Vector3.zero;
        //playerMovement.inputsEnabled = false;


        //Animacion piezas
        PiezasRecuerdoMalo.GetComponent<Animator>().enabled = true;
        AnimationClip animacion = PiezasRecuerdoMalo.GetComponent<Animator>().runtimeAnimatorController.animationClips[0];
        yield return new WaitForSecondsRealtime(animacion.averageDuration);
        // yield return new WaitForSeconds(1);

        //Mira hacia video
        //playerMovement.transform.localScale = new Vector3(-playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);
        SwitchPlayerTransform(false);

        //Cambia a camara de video
        CameraManager.instance.SingleSwapCamera(cameraVideo, 1f);
        yield return new WaitForSeconds(2f);

        //Reproduce video
        PiezasRecuerdoMalo.SetActive(false);//desactiva imagen de recuerdo bueno
        //videoPlayerPlane.SetActive(true);
        //videoPlayer.clip = videoClips[countVideoClips];
        //videoPlayer.Play();

    }
    private IEnumerator ActivateEnte()
    {
        SwitchPlayerTransform(true);
        yield return new WaitForSeconds(1);
        transform.GetChild(0).gameObject.SetActive(true);
        panelHUD.SetActive(false);//Desactiva HUD
        panelDialogueGargolas.GetComponent<Animator>().enabled = true;
        //playerMovement.transform.localScale = new Vector3(playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);
    }
    public void SwitchPlayerTransform(bool isFacingRight)
    {
        //if (isFacingRight)
        //{
        //    playerMovement.transform.localScale = new Vector3(playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);
        //   // playerMovement.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        //   // isFacingRight = !isFacingRight;   
        //   // isFacingRight = false;   
        //}
        //else
        //{
        //    playerMovement.transform.localScale = new Vector3(-playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);
        //    //playerMovement.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        //    //  isFacingRight = !isFacingRight;
        //    //  isFacingRight = true;
        //}
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

                //print("Te faltan " + piezasRestantes);
                Invoke("ShowFeedbackPanel", 2);
            }
            else
            {
                textPanel.text = "¡Fragmentos Complatos!";
                //panelFeedback.SetActive(false);
                // print("Piezas complatas");
                //   if(targetPosition == Vector3.zero) targetPosition = playerMovement.transform.position + new Vector3(playerMovement.transform.position.x + 2000, playerMovement.transform.position.y, playerMovement.transform.position.z);
                //  StartCoroutine(ShowVideoPanel());
                //if(!isComplete)StartCoroutine(AnimacionGargolas());
                //triggerParteFinal.SetActive(true);
                StartCoroutine(ActivateEnte());
                GetComponent<BoxCollider2D>().enabled = false;
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
            pm1.SetActive(false);
            //piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
            //piezasPanel.piezaA.SetActive(false);
        }
        if (LevelManager.usedPB)
        {
            //p2.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaB.GetComponent<Image>().color = Color.black;
            p2.SetActive(true);
            pm2.SetActive(false);
            //piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();

            // piezasPanel.piezaB.SetActive(false);
        }
        if (LevelManager.usedPC)
        {
            //p3.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaC.GetComponent<Image>().color = Color.black;
            p3.SetActive(true);
            pm3.SetActive(false);
            //piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();

            //piezasPanel.piezaC.SetActive(false);
        }
        if (LevelManager.usedPD)
        {
            //p4.GetComponent<SpriteRenderer>().color = Color.red;
            //piezasPanel.piezaD.GetComponent<Image>().color = Color.black;
            p4.SetActive(true);
            pm4.SetActive(false);
            //piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();

            //piezasPanel.piezaD.SetActive(false);
        }
    }
    public void CreateExplosion()
    {
        // Verifica si la partícula ya está en reproducción para evitar iniciarla múltiples veces
        if (!isPlaying)
        {
            AudioManager.Instance.PlaySfx("Espejo_explocion");

            explodeObject.SetActive(true);
            explodePartycle.Play();
            isPlaying = true;
        }
    }

    public IEnumerator AnimacionGargolas()
    {


        CreateExplosion();
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

        //Intercambiar Recuerdos de espejo
        videoPlayerPlane.SetActive(false);
        PiezasRecuerdoBueno.SetActive(true);

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
       // playerMovement.inputsEnabled = true;
        isComplete = true;
       // playerMovement.transform.localScale = new Vector3(playerMovement.transform.localScale.x, playerMovement.transform.localScale.y, playerMovement.transform.localScale.z);

        CameraManager.instance.SingleSwapCamera(cameraPlayer, 1f);
        SwitchPlayerTransform(true);
        triggerParteFinal.SetActive(false);
       // panelHUD.SetActive(true);
        //videoPlayerPlane.SetActive(true);
        //videoPlayer.clip = videoClips[countVideoClips];
        //videoPlayer.Play();

    }
}
