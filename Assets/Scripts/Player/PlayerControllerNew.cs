using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject panelInmunidad;
    [SerializeField] private GameObject panelPocaVida;
    [SerializeField] private  UI_Piezas piezasPanel;
    [SerializeField] private  UI_SaludBar saludBar;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    private PlayerMovementNew playerMovement;
    private GameObject currentItem;
    private GameObject currenPiece;
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
    [SerializeField]private Material[] materials;
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
    public static bool showInmunidadPanel = true, showPocaVidePanel = true;

    [Header("Coroutines")]
    private Coroutine blinkCoroutine;
    private Coroutine enemyEffectCoroutine;
    private Coroutine coroutineFeedback;

    [Header("Intern Variables")]
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 targetPiecePosition;

    #endregion
    #region Unity Callbacks
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
        //saludBar.UpdateHealth(currentSalud);
        //print("Current Salud: " + currentSalud);
        //print("Fill Amount: " + saludBar.healthFillBar.fillAmount);

        //if(levelManager.currentScene == LevelManager.CurrentScene.Nivel1 && NewStartPosition != null && TutorialManager.endTutorial)
        //{
        //   transform.position = new Vector3(NewStartPosition.transform.position.x, transform.position.y, transform.position.z);
        //}
        
        switch (currentSalud)
        {
            case 0:
                // ui_enemyAttachedBar.UpdateTime(999999999);
                isIndestructible = false;
                break;
            case .1f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(0);
                break;
            case .2f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.2f);
                break;
            case .3f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.3f);
                break;
            case .4f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.4f);
                break;
            case .5f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.5f);
                break;
            case .6f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.6f);
                break;
            case .7f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.7f);
                break;
            case .8f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.8f);
                break;
            case .9f:
                isIndestructible = false;
                GlowSpriteEffect.SetActive(false);
                AdjustLuminance(.9f);
                break;
            case 1:
                //isIndestructible = true;
                //GlowSpriteEffect.SetActive(true);
                AdjustLuminance(1);
                break;
            default:
                ui_enemyAttachedBar.UpdateTime(1);
                AdjustLuminance(1);
                break;
        }


    }


    private void Update()
    {
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Salud")
        {
            audioSource.Stop();
           // StopAllCoroutines();
            //saludBar.UpdateHealth(saludAmount);
            //saludBar.currentAdiccion = currentSalud;
            currentSalud += saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;
            print("Current Salud: "+currentSalud);
            print("Fill Amount: " + saludBar.healthFillBar.fillAmount);

            collision.GetComponent<BoxCollider2D>().enabled = false;

            if (isDrugged)
            {
                effectPanel.SetActive(false);
                enemyAttached.SetActive(false);
                if (enemyEffectCoroutine != null) StopCoroutine(enemyEffectCoroutine);
                if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
                isDrugged = false;
            }
            ////if(isDrugged) { isDrugged = false; transform.GetChild(1).gameObject.SetActive(false); }


            startPosition = collision.ClosestPoint(transform.position);
            currentItem = collision.gameObject;
            StartCoroutine(TakeSaludAnim());
            StartCoroutine(ActivarEnfasis());
            ShowFeedback();

            switch (currentSalud)
            {
                case 0:
                    // ui_enemyAttachedBar.UpdateTime(999999999);
                    isIndestructible = false;
                    playerMovement.Die();
                    break;
                case .1f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(0);
                    break;
                case .2f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.2f);
                    break;
                case .3f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.3f);
                    break;
                case .4f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.4f);
                    break;
                case .5f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.5f);
                    break;
                case .6f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.6f);
                    break;
                case .7f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.7f);
                    break;
                case .8f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.8f);
                    break;
                case .9f:
                    isIndestructible = false;
                    GlowSpriteEffect.SetActive(false);
                    AdjustLuminance(.9f);
                    break;
                case 1:
                    //isIndestructible = true;
                    //GlowSpriteEffect.SetActive(true);
                    AdjustLuminance(1);
                    StartCoroutine(Inmunidad());
                    audioPause.Pause(true);
                    if (showInmunidadPanel) panelInmunidad.SetActive(true);
                    //if (showInmunidadPanel)
                    //{
                    //    audioPause.Pause(true);
                    //    panelInmunidad.SetActive(true);
                    //    //playerMovement.inputsEnabled = false;
                    //}
                    //else
                    //{
                    //    panelInmunidad.SetActive(false);
                    //    audioPause.Pause(false);
                    //    //playerMovement.inputsEnabled = true;
                    //}
                    break;
                default:
                    ui_enemyAttachedBar.UpdateTime(1);
                    AdjustLuminance(1);
                    break;
            }

            if (currentSalud > 90) spriteRenderer.material = materials[1];

            return;
        }

        if (collision.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);

           // if (!isIndestructible)
           // {

                collision.GetComponent<EnemyNew>().Effect();
                enemyAttached.SetActive(true);

            //currentSalud -= saludAmount;
            //saludBar.UpdateHealth(-saludAmount);
            //currentSalud = Mathf.Clamp(currentSalud, 0f, 100);
            //saludBar.currentAdiccion = currentSalud;
            currentSalud -= saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;
            //print("Current Salud: " + currentSalud);
            //    print("Fill Amount: " + saludBar.healthFillBar.fillAmount);

            isDrugged = true;

                switch (currentSalud)
                {
                    case 0:
                    // ui_enemyAttachedBar.UpdateTime(999999999);
                    //shatterEffect.StartShatter();
                    //StopAllCoroutines();
                    //playerExplode.SetActive(true);
                    //gameObject.SetActive(false);
                    playerMovement.Die();
                    isIndestructible = false;
                        break;
                    case .1f:
                        isIndestructible = false;
                    if (showPocaVidePanel) panelPocaVida.SetActive(true);
                    effectPanel.SetActive(true);
                    //if (showPocaVidePanel)
                    //{
                    //    panelPocaVida.SetActive(true);
                    //    audioPause.Pause(true);
                    //    playerMovement.inputsEnabled = false;
                    //}
                    //else
                    //{
                    //    panelPocaVida.SetActive(false);
                    //    audioPause.Pause(false);
                    //    playerMovement.inputsEnabled = true;
                    //}
                    ui_enemyAttachedBar.UpdateTime(10);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(10));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(0);
                        break;
                    case .2f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(9);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(9));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.2f);
                        break;
                    case .3f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    enemyAttached.SetActive(false);
                        ui_enemyAttachedBar.UpdateTime(8);
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.3f);
                        break;
                    case .4f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(7);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(7));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.4f);
                        break;
                    case .5f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(6);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(6));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.5f);
                        break;
                    case .6f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(5);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(5));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.6f);
                        break;
                    case .7f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(4);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(4));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.7f);
                        break;
                    case .8f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(3);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(3));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.8f);
                        break;
                    case .9f:
                        isIndestructible = false;
                    effectPanel.SetActive(true);
                    ui_enemyAttachedBar.UpdateTime(2);
                        enemyEffectCoroutine = StartCoroutine(CurrentEffect(2));
                        GlowSpriteEffect.SetActive(false);
                        AdjustLuminance(.9f);
                        break;
                    case 1:
                        //isIndestructible = true;
                        //ui_enemyAttachedBar.UpdateTime(1);
                        //enemyEffectCoroutine = StartCoroutine(CurrentEffect(1));
                        //GlowSpriteEffect.SetActive(true);
                        //Inmunidad();
                        AdjustLuminance(1);
                        break;
                    default:
                        ui_enemyAttachedBar.UpdateTime(1);
                        break;
                }

                StartBlinking(0);
                StartCoroutine(DeactivateEnfasis());

            if (currentSalud != 100) spriteRenderer.material = materials[0];

            return;
            //}
        }

        if (collision.tag == "Pieza")
        {
            currenPiece = collision.gameObject;
            if ( collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasA)
            {                
                piezaA = true;
                // piezasPanel.piezaA.GetComponent<Image>().color = Color.red;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaA.SetActive(false);
            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasB)
            {
                piezaB = true;
                //piezasPanel.piezaB.GetComponent<Image>().color = Color.red;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaB.SetActive(false);

            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasC)
            {
                piezaC = true;
                //piezasPanel.piezaC.GetComponent<Image>().color = Color.red;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaC.SetActive(false);

            }
            if (collision.GetComponent<Rompecabezas>().rompecabezasType == Rompecabezas.RompecabezasType.RompecabezasD)
            {
                piezaD = true;
                //piezasPanel.piezaD.GetComponent<Image>().color = Color.red;
                StartCoroutine(TakePieceAnim());
                piezasPanel.piezaD.SetActive(false);

            }

        }

        if (collision.tag == "BadFloor" && !isAttack)
        {
            if (isIndestructible)
            {
                collision.gameObject.SetActive(false);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BadFloor" && !isAttack)
        {
            //StopAllCoroutines();
            if(playerMovement.doingRoll)
            {
                //playerMovement.rb.bodyType = RigidbodyType2D.Kinematic;
                //playerMovement.rb.gravityScale = 0;
                playerMovement.capsuleCollider.size = playerMovement.capsuleColliderSize;
                playerMovement.doingRoll = false;
                //playerMovement.rb.velocity = playerMovement.rbVelocityTemp;
            }
            //currentSalud -= saludAmount;
            //saludBar.UpdateHealth(-saludAmount);
            //currentSalud = Mathf.Clamp(currentSalud, 0f, 100);
            //saludBar.currentAdiccion = currentSalud;
            currentSalud -= saludAmount;
            currentSalud = Mathf.Clamp(currentSalud, 0f, 1);
            saludBar.healthFillBar.fillAmount = currentSalud;

            currentItem = collision.gameObject;
            StartCoroutine(HitBadFloor());

        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            currentSalud -= saludAmount;
            saludBar.UpdateHealth(-saludAmount);
            currentSalud = Mathf.Clamp(currentSalud, 0f, 100);
            saludBar.currentAdiccion = currentSalud;
            StartBlinking(0);
            StartCoroutine(DeactivateEnfasis());

            // if (isIndestructible)
            // {
            collision.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            //collision.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            collision.gameObject.GetComponent<ExplodeOnClick>().Explode();
            // }
        }
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
        if (!playerMovement.isFallingMode)
        {
            StartCoroutine(DeactivateEnfasis());
           // playerMovement.rb.gravityScale = 0;
            playerMovement.inputsEnabled = false;
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
            //playerMovement.rb.bodyType = RigidbodyType2D.Dynamic;

            StartCoroutine(ResetCollision());
        }
        else
        {
            StartCoroutine(DeactivateEnfasis());

            playerMovement.inputsEnabled = false;
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
        }



    }
    private IEnumerator ResetCollision()
    {
        if (playerMovement.isFallingMode) playerMovement.rb.gravityScale = 1;
        currentItem.GetComponent<CompositeCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(1);
        if (playerMovement.isFallingMode) playerMovement.rb.gravityScale = 0;
        currentItem.GetComponent<CompositeCollider2D>().isTrigger = false;
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
        GlowSpriteEffect.SetActive(true);        
        GetCompositeColliders2D();
        SetAllCompositeCollidersTrigger(compositeColliders, true);
        yield return new WaitForSeconds(5);
        isIndestructible = false;
        GlowSpriteEffect.SetActive(false);
        spriteRenderer.material = materials[0];
        SetAllCompositeCollidersTrigger(compositeColliders, false);


    }

    private IEnumerator CurrentEffect(float delay)
    {
        audioSource.Play();
        effectPanel.SetActive(true);
        //effectPanel.GetComponent<Animator>().SetBool("Smoke", true);
        yield return new WaitForSecondsRealtime(delay);
        audioSource.Stop();
        effectPanel.SetActive(false);
        enemyAttached.SetActive(false);
        isCannabis = false;
        isCocaMetaHero = false;
        isPsilo = false;
        isAlcohol = false;
        isTabaco = false;
        isDrugged = false;
        playerMovement.inputsEnabled = true;
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
        if (isShowPanel) StopCoroutine(coroutineFeedback);
        coroutineFeedback = StartCoroutine(Feedback());
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
