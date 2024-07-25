using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class PlayerControllerNew : MonoBehaviour
{
    #region Variables
    [Header("Class References")]
    [SerializeField] private GameObject GlowSpriteEffect;
    [SerializeField] private GameObject effectPanel;
    [SerializeField] private GameObject panelFeedback;
    [SerializeField] private GameObject panelFeedbackBadFloor;
    [SerializeField] private GameObject panelInmunidadTuto;
    [SerializeField] private GameObject panelPocaVidaTuto;
    [SerializeField] private GameObject panelPiezasTuto;
    [SerializeField] private UI_Piezas piezasPanel;
    [SerializeField] private UI_SaludBar saludBar;
    [SerializeField] private UI_FeedbackSalud ui_FeedbackSalud;
    [SerializeField] private UI_IndestructibleBar ui_IndestructibleBar;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    private PlayerMovementNew playerMovement;
    private GameObject currentItem;
    private GameObject currenBadFloortItem;
    private GameObject currenBadFloortDestructibleItem;
    private GameObject currenPiece;
    [HideInInspector] public Salud currentItemSalud;
    [SerializeField] private GameObject enemyAttached;
    [SerializeField] private GameObject playerExplode;
    [SerializeField] private GameObject ExplodeObject;
    [SerializeField] private UI_SaludAttachedBar ui_enemyAttachedBar;

    private AudioPause audioPause;
    private LevelManager levelManager;


    [Header("Stats")]
    [SerializeField] private float saludAmount;
    private float currentLuminance = 1;
    public AnimationCurve animationItemPositionCurve, animationItemScaleCurve;

    [Header("Components")]
    [SerializeField] private Gradient colorEnemyGradient;
    [SerializeField] private Material[] materials;
    private AudioSource audioSource;
    [SerializeField] GameObject[] pieces;
    [HideInInspector] public Color color;
    [SerializeField] GameObject NewStartPosition;


    private Image healthFillBar;
    private List<CompositeCollider2D> compositeColliders = new List<CompositeCollider2D>();

    [Header("Bools")]
    public float currentSalud = 0;
    public bool isDie = false;
    public bool isCannabis, isCocaMetaHero, isPsilo, isAlcohol, isTabaco;
    public bool escudo, saltoDoble, vidaExtra, paracaidas;
    public bool isDrugged;
    public bool isAttack = true;
    private bool isShowPanel;
    private bool isIndestructible;
    public static bool piezaA, piezaB, piezaC, piezaD;
    public static bool showInmunidadPanel = true, showPocaVidePanel = true, showPiezasPanel = true;

    [Header("Coroutines")]
    private Coroutine blinkCoroutine;
    private Coroutine enemyEffectCoroutine;
    private Coroutine coroutineFeedback;
    private Coroutine takeSaludAnim;
    private Coroutine activarEnfasis;
    public Coroutine enemyCameraShake;
    public Coroutine inmunidadCoroutine;

    [Header("Intern Variables")]
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 targetPiecePosition;
    [HideInInspector]public bool isSmokePanelEffect;
    private CinemachineVirtualCamera cm;
    public bool doingEnemyShake;
    #endregion
    #region Unity Callbacks
    private void Awake()
    {
    }
    public void ActiveGamePanelsl(bool active)
    {
        showInmunidadPanel = active;
        showPocaVidePanel = active;
    }
    private void Start()
    {

        playerMovement = GetComponent<PlayerMovementNew>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioPause = FindAnyObjectByType<AudioPause>();
        saludBar.healthFillBar.fillAmount = currentSalud;
        cm = CameraManager.instance.currentCamera;

        //if (ui_enemyAttachedBar) print(ui_enemyAttachedBar.gameObject.name);

        switch (currentSalud)
        {
            case 0:
              
                break;
            case .1f:
                
                AdjustLuminance(0);
                break;
            case .2f:
               
                AdjustLuminance(.2f);
                break;
            case .3f:
               
                AdjustLuminance(.3f);
                break;
            case .4f:
               
                AdjustLuminance(.4f);
                break;
            case .5f:
              
                AdjustLuminance(.5f);
                break;
            case .6f:
               
                AdjustLuminance(.6f);
                break;
            case .7f:
             
                AdjustLuminance(.7f);
                break;
            case .8f:
              
                AdjustLuminance(.8f);
                break;
            case .9f:
               
                AdjustLuminance(.9f);
                break;
            case 1:
               
                AdjustLuminance(1);
                break;
            default:
                AdjustLuminance(1);
                break;
        }




    }


    private void Update()
    {
              //print("SaludActual: " + currentSalud + ". Boleano barrita: " + ui_enemyAttachedBar.startUpdateTimeCoroutine + ". Boleano efecto panel: " + isSmokePanelEffect);
      //  print("Haciendo Enemy Shake: "+doingEnemyShake);
        targetPosition = GetWorldPositionFromUI(saludBar.GetComponent<RectTransform>());
        targetPosition = targetPosition + new Vector3(0, -1.25f, 0);
        
        if(currenPiece != null && pieces != null && currenPiece.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasA)
            targetPiecePosition = GetWorldPositionFromUI(pieces[0].GetComponent<RectTransform>());
        if(currenPiece != null && pieces != null && currenPiece.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasB)
            targetPiecePosition = GetWorldPositionFromUI(pieces[1].GetComponent<RectTransform>());
        if(currenPiece != null && pieces != null && currenPiece.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasC)
            targetPiecePosition = GetWorldPositionFromUI(pieces[2].GetComponent<RectTransform>());
        if(currenPiece != null && pieces != null && currenPiece.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasD)
            targetPiecePosition = GetWorldPositionFromUI(pieces[3].GetComponent<RectTransform>());

        //if (currentSalud > .9f)
        //{
        //    currentItemSalud.healthType = Salud.HealthType.Indestructible;
        //    ui_FeedbackSalud.AssignFeedbackSprite();
        //    inmunidadCoroutine = StartCoroutine(Inmunidad());
        //    ui_IndestructibleBar.gameObject.SetActive(true);
        //    ui_IndestructibleBar.UpdateTime(10);
        //    return;
        //}
        //else
        //{

        //    if(inmunidadCoroutine != null) StopCoroutine(inmunidadCoroutine);
        //    ui_IndestructibleBar.gameObject.SetActive(false);
        //    ui_IndestructibleBar.UpdateTime(0);
        //    return;
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Salud")
        {

         
            audioSource.Stop();

            currentSalud += saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;
            currentSalud = (float)Math.Round(currentSalud, 1);

            //print("Current Salud: "+currentSalud);
            //print("Fill Amount: " + saludBar.healthFillBar.fillAmount);


            currentItemSalud = collision.GetComponent<Salud>();
            ui_FeedbackSalud.AssignFeedbackSprite();

            //if (currentSalud > .9f)
            //{
            //    currentItemSalud.healthType = Salud.HealthType.Indestructible;
            //    ui_FeedbackSalud.AssignFeedbackSprite();
            //    inmunidadCoroutine = StartCoroutine(Inmunidad());
            //    ui_IndestructibleBar.gameObject.SetActive(true);
            //    ui_IndestructibleBar.UpdateTime(10);

            //}
            //else
            //{
            //    ui_IndestructibleBar.gameObject.SetActive(false);

            //}

            collision.GetComponent<BoxCollider2D>().enabled = false;

            if (ui_enemyAttachedBar.startUpdateTimeCoroutine)
            {
                effectPanel.SetActive(false);
                //enemyAttached.SetActive(false);
                Color newColor = new Color(1, 1, 1, 0);
                ui_enemyAttachedBar.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = newColor;
                ui_enemyAttachedBar.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = newColor;
                if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
                if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
                //if (enemyCameraShake != null) StopCoroutine(enemyCameraShake);
                //if (ui_enemyAttachedBar.updateTimeCoroutine != null) StopCoroutine(ui_enemyAttachedBar.updateTimeCoroutine);
                // ui_enemyAttachedBar.startUpdateTimeCoroutine = false;
            }

            if (doingEnemyShake) 
            {
               if(enemyCameraShake != null) StopCoroutine(enemyCameraShake);
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }

            startPosition = collision.ClosestPoint(transform.position);
            currentItem = collision.gameObject;

            if (takeSaludAnim != null) 
            {
                StopCoroutine(takeSaludAnim);
            }
            takeSaludAnim = StartCoroutine(TakeSaludAnim());

            if (activarEnfasis != null)
            {
                StopCoroutine (activarEnfasis);
            }
            activarEnfasis = StartCoroutine(ActivarEnfasis());

            ShowFeedback();

            switch (currentSalud)
            {
                case 0:
                   
                    playerMovement.Die();
                 
                    break;
                case .1f:
                 
                    AdjustLuminance(0);
                    break;
                case .2f:
               
                    AdjustLuminance(.2f);
                    break;
                case .3f:
                 
                    AdjustLuminance(.3f);
                    break;
                case .4f:
                   
                    AdjustLuminance(.4f);
                    break;
                case .5f:
                   
                    AdjustLuminance(.5f);
                    break;
                case .6f:
                   
                    AdjustLuminance(.6f);
                    break;
                case .7f:
                 
                    AdjustLuminance(.7f);
                    break;
                case .8f:
                  
                    AdjustLuminance(.8f);
                    break;
                case .9f:
                  
                    AdjustLuminance(.9f);
                    break;
                case 1:
                    AdjustLuminance(1);
                    currentItemSalud.healthType = Salud.HealthType.Indestructible;
                    ui_FeedbackSalud.AssignFeedbackSprite();
                    inmunidadCoroutine = StartCoroutine(Inmunidad());
                    ui_IndestructibleBar.gameObject.SetActive(true);
                    ui_IndestructibleBar.UpdateTime(10);
                    break;
                default:

                    AdjustLuminance(1);
                    break;
            }


           // return;
        }

        if (collision.tag == "Enemy")
        {

            collision.GetComponent<EnemyNew>().Effect();
            collision.gameObject.SetActive(false);

            currentSalud -= saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;
            currentSalud = (float)Math.Round(currentSalud, 1);

            //if (currentSalud < .9f)
            //{
            //    if (inmunidadCoroutine != null) StopCoroutine(inmunidadCoroutine);
            //    ui_IndestructibleBar.gameObject.SetActive(false);
            //    ui_IndestructibleBar.UpdateTime(0);

            //}

            if (currentSalud < .1f)
            {
                playerMovement.Die();
                return;
            }

            isDrugged = true;

            if (isShowPanel)
            {
                panelFeedback.SetActive(false);
                if (takeSaludAnim != null) StopCoroutine(takeSaludAnim);
                if (activarEnfasis != null) StopCoroutine(activarEnfasis);

            }

            //StopCoroutine(ui_enemyAttachedBar.updateTimeCoroutine);



            switch (currentSalud)
            {
                case 0:

                    break;
                case .1f:

                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }

                    enemyCameraShake = StartCoroutine(EnemyCameraShake(10));
                    CurrentEffectPanel(10);
                    ui_enemyAttachedBar.UpdateTime(10);
                    AdjustLuminance(.1f);
                    break;
                case .2f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }

                    enemyCameraShake = StartCoroutine(EnemyCameraShake(9));
                    CurrentEffectPanel(9);
                    ui_enemyAttachedBar.UpdateTime(9);
                    AdjustLuminance(.2f);
                    break;
                case .3f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(8));
                    CurrentEffectPanel(8);
                    ui_enemyAttachedBar.UpdateTime(8);
                    AdjustLuminance(.3f);
                    break;
                case .4f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(7));
                    CurrentEffectPanel(7);                  
                    ui_enemyAttachedBar.UpdateTime(7);
                    AdjustLuminance(.4f);
                    break;
                case .5f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(6));
                    CurrentEffectPanel(6);                    
                    ui_enemyAttachedBar.UpdateTime(6);
                    AdjustLuminance(.5f);
                    break;
                case .6f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(5));
                    CurrentEffectPanel(5); 
                    ui_enemyAttachedBar.UpdateTime(5);
                    AdjustLuminance(.6f);
                    break;
                case .7f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(4));
                    CurrentEffectPanel(4);                  
                    ui_enemyAttachedBar.UpdateTime(4);                   
                    AdjustLuminance(.7f);
                    break;
                case .8f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(3));
                    CurrentEffectPanel(3);                  
                    ui_enemyAttachedBar.UpdateTime(3);
                    AdjustLuminance(.8f);
                    break;
                case .9f:
                    if (isIndestructible)
                    {
                        StopCoroutine(inmunidadCoroutine);
                        ui_IndestructibleBar.gameObject.SetActive(false);
                        GlowSpriteEffect.SetActive(false);

                    }
                    enemyCameraShake = StartCoroutine(EnemyCameraShake(2));
                    CurrentEffectPanel(2);             
                    ui_enemyAttachedBar.UpdateTime(2);
                    AdjustLuminance(.9f);
                    break;
                case 1:
                    AdjustLuminance(1);
                    break;
                default:
                    ui_enemyAttachedBar.UpdateTime(1);
                    break;
            }

            StartBlinking(0);
            StartCoroutine(DeactivateEnfasis());

           // if (currentSalud != 100) spriteRenderer.material = materials[0];




            //  return;
        }

        if (collision.tag == "Pieza")
        {

            if(showPiezasPanel)
            {
                audioPause.Pause(true);
                playerMovement.swipeDetector.gameObject.SetActive(false);
                panelPiezasTuto.SetActive(true);
                showPiezasPanel = false;
            }
            currenPiece = collision.gameObject;
            if ( collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasA)
            {                
                piezaA = true;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaA.SetActive(false);
            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasB)
            {
                piezaB = true;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaB.SetActive(false);

            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasC)
            {
                piezaC = true;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaC.SetActive(false);

            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasD)
            {
                piezaD = true;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaD.SetActive(false);

            }

        }

        if (collision.tag == "BadFloor")
        {
            if (isIndestructible)
            {
                currenBadFloortDestructibleItem = collision.gameObject;
                currenBadFloortDestructibleItem.SetActive(false);
                playerMovement.StartCameraShake(.1f);
                Vector2 contactPoint = collision.ClosestPoint(transform.position);
                ExplodeObject.SetActive(true);
                GameObject explodeInstance = Instantiate(ExplodeObject, contactPoint, Quaternion.identity);
                ParticleSystem particleSystem = explodeInstance.GetComponentInChildren<ParticleSystem>();
                particleSystem.Play();
                Destroy(explodeInstance, particleSystem.main.duration);
                ExplodeObject.SetActive(false);

            }

        }
    }

    private IEnumerator ResetHitBadFloorDestructible()
    {
        currenBadFloortDestructibleItem.SetActive(false);
        yield return new WaitForSeconds(1f);
        currenBadFloortDestructibleItem.SetActive(true);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BadFloor")
        {
            if (playerMovement.doingRoll)
            {

                playerMovement.capsuleCollider.size = playerMovement.capsuleColliderSize;
                playerMovement.doingRoll = false;
            }        
            currentSalud -= saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;

            currenBadFloortItem = collision.gameObject;

            //if (currentSalud < .9f)
            //{
            //    if (inmunidadCoroutine != null) StopCoroutine(inmunidadCoroutine);
            //    ui_IndestructibleBar.gameObject.SetActive(false);
            //    ui_IndestructibleBar.UpdateTime(0);
            //}
            if (isIndestructible)
            {
                StopCoroutine(inmunidadCoroutine);
                ui_IndestructibleBar.gameObject.SetActive(false);
            }

            StartCoroutine(HitBadFloor());

        }

        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //    currentSalud -= saludAmount;
        //    saludBar.UpdateHealth(-saludAmount);
        //    currentSalud = Mathf.Clamp(currentSalud, 0f, 100);
        //    saludBar.currentAdiccion = currentSalud;
        //    StartBlinking(0);
        //    StartCoroutine(DeactivateEnfasis());

        //    // if (isIndestructible)
        //    // {
        //    collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        //    //collision.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        //    collision.gameObject.GetComponent<ExplodeOnClick>().Explode();
        //    // }
        //}
    }
    public void FirstEnemyEffect()
    {
        effectPanel.SetActive(true);
        if (ui_enemyAttachedBar) ui_enemyAttachedBar.UpdateTime(10);
        enemyEffectCoroutine = StartCoroutine(CurrentEffect(10));
        GlowSpriteEffect.SetActive(false);
        AdjustLuminance(0);
    }
    public void TakePiece()
    {
        StartCoroutine(TakePieceAnim());
    }
    private IEnumerator TakePieceAnim()
    {
        Vector3 startPosition = currenPiece.transform.position;
        Vector3 startScale = currenPiece.transform.localScale;

        float elapsedTime = 0;
        float duration = 0.5f;
        float duration2 = .5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float t1 = elapsedTime / duration2;
            float curvePosValue = animationItemPositionCurve.Evaluate(t);
            float curveScaleValue = animationItemScaleCurve.Evaluate(t1);
            currenPiece.transform.position = Vector3.Lerp(startPosition, targetPiecePosition, curvePosValue);
            currenPiece.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, curveScaleValue);
            yield return null;
        }

        // Asegurarse de que el valor final sea el targetPosition
        currenPiece.transform.position = targetPiecePosition;
        currenPiece.SetActive(false);
    }
    private IEnumerator HitBadFloor()
    {
        playerMovement.isHitBadFloor = true;
       // playerMovement.anim.SetBool("HitBadFloor", true);

        if (!playerMovement.isFallingMode)
        {
            StartCoroutine(DeactivateEnfasis());
           // playerMovement.rb.gravityScale = 0;
            if(!isSmokePanelEffect)playerMovement.inputsEnabled = false;
            playerMovement.rb.bodyType = RigidbodyType2D.Static;
            playerMovement.direction = Vector2.zero;
            panelFeedbackBadFloor.SetActive(true);
            StartBlinking(2);
            playerMovement.StartCameraShake(.1f);

            float shakeDuration = 0.25f;
            float shakeMagnitude = 1.5f;
            float inertiaDuration = 0.5f;
            float returnDuration = 0.25f;
            Transform cameraTarget;
            Vector3 originalCameraPosition;
            bool isShaking = false;
            cameraTarget = CameraManager.instance.currentCamera.Follow;
            originalCameraPosition = cameraTarget.position;
            // Inertia movement
            Vector3 inertiaTargetPosition = cameraTarget.position + (Vector3)transform.right * shakeMagnitude;
            float elapsed = 0f;

            while (elapsed < inertiaDuration)
            {
                cameraTarget.position = Vector3.Lerp(originalCameraPosition, inertiaTargetPosition, elapsed / inertiaDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTarget.position = originalCameraPosition;
            isShaking = false;

            // Return to player
            elapsed = 0f;
            while (elapsed < returnDuration)
            {
                cameraTarget.position = Vector3.Lerp(inertiaTargetPosition, originalCameraPosition, elapsed / returnDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTarget.position = originalCameraPosition;

            yield return new WaitForSeconds(.5f);
            playerMovement.direction = new Vector2(1.2f, 1);
            playerMovement.inputsEnabled = true;
            playerMovement.isHitBadFloor = false;
            panelFeedbackBadFloor.SetActive(false);

            //playerMovement.rb.gravityScale = playerMovement.gravityScale;
            StartCoroutine(ResetCollision());
            playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
            playerMovement.anim.SetBool("HitBadFloor", false);
            if(!playerMovement.isGrounded)playerMovement.anim.SetBool("Walk", true);   
            else playerMovement.anim.SetBool("Jump", true);

        }
        else
        {
            StartCoroutine(DeactivateEnfasis());
            playerMovement.rb.bodyType = RigidbodyType2D.Static;

            if(!isSmokePanelEffect)playerMovement.inputsEnabled = false;
            playerMovement.rb.bodyType = RigidbodyType2D.Static;
            playerMovement.direction = Vector2.zero;

            panelFeedbackBadFloor.SetActive(true);
            StartBlinking(2);
            playerMovement.StartCameraShake(.1f);


            float shakeDuration = 0.25f;
            float shakeMagnitude = 1.5f;
            float inertiaDuration = 0.5f;
            float returnDuration = 0.25f;
            Transform cameraTarget;
            Vector3 originalCameraPosition;
            bool isShaking = false;
            cameraTarget = CameraManager.instance.currentCamera.Follow;
            originalCameraPosition = cameraTarget.position;
            // Inertia movement
            Vector3 inertiaTargetPosition = cameraTarget.position + (Vector3)transform.up * -shakeMagnitude;
            float elapsed = 0f;

            while (elapsed < inertiaDuration)
            {
                cameraTarget.position = Vector3.Lerp(originalCameraPosition, inertiaTargetPosition, elapsed / inertiaDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTarget.position = originalCameraPosition;
            isShaking = false;

            // Return to player
            elapsed = 0f;
            while (elapsed < returnDuration)
            {
                cameraTarget.position = Vector3.Lerp(inertiaTargetPosition, originalCameraPosition, elapsed / returnDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTarget.position = originalCameraPosition;

            yield return new WaitForSeconds(.5f);
            playerMovement.direction = new Vector2(1.2f, 1);
            playerMovement.inputsEnabled = true;
            playerMovement.isHitBadFloor = false;
            panelFeedbackBadFloor.SetActive(false);   
            StartCoroutine(ResetCollision());
            playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;
            playerMovement.anim.SetBool("HitBadFloor", false);
            if (!playerMovement.isGrounded) playerMovement.anim.SetBool("Walk", true);
            else playerMovement.anim.SetBool("Jump", true);
        }



    }
    private IEnumerator ResetCollision()
    {
        if (playerMovement.isFallingMode) playerMovement.rb.gravityScale = 1;
        if(currenBadFloortItem != null) currenBadFloortItem.GetComponent<CompositeCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(2);
        if (playerMovement.isFallingMode) playerMovement.rb.gravityScale = 0;
        if (currenBadFloortItem != null) currenBadFloortItem.GetComponent<CompositeCollider2D>().isTrigger = false;
    }
    private void GetCompositeColliders2D()
    {
        // Obtener todos los GameObjects activos en la escena
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Iterar a través de todos los GameObjects y buscar CompositeCollider2D con el tag "BadFloor"
        foreach (GameObject obj in allObjects)
        {
            // Obtener el CompositeCollider2D del GameObject actual, si existe
            CompositeCollider2D collider = obj.GetComponent<CompositeCollider2D>();

            // Verificar si el objeto tiene el tag "BadFloor" y el componente CompositeCollider2D
            if (obj.CompareTag("BadFloor") && collider != null)
            {
                // Agregar el CompositeCollider2D a la lista
                compositeColliders.Add(collider);
            }
        }

        // Método para convertir todos los CompositeCollider2D a trigger
        // SetAllCompositeCollidersTrigger(compositeColliders, true);
    }
    void SetAllCompositeCollidersTrigger(List<CompositeCollider2D> colliders, bool isTrigger)
    {
        foreach (CompositeCollider2D collider in colliders)
        {
            collider.isTrigger = isTrigger;
        }
    }


    #endregion
    #region Enemy Managment


    private IEnumerator TakeEnemyBarAnim(float duration)
    {
        float startFillAmount = healthFillBar.fillAmount;
        float targetFillAmount = 1 / 100;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            healthFillBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
            //print("Durecion de animBar: "+ elapsedTime);
            //healthFillBar.color = colorEnemyGradient.Evaluate(targetFillAmount);
        }
        currentItem.gameObject.SetActive(false);
    }

    private IEnumerator TakeEnemyAnim()
    {
        Vector3 targetPosition = transform.localPosition;
        targetPosition = targetPosition + new Vector3(1.5f, .15f, 0);
        float elapsedTime = 0;
        float duration = 0.25f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            currentItem.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        currentItem.gameObject.transform.parent = transform;
        currentItem.transform.position = targetPosition;

    }


    private IEnumerator Inmunidad()
    {
        isIndestructible = true;
        //spriteRenderer.material = materials[1];
        GlowSpriteEffect.SetActive(true);

        GetCompositeColliders2D();
        SetAllCompositeCollidersTrigger(compositeColliders, true);
        yield return new WaitForSecondsRealtime(10);
        GlowSpriteEffect.SetActive(false);
        ui_IndestructibleBar.gameObject.SetActive(false);
        //spriteRenderer.material = materials[0];
        isIndestructible = false;
        SetAllCompositeCollidersTrigger(compositeColliders, false);


    }
    private void CurrentEffectPanel(float delay)
    {
        if(enemyEffectCoroutine != null)
        {
            StopCoroutine(enemyEffectCoroutine);
            isSmokePanelEffect = false;
        }
        if(!isSmokePanelEffect) enemyEffectCoroutine = StartCoroutine(CurrentEffect(delay));

    }
    private IEnumerator CurrentEffect(float delay)
    {
        isSmokePanelEffect = true;
        audioSource.Play();
        effectPanel.SetActive(true);
        //effectPanel.GetComponent<Animator>().SetBool("Smoke", true);
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        effectPanel.SetActive(false);
        //enemyAttached.SetActive(false);
        isDrugged = false;
        isCannabis = false;
        isCocaMetaHero = false;
        isPsilo = false;
        isAlcohol = false;
        isTabaco = false;
        playerMovement.inputsEnabled = true;
        isSmokePanelEffect = false;
        //if (!isDie && playerMovement.canMove)
        //{
        //    audioSource.Play();
        //    effectPanel.SetActive(true);
        //    //effectPanel.GetComponent<Animator>().SetBool("Smoke", true);
        //    yield return new WaitForSecondsRealtime(delay);
        //    audioSource.Stop();
        //    effectPanel.SetActive(false);
        //    enemyAttached.SetActive(false);
        //    isCannabis = false;
        //    isCocaMetaHero = false;
        //    isPsilo = false;
        //    isAlcohol = false;
        //    isTabaco = false;
        //    isDrugged = false;
        //    playerMovement.inputsEnabled = true;
        //}
    }

    IEnumerator DeactivateEnfasis()
    {
        yield return new WaitForSeconds(.5f);
        Animator animator = saludBar.GetComponent<Animator>();
        if (!animator.isActiveAndEnabled) animator.enabled = true;
        animator.Rebind();
        animator.Play("Wrong");
       // animator.SetBool("Enfasis", false);
    }

    #endregion
    #region Salud Managment
    public void ShowFeedback()
    {
        //if (isShowPanel) StopCoroutine(coroutineFeedback);
        if (coroutineFeedback != null)
        {
            StopCoroutine(coroutineFeedback);            
            isShowPanel = false;
        }
        if(!isShowPanel) coroutineFeedback = StartCoroutine(Feedback());

    }
    private IEnumerator Feedback()
    {
        isShowPanel = true;
        panelFeedback.SetActive(true);
        yield return new WaitForSeconds(2f);
        panelFeedback.SetActive(false);
        
        isShowPanel = false;
    }
    private IEnumerator TakeSaludAnim()
     {
        Vector3 startPosition = currentItem.transform.position;
        //targetPosition = GetWorldPositionFromUI(saludBar.GetComponent<RectTransform>());
        
        //Vector3 targetPosition = currentItem.transform.position;
        //targetPosition = targetPosition + new Vector3(8, 4, 0);
        Vector3 startScale = currentItem.transform.localScale;

        float elapsedTime = 0;
        float duration = 0.5f;
        float duration2 = .5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float t1 = elapsedTime / duration2;
            float curvePosValue = animationItemPositionCurve.Evaluate(t);
            float curveScaleValue = animationItemScaleCurve.Evaluate(t1);
            currentItem.transform.position = Vector3.Lerp(startPosition, targetPosition, curvePosValue);
            currentItem.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, curveScaleValue);
            yield return null;
        }

        // Asegurarse de que el valor final sea el targetPosition
        currentItem.transform.position = targetPosition;
        currentItem.SetActive(false);
    }

    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
        // Obtener la posición en pantalla del elemento UI
        Vector3 screenPos = uiElement.position;
        // Convertir la posición en pantalla a posición en el mundo
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, screenPos, Camera.main, out worldPos);
        return worldPos;
    }
    IEnumerator ActivarEnfasis()
    {
        yield return new WaitForSeconds(.5f);
        Animator animator = saludBar.GetComponent<Animator>();
        if (!animator.isActiveAndEnabled) animator.enabled = true;
        animator.Rebind();
        animator.Play("Enfasis");
        //animator.SetBool("Enfasis", true);
    }
    #endregion
    #region Blinking
    public void StartEnemyCameraShake(float tiempo)
    {

        if (enemyCameraShake != null)
        {
            StopCoroutine(enemyCameraShake);
            doingEnemyShake = false;
        }
        enemyCameraShake = StartCoroutine(EnemyCameraShake(tiempo)); 
    }

    private IEnumerator EnemyCameraShake(float tiempo)
    {
        doingEnemyShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5;
        print(". Duracion: " + tiempo);
        yield return new WaitForSeconds(tiempo);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        doingEnemyShake = false;
    }

    public void StartBlinking(float duration)
    {

        //if(isDrugged) playerMovement.inputsEnabled = false;
        if (playerMovement.canMove)
        {

            if (!isAttack)
            {
                if (blinkCoroutine != null)
                {
                    StopCoroutine(blinkCoroutine);
                }

                blinkCoroutine = StartCoroutine(BlinkAlpha(duration));
            }
        } 
    }

    private IEnumerator BlinkAlpha(float duration)
    {
        //capsuleCollider.enabled = false;
        //yield return new WaitForSeconds(duration);
        isAttack = true;       
        float blinkSpeed = 6.0f; // Velocidad del parpadeo
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsedTime * blinkSpeed, 1.0f);
            SetAlpha(alpha);
            yield return null;
        }

        // Asegúrate de que el sprite esté completamente visible al final
        //capsuleCollider.enabled = false;
        SetAlpha(1.0f);
        isAttack = false;
        //if (!isDrugged) playerMovement.inputsEnabled = true;
        blinkCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    #endregion
    #region Color Managment
    // Convertir de RGB a HSV
    private static void RGBToHSV(Color color, out float hue, out float saturation, out float value)
    {
        float max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
        float min = Mathf.Min(color.r, Mathf.Min(color.g, color.b));

        hue = 0f;
        if (max == min)
        {
            hue = 0f; // Achromatic
        }
        else if (max == color.r)
        {
            hue = (color.g - color.b) / (max - min);
            hue = (hue < 0) ? hue + 6 : hue;
        }
        else if (max == color.g)
        {
            hue = (color.b - color.r) / (max - min) + 2;
        }
        else if (max == color.b)
        {
            hue = (color.r - color.g) / (max - min) + 4;
        }

        hue /= 6;

        saturation = (max == 0) ? 0 : (max - min) / max;
        value = max;
    }

    // Convertir de HSV a RGB
    private static Color HSVToRGB(float hue, float saturation, float value)
    {
        int i = Mathf.FloorToInt(hue * 6);
        float f = hue * 6 - i;
        float p = value * (1 - saturation);
        float q = value * (1 - f * saturation);
        float t = value * (1 - (1 - f) * saturation);

        switch (i % 6)
        {
            case 0: return new Color(value, t, p);
            case 1: return new Color(q, value, p);
            case 2: return new Color(p, value, t);
            case 3: return new Color(p, q, value);
            case 4: return new Color(t, p, value);
            case 5: return new Color(value, p, q);
            default: return Color.black; // Esto nunca debería suceder
        }
    }

    public void AdjustLuminance(float newLuminance)
    {
        // Obtén el color actual del SpriteRenderer
         color = spriteRenderer.color;

        // Convierte el color de RGB a HSV
        float hue, saturation, value;
        RGBToHSV(color, out hue, out saturation, out value);

        // Ajusta la luminancia (valor en el espacio HSV)
        value = newLuminance;

        // Convierte el color de vuelta de HSV a RGB
        color = HSVToRGB(hue, saturation, value);

        // Asigna el color ajustado al SpriteRenderer
        spriteRenderer.color = color;
    }

    //private void SetPlayerLuminance()
    //{
    //    switch (i % 6)
    //    {
    //        case 0: return new Color(value, t, p);
    //        case 1: return new Color(q, value, p);
    //        case 2: return new Color(p, value, t);
    //        case 3: return new Color(p, q, value);
    //        case 4: return new Color(t, p, value);
    //        case 5: return new Color(value, p, q);
    //        default: return Color.black; // Esto nunca debería suceder
    //    }
    //}
    #endregion

}
