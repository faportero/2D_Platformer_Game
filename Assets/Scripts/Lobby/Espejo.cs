using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static Gargola;

public class Espejo : MonoBehaviour
{
    [SerializeField] GameObject p1, p2, p3, p4, pm1, pm2, pm3, pm4, GargolaBuena, GargolaMala, explodeObject, PiezasRecuerdoMalo, PiezasRecuerdoBueno, turnCollider, playerGuide4, playerGuide5;
    public GameObject panelFeedback;
    public GameObject triggerParteFinal, panelHUD;
    [SerializeField] UI_Piezas piezasPanel;
    [SerializeField] TextMeshPro textPanel;
    public GameObject videoPlayerPlane;
    
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    public CinemachineVirtualCamera cameraPlayer, cameraVideo, cameraGeneral;

    private ParticleSystem explodePartycle;
    public static int countPiezas = 0;
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
    [SerializeField] GameObject reintentarBtn;
    public enum EspejoType
    {
        Espejo1,
        Espejo2,
        Espejo3
    }

    public EspejoType espejoType;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerControllerNew>();
        playerMovement = FindAnyObjectByType<PlayerMovementNew>();
        // dialogueGargolas = FindAnyObjectByType<DialogueGargolas>();
        AssignEspejoType();
    }
    private void Start()
    {


        explodePartycle = explodeObject.GetComponent<ParticleSystem>();
        videoPlayer.loopPointReached += OnVideoEnd;

        // CheckEspejoPieces();
        if(countPiezas != maxPiezas) CheckEspejoPiecesInit();
        //if(countPiezas == maxPiezas) GetComponent<BoxCollider2D>().enabled = false;


        //if (espejoType == EspejoType.Espejo1 && UserData.completoNivel1)
        //{
        //    GargolaMala.SetActive(false);
        //    GargolaBuena.SetActive(true);
        //    PiezasRecuerdoBueno.SetActive(true);
        //}
        //if (espejoType == EspejoType.Espejo2 && UserData.completoNivel2)
        //{
        //    GargolaMala.SetActive(false);
        //    GargolaBuena.SetActive(true);
        //    PiezasRecuerdoBueno.SetActive(true);
        //}
        //if (espejoType == EspejoType.Espejo3 && UserData.completoNivel3)
        //{
        //    GargolaMala.SetActive(false);
        //    GargolaBuena.SetActive(true);
        //    PiezasRecuerdoBueno.SetActive(true);
        //}


    }
    private void Update()
    {
        if (isPlaying && !explodePartycle.isPlaying)
        {
            explodeObject.SetActive(false);
            isPlaying = false;
        }

        //if (!UserData.playerGuide4)
        //{
        //    playerGuide4.SetActive(true);
        //    UserData.playerGuide4 = true;
        //}
        //else
        //{
        //    playerGuide4.SetActive(false);
        //}

        //if (!UserData.playerGuide5)
        //{
        //    playerGuide5.SetActive(true);
        //    UserData.playerGuide5 = true;
        //}
        //else
        //{
        //    playerGuide5.SetActive(false);
        //}
    }

    public void AssignEspejoType()
    {
        //switch (espejoType)
        //{
        //    case EspejoType.Espejo1:
        //        reintentarBtn.GetComponent<Button>().interactable = true;
        //        if (UserData.completoNivel1)
        //        {
        //            GetComponent<BoxCollider2D>().enabled = false;
        //            panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = true;

        //        }

        //        break;
        //    case EspejoType.Espejo2:
        //        if (UserData.completoNivel2)
        //        {
        //            GetComponent<BoxCollider2D>().enabled = false;
        //            panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;

        //        }
        //        if (UserData.completoNivel1)
        //        {
        //            GetComponent<BoxCollider2D>().enabled = true;
        //            panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        //            reintentarBtn.GetComponent<Button>().interactable = true;
        //        }

        //        break;
        //    case EspejoType.Espejo3:
        //        if (UserData.completoNivel2)
        //        {
        //            if (UserData.completoNivel3)
        //            {
        //                GetComponent<BoxCollider2D>().enabled = false;
        //                panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = false;

        //            }
        //            GetComponent<BoxCollider2D>().enabled = true;
        //            panelFeedback.transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        //            reintentarBtn.GetComponent<Button>().interactable = true;
        //        }

        //        break;
        //}
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

        yield return new WaitForSeconds(.5f);
        panelHUD.SetActive(false);//Desactiva HUD


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

    }
    private IEnumerator MoveCharacterRight()
    {
        // Configura la dirección de movimiento a la derecha
        playerMovement.clicDirection = 1;


        Coroutine coroutine = null;
        // Inicia la corutina para mover el personaje una distancia fija
        coroutine = StartCoroutine(playerMovement.MoveFixedDistance());

        // Espera a que la corutina de movimiento termine antes de continuar
        yield return coroutine;
    }


    private IEnumerator ActivateEnte()
    {
        CameraManager.instance.SingleSwapCamera(cameraGeneral, 1f);
        playerMovement.inputsEnabled = false;
        yield return StartCoroutine(MoveCharacterRight()); // Mueve el personaje primero

        yield return new WaitForSeconds(1); // Espera 1 segundo

        transform.GetChild(0).gameObject.SetActive(true); // Activa el hijo
        panelHUD.SetActive(false); // Desactiva HUD
        panelDialogueGargolas.GetComponent<Animator>().enabled = true; // Activa el animador del panel de diálogo
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
            //if (espejoType == EspejoType.Espejo1 && UserData.completoNivel1)
            //{
            //   // GetComponent<Collider2D>().enabled = false;

            //}
            //if (espejoType == EspejoType.Espejo2 && UserData.completoNivel2)
            //{
            //   // GetComponent<Collider2D>().enabled = false;

            //}
            //if (espejoType == EspejoType.Espejo3 && UserData.completoNivel3)
            //{
            //   // GetComponent<Collider2D>().enabled = false;

            //}
            //else 
            //{
            //    //if (espejoType == EspejoType.Espejo1 && PlayerControllerNew.previousLevel == 1) CheckEspejoPieces();
            //    //else if (espejoType == EspejoType.Espejo2 && PlayerControllerNew.previousLevel == 2) CheckEspejoPieces();
            //    //else if (espejoType == EspejoType.Espejo3 && PlayerControllerNew.previousLevel == 3) CheckEspejoPieces();
            //    CheckEspejoPieces();
            //    //isChecked = true;
            //    if (countPiezas != maxPiezas)
            //    {
            //        piezasRestantes = maxPiezas - countPiezas;
            //        if(countPiezas <= 1) textPanel.text = "Vuelve al lugar donde todo empezó, te falta: " + piezasRestantes + " fragmento";
            //        else if(countPiezas > 1) textPanel.text = "Vuelve al lugar donde todo empezó, te faltan: " + piezasRestantes + " fragmentos";
            //    }
            //    else
            //    {
            //        panelFeedback.SetActive(false);
            //        textPanel.text = "¡Fragmentos Complatos!";

            //        StartCoroutine(ActivateEnte());
            //        GetComponent<BoxCollider2D>().enabled = false;
            //    }
            //}
            CheckEspejoPieces();
            //isChecked = true;
            if (countPiezas != maxPiezas)
            {
                piezasRestantes = maxPiezas - countPiezas;
                if (countPiezas == 3) textPanel.text = "Vuelve al lugar donde todo empezó, te falta: " + piezasRestantes + " fragmento";
                else if (countPiezas <3) textPanel.text = "Vuelve al lugar donde todo empezó, te faltan: " + piezasRestantes + " fragmentos";
            }
            else
            {
                panelFeedback.SetActive(false);
                textPanel.text = "¡Fragmentos Complatos!";

                StartCoroutine(ActivateEnte());
                GetComponent<BoxCollider2D>().enabled = false;
                turnCollider.GetComponent<BoxCollider2D>().enabled = false;
                
            }

        }
    }

    private void ShowFeedbackPanel()
    {
        panelFeedback.SetActive(true);
    }


    private void CheckEspejoPieces()
    {
        switch (espejoType)
        {
            case EspejoType.Espejo1:
                if (UserData.piezaA_N1 && !UserData.usedPiezaA_N1)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                    piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p1.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaA_N1 = true;
                }
                if (UserData.piezaB_N1 && !UserData.usedPiezaB_N1)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                    piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p2.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaB_N1 = true;
                }
                if (UserData.piezaC_N1 && !UserData.usedPiezaC_N1)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                    piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p3.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaC_N1 = true;
                }
                if (UserData.piezaD_N1 && !UserData.usedPiezaD_N1)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                    piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p4.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaD_N1 = true;
                }
                break;

            case EspejoType.Espejo2:
                if (UserData.piezaA_N2 && !UserData.usedPiezaA_N2)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                    piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p1.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaA_N2 = true;
                }
                if (UserData.piezaB_N2 && !UserData.usedPiezaB_N2)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                    piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p2.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaB_N2 = true;
                }
                if (UserData.piezaC_N2 && !UserData.usedPiezaC_N2)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                    piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p3.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaC_N2 = true;
                }
                if (UserData.piezaD_N2 && !UserData.usedPiezaD_N2)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                    piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p4.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaD_N2 = true;
                }
                break;

            case EspejoType.Espejo3:
                if (UserData.piezaA_N3 && !UserData.usedPiezaA_N3)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                    piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p1.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaA_N3 = true;
                }
                if (UserData.piezaB_N3 && !UserData.usedPiezaB_N3)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                    piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p2.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaB_N3 = true;
                }
                if (UserData.piezaC_N3 && !UserData.usedPiezaC_N3)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                    piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p3.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaC_N3 = true;
                }
                if (UserData.piezaD_N3 && !UserData.usedPiezaD_N3)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                    piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
                    p4.GetComponent<Piezas>().ShowPiece(.1f);
                    countPiezas++;
                    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
                    UserData.usedPiezaD_N3 = true;
                }
                break;
        }



        //if (PlayerControllerNew.piezaA && !LevelManager.usedPA)
        //{
        //    p1.SetActive(true);
        //    pm1.SetActive(false);
        //    piezasPanel.piezaA.GetComponent<SwitchSprite>().SwitchNewSprite();
        //    p1.GetComponent<Piezas>().ShowPiece(.1f);
        //    countPiezas++;
        //    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
        //    LevelManager.usedPA = true;
        //}
        //if (PlayerControllerNew.piezaB && !LevelManager.usedPB)
        //{
        //    p2.SetActive(true);
        //    pm2.SetActive(false);
        //    piezasPanel.piezaB.GetComponent<SwitchSprite>().SwitchNewSprite();
        //    p2.GetComponent<Piezas>().ShowPiece(.2f);
        //    countPiezas++;
        //    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
        //    LevelManager.usedPB = true;
        //}
        //if (PlayerControllerNew.piezaC && !LevelManager.usedPC)
        //{
        //    p3.SetActive(true);
        //    pm3.SetActive(false);
        //    piezasPanel.piezaC.SetActive(false);
        //    piezasPanel.piezaC.GetComponent<SwitchSprite>().SwitchNewSprite();
        //    p3.GetComponent<Piezas>().ShowPiece(.3f);
        //    countPiezas++;
        //    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
        //    LevelManager.usedPC = true;
        //}
        //if (PlayerControllerNew.piezaD && !LevelManager.usedPD)
        //{
        //    p4.SetActive(true);
        //    pm4.SetActive(false);
        //    piezasPanel.piezaD.GetComponent<SwitchSprite>().SwitchNewSprite();
        //    p4.GetComponent<Piezas>().ShowPiece(.4f);
        //    countPiezas++;
        //    countPiezas = Mathf.Clamp(countPiezas, 0, 4);
        //    LevelManager.usedPD = true;
        //}
    } 
    private void CheckEspejoPiecesInit()
    {
        switch (espejoType)
        {
            case EspejoType.Espejo1:
                if (UserData.usedPiezaA_N1)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                }
                if (UserData.usedPiezaB_N1)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                }
                if (UserData.usedPiezaC_N1)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                }
                if (UserData.usedPiezaD_N1)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                }
                break;

            case EspejoType.Espejo2:
                if (UserData.usedPiezaA_N2)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                }
                if (UserData.usedPiezaB_N2)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                }
                if (UserData.usedPiezaC_N2)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                }
                if (UserData.usedPiezaD_N2)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                }
                break;

            case EspejoType.Espejo3:
                if (UserData.usedPiezaA_N3)
                {
                    p1.SetActive(true);
                    pm1.SetActive(false);
                }
                if (UserData.usedPiezaB_N3)
                {
                    p2.SetActive(true);
                    pm2.SetActive(false);
                }
                if (UserData.usedPiezaC_N3)
                {
                    p3.SetActive(true);
                    pm3.SetActive(false);
                }
                if (UserData.usedPiezaD_N3)
                {
                    p4.SetActive(true);
                    pm4.SetActive(false);
                }
                break;
        }


        //if (LevelManager.usedPA)
        //{
           
        //    p1.SetActive(true);
        //    pm1.SetActive(false);
           
        //}
        //if (LevelManager.usedPB)
        //{
        //    p2.SetActive(true);
        //    pm2.SetActive(false);
        //}
        //if (LevelManager.usedPC)
        //{
        //    p3.SetActive(true);
        //    pm3.SetActive(false);
        //}
        //if (LevelManager.usedPD)
        //{
        //    p4.SetActive(true);
        //    pm4.SetActive(false);
        //}
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

        if(espejoType == EspejoType.Espejo1)UserData.completoNivel1 = true;
        if(espejoType == EspejoType.Espejo2)UserData.completoNivel2 = true;
        if(espejoType == EspejoType.Espejo3)UserData.completoNivel3 = true;
       
        GetComponent<BoxCollider2D>().enabled = false;
        panelFeedback.transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        countPiezas = 0;

        turnCollider.GetComponent<BoxCollider2D>().enabled = true;

        if (!UserData.playerGuide4 && UserData.completoNivel1)
        {
            playerGuide4.SetActive(true);
            UserData.playerGuide4 = true;
        }
        else
        {
            playerGuide4.SetActive(false);
        }

        if (!UserData.playerGuide5 && UserData.completoNivel1 && UserData.completoNivel2 && UserData.completoNivel3)
        {
            playerGuide5.SetActive(true);
            UserData.playerGuide5 = true;
        }
        else
        {
            playerGuide5.SetActive(false);
        }
            //}

            //switch (espejoType)
            //{
            //    case EspejoType.Espejo1:
            //        UserData.piezaA_N1 = false;
            //        UserData.piezaB_N1 = false;
            //        UserData.piezaC_N1 = false;
            //        UserData.piezaD_N1 = false;
            //        UserData.usedPiezaA_N1 = false;
            //        UserData.usedPiezaB_N1 = false;
            //        UserData.usedPiezaC_N1 = false;
            //        UserData.usedPiezaD_N1 = false;
            //        break;

            //    case EspejoType.Espejo2:
            //        UserData.piezaA_N2 = false;
            //        UserData.piezaB_N2 = false;
            //        UserData.piezaC_N2 = false;
            //        UserData.piezaD_N2 = false;
            //        UserData.usedPiezaA_N2 = false;
            //        UserData.usedPiezaB_N2 = false;
            //        UserData.usedPiezaC_N2 = false;
            //        UserData.usedPiezaD_N2 = false;
            //        break;

            //    case EspejoType.Espejo3:
            //        UserData.piezaA_N3 = false;
            //        UserData.piezaB_N3 = false;
            //        UserData.piezaC_N3 = false;
            //        UserData.piezaD_N3 = false;
            //        UserData.usedPiezaA_N3 = false;
            //        UserData.usedPiezaB_N3 = false;
            //        UserData.usedPiezaC_N3 = false;
            //        UserData.usedPiezaD_N3 = false;
            //        break;
            //}

            //LevelManager.usedPA = false;
            //LevelManager.usedPB = false;
            //LevelManager.usedPC = false;
            //LevelManager.usedPD = false;

            //PlayerControllerNew.piezaA = false;
            //PlayerControllerNew.piezaB = false;
            //PlayerControllerNew.piezaC = false;
            //PlayerControllerNew.piezaD = false;
        }
}
