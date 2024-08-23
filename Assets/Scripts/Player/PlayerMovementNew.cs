using System.Collections;
using UnityEngine;
using Cinemachine;
using static SwipeDetector;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerMovementNew : MonoBehaviour
{
    private Touch theTouch;
    #region Variables
    public enum MovementMode
    {
        TapMode,
        RunnerMode,
        FallingMode,
        FlappyMode
    }
    [Header("Movement Mode")]
    public MovementMode movementMode;

    [Header("Components Reference")]
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private LevelManager levelManager;
    [HideInInspector] public CapsuleCollider2D capsuleCollider;
    [HideInInspector] public Animator anim;
    private CinemachineVirtualCamera cm;
    private SpriteRenderer spriteRenderer;
    // private PlayerController playerController;
    private PlayerControllerNew playerController;
    private GhostController ghostController;
    [HideInInspector] public CameraFollowObject cameraFollowObject;
    public GameObject cameraFollowGo;
    [HideInInspector] public Coroutine heartbeatShakeSequence, cameraShake, currentMovementCoroutine;

    [Header("Level Colisions")]
    [SerializeField] private List<FallingLevelColliders> fallingColliders;
    [SerializeField] private GameObject flappyCollider;
    [SerializeField] private GameObject faillingCollider;
    [SerializeField] private Vector2 down;
    [SerializeField] private float collisionRatio;
    [SerializeField] private LayerMask layerFloor;

    [Header("Input")]
    public SwipeDetector swipeDetector;
    [HideInInspector] public Vector2 direction;


    [Header("Movement Parameters")]
    [HideInInspector] public float gravityScale;
    [SerializeField] private float velocity = 10;
    [SerializeField] private float jumpStrength = 5;
    [SerializeField] private float jumpFlappyStrength = 5;
    [SerializeField] private float rollVelocity = 20;
    [SerializeField] private float smashVelocity = 20;
    [SerializeField] private float slowFallGravity = 1;
    public float fallingGravity = 1;
    [SerializeField] private float fallingModeMovementAmmount;
    public float fallingVelocity = 20;
    [SerializeField] private float clickMoveSpeed = 5;
    [SerializeField] private float rotationFallingSpeed = 10;
    [SerializeField] private float coyoteTime = .2f;
    [SerializeField] private float jumpBufferTime = .4f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private Material material;

    private float fallSpeedYDampingChangeThreshold;

    [Header("Input Parameters")]
    [SerializeField] private float tapTimeThreshold = .3f;
    [SerializeField] private float swipeDistanceThreshold = 150;
    [HideInInspector] public Vector2 capsuleColliderSize;
    private Vector2 capsuleColliderOffset;
    private Vector3 screenPosition;
    [HideInInspector] public Vector3 targetPosition;
    private float x, y;
    private float xRaw, yRaw;
    private float tapStartTime;
    private float tapDuration;
    private Vector2 tapStartPos;
    private Touch touch;


    [Header("Bools")]
    public bool isPC;
    public bool inputsEnabled = true;
    public bool canMove = true;
    public bool isGrounded;
    public bool canRoll;
    [HideInInspector] public Vector2 rbVelocityTemp;
    public bool doingRoll;
    public bool canSmash;
    public bool doingSmash;
    public bool tapFloor;
    public bool doingShake = false;
    public bool doingJump;
    public bool mouseWalk;
    public bool flappyMode;
    public bool isFallingMode;
    private bool tapDetected;
    private bool twoFingerTapDetected;
    private bool canDoubleJump;
    private float dissolveAmount;
    private bool isAnimating;
    private float clicDirection;
    public bool isFacingRight = true;
    public bool tutorialActive;
    public bool isHitBadFloor;
    public bool isLeftClick;
    public bool isPortalEnter;
    [HideInInspector] public bool isMoving;

    private Coroutine tapCoroutine;
    private bool isDiying;
    #endregion
    #region Unity Callbacks
    private void Awake()

    {
        //if (UserData.terminoTutorial) inputsEnabled = true;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cm = CameraManager.instance.currentCamera;
        //camOffset = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineCameraOffset>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //playerController = GetComponent<PlayerController>();
        playerController = GetComponent<PlayerControllerNew>();
        ghostController = GetComponent<GhostController>();
        cameraFollowObject = cameraFollowGo.GetComponent<CameraFollowObject>();
    }

    void Start()
    {
        isPC = InputManager.isPC;
        capsuleColliderSize = capsuleCollider.size;
        capsuleColliderOffset = capsuleCollider.offset;
        targetPosition = transform.position;
        material = spriteRenderer.material;

        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        //touch = Input.GetTouch(0);
    }
    public void InputEnable(bool pause)
    {
        if (pause)
        {
            inputsEnabled = true;
        }
        else
        {
            inputsEnabled = false;
        }
    }
    private void LerpYDamping()
    {
        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0 && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void Update()
    {
        //print("Gravity Scale: "+rb.gravityScale);
        //print("Velocity: " + rb.velocity + "ANimator: "+anim.GetBool("Roll") + "Boll: " + doingRoll);

        //print("FallingGravity: "+fallingGravity);
        //print("Falling Movement ModeAmmount: "+fallingModeMovementAmmount);
        //print("Falling Velocity: "+fallingVelocity);
        // StartCoroutine(DieAnimation());
        //print("Clic Direction: " + clicDirection);
        //print(material.GetFloat("_DissolveAmmount"));
        if (canMove)
        {
            switch (movementMode)
            {
                case MovementMode.TapMode:
                    //TapMovement();
                    // print("Inputs:" + inputsEnabled + ". Moving: " + isMoving);

                    isFallingMode = false;
                    if (inputsEnabled)
                    {
                        LerpYDamping();
                        TapMovement();
                    }
                    else
                    {
                        //anim.SetBool("SlowWalk", false);
                        //anim.Play("Idle");
                    }
                    break;

                case MovementMode.RunnerMode:




                    isFallingMode = false;

                    float clampedHorizontalSpeed = Mathf.Clamp(rb.velocity.x, 0, 11);
                    rb.velocity = new Vector2(clampedHorizontalSpeed, rb.velocity.y);
                    if(!doingRoll)capsuleCollider.size = capsuleColliderSize;


                    LerpYDamping();
                    RunnerMovement();
                    CheckGround();


                    break;
                case MovementMode.FallingMode:
                    isFallingMode = true;

                    rb.velocity = new Vector2(0, rb.velocity.y);
                    float clampedVerticalSpeed = Mathf.Clamp(rb.velocity.y, -10f, 0f);
                    rb.velocity = new Vector2(rb.velocity.x, clampedVerticalSpeed);

                    GetInputDirection();
                    if (inputsEnabled) GetDirecction();
                    //if(inputsEnabled)TurnCheck();
                    CheckGround();
                    FallingMovement();
                    break;
                case MovementMode.FlappyMode:
                    isFallingMode = false;
                    FlappyMovement();
                    capsuleCollider.size = new Vector2(.7f, 0.0001f);
                    break;
            }
        }

    }

    private void FixedUpdate()
    {
        if (movementMode == MovementMode.FlappyMode)
        {

            transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * 1.5f);
        }
    }


    #endregion
    #region InputGroundChecksWalk
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(((Vector2)transform.position) + down, collisionRatio, layerFloor);
    }
    private void GetInputDirection()
    {
        if (isPC)
        {

            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");

            xRaw = Input.GetAxisRaw("Horizontal");
            yRaw = Input.GetAxisRaw("Vertical");
            //direction = new Vector2(x, y);            
        }

    }
    private void GetDirecction()
    {
        if (direction.x < 0 && transform.localScale.x > 0)
        {

            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if (isPC)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPosition = Input.mousePosition;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;
            }
        }
        else if (!isPC)
        {
            // print("entro al modo tap");
            if (Input.touchCount > 0 || swipeDetector.TapPerformed == true)
            {
                Touch touch = Input.GetTouch(0);
                screenPosition = touch.position;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;
            }
            swipeDetector.TapPerformed = false;
        }

    }
    private void Walk()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(direction.x * velocity, direction.y * rb.velocity.y);
            //rb.velocity = new Vector2(direction.x * velocity,rb.velocity.y);
            GetDirecction();
        }
    }
    #endregion
    #region TapMode
    public void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, -180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            // isFacingRight = !isFacingRight;

            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            //  isFacingRight = !isFacingRight;

            cameraFollowObject.CallTurn();
        }
    }
    public void TurnCheck()
    {
        //  isFacingRight = !isFacingRight;
        GetDirecction();
        if (clicDirection > 0) isFacingRight = false;
        else isFacingRight = true;
        if (clicDirection > 0 && !isFacingRight)
        {
            Turn();
            // isFacingRight = !isFacingRight;

        }
        else if (clicDirection < 0 && isFacingRight)
        {
            Turn();
            // isFacingRight = !isFacingRight;

        }
        //else if (clicDirection > 0 && isFacingRight)
        //{
        //    Turn();
        //}
    }
    //private void TapMovement()
    //{
    //    rb.gravityScale = gravityScale;
    //    direction = new Vector2(x, y);
    //    TurnCheck();
    //    StartCoroutine(MovetoTarget());

    //}


    private void TapMovement()
    {
        if (!isMoving) // Verifica si el personaje no está en movimiento
        {
            if (DetectTap())
            {
                rb.gravityScale = gravityScale;
                direction = new Vector2(x, y);
                TurnCheck();

                // Detener la corrutina actual si existe
                if (currentMovementCoroutine != null)
                {
                    StopCoroutine(currentMovementCoroutine);
                }

                currentMovementCoroutine = StartCoroutine(MoveFixedDistance()); // Inicia la corrutina para mover una distancia fija
            }
            else
            {
                anim.SetBool("SlowWalk", false);
            }
        }
    }

    private IEnumerator MoveFixedDistance()
    {
        isMoving = true; // Marca el inicio del movimiento
        anim.SetBool("SlowWalk", true); // Activar animación de caminar

        Vector2 startPosition = rb.position;
        Vector2 endPosition;

        if (clicDirection > 0)
        {
            endPosition = startPosition + new Vector2(5.25f, 0f);
        }
        else
        {
            endPosition = startPosition + new Vector2(-5.25f, 0f);
        }

        // Realizar un raycast para detectar obstáculos
        RaycastHit2D hit = Physics2D.Raycast(rb.position, endPosition - startPosition, Vector2.Distance(startPosition, endPosition), LayerMask.GetMask("SolidObjects"));

        // Si el raycast golpea algo, ajustar la posición final
        if (hit.collider != null)
        {
            endPosition = hit.point; // Establecer la posición final en el punto de impacto
        }

        // Mover el personaje hacia la posición final
        while (Vector2.Distance(rb.position, endPosition) > 0.1f)
        {
            rb.position = Vector2.MoveTowards(rb.position, endPosition, clickMoveSpeed * Time.deltaTime);

            // Salir del bucle si se inicia un nuevo movimiento
            if (!isMoving)
            {
                yield break;
            }

            yield return null;
        }

        // Desactivar animación de caminar y activar animación de Idle
        anim.SetBool("SlowWalk", false);
        anim.Play("Idle");
        isMoving = false; // Marca el fin del movimiento
    }
    public bool DetectTap()
    {
        if (isPC)
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPosition = Input.mousePosition;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;
                return true; // Se detectó un clic
            }

        }
        else if (!isPC)
        {
            if (Input.touchCount > 0 || swipeDetector.TapPerformed == true)
            {
                Touch touch = Input.GetTouch(0);
                screenPosition = touch.position;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;
                swipeDetector.TapPerformed = false;
                return true; // Se detectó un toque
            }
        }
        return false; // No se detectó un clic o toque
    }


    private IEnumerator MovetoTarget()
    {
        //anim.SetBool("Walk", false);
        anim.SetBool("SlowWalk", false);
        rb.position = Vector3.MoveTowards(rb.position, targetPosition, clickMoveSpeed * Time.deltaTime);
        yield return new WaitWhile(() => rb.position.x == targetPosition.x);
        anim.SetBool("SlowWalk", true);
        //Sprint("llego a su destino");
    }
    #endregion
    #region RunnerMode
    // Coroutine para manejar el tap si no se detectó un swipe
    // Coroutine para manejar el tap si no se detectó un swipe
    // Coroutine para manejar el tap si no se detectó un swipe
    // Manejar swipe detectado
    // Cambios en HandleSwipe y HandleTap
    // Modificación en HandleSwipe para manejar la dirección Down sin restricciones
    private void HandleSwipe()
    {
        // Se permite el roll en cualquier momento, incluso en el aire, sin verificar `doingRoll`
       // if (swipeDetector.swipeDirection == SwipeDirection.Down && inputsEnabled)
        //{
            swipeDetector.TapPerformed = false;
            StartCoroutine(Roll(0, -1, playerController.isCannabis ? 0.25f : 0)); // Ajuste para el roll sin restricciones de estado
            swipeDetector.swipeDirection = SwipeDirection.None;
       // }
    }

    // Modificación en HandleTap para gestionar el roll durante el salto
    private IEnumerator HandleTap()
    {
        yield return new WaitForSeconds(0.05f); // Retardo corto para garantizar que no es un swipe

        if (touch.phase != TouchPhase.Moved && tapDetected && jumpBufferCounter > 0 && !playerController.isCocaMetaHero)
        {
            if (coyoteTimeCounter > 0)
            {
                swipeDetector.TapPerformed = false;
                anim.SetBool("Jump", true);
                canDoubleJump = true;
                StartCoroutine(Jump(playerController.isCannabis || playerController.isAlcohol ? 0.25f : 0));

                if (playerController.isAlcohol)
                {
                    // Se permite el roll sin restricciones de estado
                    canDoubleJump = false;
                    StartCoroutine(Roll(0, -1, 0));
                }
            }
            else if (canDoubleJump && !isGrounded && !playerController.isCocaMetaHero)
            {
                StartCoroutine(Jump(0));
                canDoubleJump = false;
            }
        }
    }
        private void RunnerMovement()
    {
        rb.gravityScale = gravityScale;
        if (!doingSmash && !doingRoll && !playerController.isCannabis) direction = new Vector2(1.2f, 1);
        else if (playerController.isCannabis) direction = new Vector2(1.0f, 1);
        else if (doingSmash) direction = new Vector2(smashVelocity, 0);
        else if (doingRoll && isGrounded) direction = new Vector2(2.5f, 0);
        if (anim.GetBool("SlowFall")) direction = new Vector2(slowFallGravity, .75f);

        if (!isDiying)
        {

            if (!isHitBadFloor)
            {
                if (!isPortalEnter) Walk();
            }
            else
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Roll", false);
                anim.SetBool("SlowFall", false);
                anim.SetBool("Flappy", false);

                anim.SetBool("HitBadFloor", true);

            }
            if (isGrounded && !isHitBadFloor)
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Jump", false);
                anim.SetBool("Flappy", false);

            }
            else if (!isGrounded && isHitBadFloor)
            {
                anim.SetBool("Flappy", false);

                anim.SetBool("Walk", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Roll", false);
                anim.SetBool("SlowFall", false);
                anim.SetBool("HitBadFloor", true);


            }
            else if (!isGrounded && !isHitBadFloor)
            {
                anim.SetBool("Jump", true);
            }
        }


        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }


        if (isPC)
        {
            if (Time.timeScale == 0) return;
            if (swipeDetector.isJumping && inputsEnabled)
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }


            if (jumpBufferCounter > 0 && !playerController.isCocaMetaHero)
            {

                if (coyoteTimeCounter > 0)
                {
                    anim.SetBool("Jump", true);
                    canDoubleJump = true;

                    if (!playerController.isCannabis && !playerController.isAlcohol)
                    {

                        StartCoroutine(Jump(0));
                        jumpBufferCounter = 0;

                    }
                    else
                    {
                        canSmash = false;
                        canDoubleJump = false;
                        StartCoroutine(Jump(.25f));
                        jumpBufferCounter = 0;
                    }

                    if (playerController.isAlcohol && !doingRoll)
                    {
                        canDoubleJump = false;
                        StartCoroutine(Roll(0, -1, 0));
                    }
                }
                else if (playerController.saltoDoble)
                {
                    if (jumpBufferCounter > 0 && !playerController.isCocaMetaHero)
                    {
                        if (canDoubleJump)
                        {
                            StartCoroutine(Jump(0));
                            canDoubleJump = false;

                        }
                    }
                }

            }
            if (Input.GetMouseButtonUp(0)) coyoteTimeCounter = 0;

            if (playerController.isCocaMetaHero)
            {
                if (isGrounded)
                {
                    anim.SetBool("Jump", true);
                    StartCoroutine(Jump(0));
                }
            }

            if (playerController.isPsilo)
            {
                Camera.main.ResetWorldToCameraMatrix();
                Camera.main.ResetProjectionMatrix();
                Camera.main.projectionMatrix = Camera.main.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
            }
            else
            {
                Camera.main.ResetWorldToCameraMatrix();
                Camera.main.ResetProjectionMatrix();
            }


            if (playerController.paracaidas)
            {
                if (!isGrounded)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        anim.SetBool("Walk", false);
                        anim.SetBool("SlowFall", true);
                        rb.velocity = new Vector2(.2f, rb.velocity.y);
                        rb.gravityScale = slowFallGravity;
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        anim.SetBool("SlowFall", false);
                        anim.SetBool("Walk", true);
                        rb.gravityScale = gravityScale;
                    }
                }
                else
                {
                    anim.SetBool("SlowFall", false);
                    anim.SetBool("Walk", true);
                    rb.gravityScale = gravityScale;
                }
            }

            if (Input.GetMouseButtonDown(1) && !doingRoll && inputsEnabled)
            {
                if (!playerController.isCannabis)
                {
                    if (!playerController.isAlcohol)
                    {
                        StartCoroutine(Roll(0, -1, 0));
                    }
                    else
                    {
                        if (isGrounded)
                        {
                            anim.SetBool("Jump", true);
                            canDoubleJump = false;
                            StartCoroutine(Jump(0));
                        }
                    }
                }
                else
                {
                    StartCoroutine(Roll(0, -1, .25f));

                }

            }

            if (canSmash && Input.GetKeyDown(KeyCode.LeftShift) && !doingSmash)
            {
                Smash(1, 0);
            }

            if (isGrounded && !tapFloor)
            {
                TapFloor();
                tapFloor = true;
            }
            if (!isGrounded && tapFloor)
            {
                tapFloor = false;
            }

            float velocity;
            if (rb.velocity.y > 0) velocity = 1;
            else velocity = -1;
            if (!isGrounded)
            {
                anim.SetFloat("VerticalVelocity", velocity);
            }
            else
            {
                if (velocity == -1)
                {
                    EndJump();
                }
            }
        }


        else if (!isPC)
        {
            if (Time.timeScale == 0) return;
            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1 && inputsEnabled)
                {
                    if (!swipeDetector.btnPause.enabled) return;
                    touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        jumpBufferCounter = jumpBufferTime;
                        tapDetected = true;
                        tapStartTime = Time.time;
                        tapStartPos = touch.position;

                        if (tapCoroutine != null)
                        {
                            StopCoroutine(tapCoroutine);
                        }
                        tapCoroutine = StartCoroutine(HandleTap());
                    }
                    else
                    {
                        jumpBufferCounter -= Time.deltaTime;
                    }

                    if ( swipeDetector.swipeDirection == SwipeDirection.Down && inputsEnabled)
                    {
                        Vector2 localPoint = touch.position;
                        Vector2 deltaPoint = touch.deltaPosition * 10;

                        if (Vector2.Distance(localPoint, deltaPoint) > swipeDistanceThreshold)
                        {
                            tapDetected = false; // Cancelar el tap
                            HandleSwipe();
                        }
                    }

                }

                //else if (Input.touchCount == 2)
                //{
                //    Touch touch0 = Input.GetTouch(0);
                //    Touch touch1 = Input.GetTouch(1);
                //    if (!isGrounded)
                //    {

                //        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                //        {
                //            twoFingerTapDetected = true;
                //        }
                //        else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
                //        {
                //            twoFingerTapDetected = false;
                //            // ExecuteTwoFingerTapEnd();
                //            anim.SetBool("SlowFall", false);
                //            anim.SetBool("Walk", true);
                //            rb.gravityScale = gravityScale;
                //        }

                //        if (twoFingerTapDetected && (touch0.phase == TouchPhase.Stationary || touch0.phase == TouchPhase.Moved) && (touch1.phase == TouchPhase.Stationary || touch1.phase == TouchPhase.Moved))
                //        {
                //            // ExecuteTwoFingerTap();
                //            if (playerController.paracaidas)
                //            {
                //                if (!isGrounded)
                //                {
                //                    anim.SetBool("Walk", false);
                //                    anim.SetBool("SlowFall", true);
                //                    //rb.velocity = new Vector2(.2f, rb.velocity.y);
                //                    rb.gravityScale = slowFallGravity;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        anim.SetBool("SlowFall", false);
                //        anim.SetBool("Walk", true);
                //        rb.gravityScale = gravityScale;
                //    }
                //}
                //else
                //{
                //    twoFingerTapDetected = false;
                //}
            }

            if (playerController.isCocaMetaHero)
            {
                if (isGrounded)
                {
                    anim.SetBool("Jump", true);
                    StartCoroutine(Jump(0));
                }
            }

            if (playerController.isPsilo)
            {
                Camera.main.ResetWorldToCameraMatrix();
                Camera.main.ResetProjectionMatrix();
                Camera.main.projectionMatrix = Camera.main.projectionMatrix * Matrix4x4.Scale(new Vector3(1, -1, 1));
            }
            else
            {
                Camera.main.ResetWorldToCameraMatrix();
                Camera.main.ResetProjectionMatrix();
            }

            if (isGrounded && !tapFloor)
            {
                TapFloor();
                tapFloor = true;
            }
            if (!isGrounded && tapFloor)
            {
                tapFloor = false;
            }
            float velocity;
            if (rb.velocity.y > 0) velocity = 1;
            else velocity = -1;
            if (!isGrounded)
            {
                anim.SetFloat("VerticalVelocity", velocity);
            }
            else
            {
                if (velocity == -1)
                {
                    EndJump();
                }
            }
        }
    }
    #endregion
    #region RunnerMethods
    public void DoJump(float delay)
    {
        if (coyoteTimeCounter > 0)
        {
            StartCoroutine(Jump(0));
        }
        else if (playerController.saltoDoble && canDoubleJump)
        {
            StartCoroutine(Jump(0));
            canDoubleJump = false;
        }
    }

    // Modificación en Jump para resetear el estado de roll inmediatamente
    private IEnumerator Jump(float delay)
    {
        if (!playerController.isCannabis)
        {
            PlayJumpSound();
            yield return new WaitForSeconds(delay);

            // Reseteo inmediato para permitir el roll en cualquier momento
            doingRoll = false;

            StartCoroutine(SwitchCapsuleColliderSize());
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpStrength;
        }
        else
        {
            yield return new WaitForSeconds(delay);
            PlayJumpSound();

            anim.SetBool("Walk", false);
            anim.SetBool("Jump", true);

            // Reseteo inmediato para permitir el roll en cualquier momento
            doingRoll = false;

            StartCoroutine(SwitchCapsuleColliderSize());
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpStrength;
        }
    }
    private void PlayJumpSound()
    {
        if (swipeDetector.playJumpSound)
        {
            if (movementMode == MovementMode.RunnerMode && !isHitBadFloor && inputsEnabled && canMove) AudioManager.Instance.PlaySfx("Jump");
            swipeDetector.playJumpSound = false;
        }
    }
    public void EndJump()
    {
        anim.SetBool("Jump", false);
    }
    private void TapFloor()
    {
        anim.SetBool("Jump", false);
        AudioManager.Instance.PlaySfx("Fall");
    }
    public void DoRoll()
    {
        StartCoroutine(Roll(0, -1, 0));
    }

    private IEnumerator Roll(float x, float y, float time)
    {
        // Eliminar la condición para permitir el roll incluso si doingRoll es true
        // Esto asegura que se pueda hacer roll sin restricciones de estado
        // Eliminamos la condición: `if (!doingRoll)`
        doingRoll = true;
        anim.SetBool("Roll", true);

        rbVelocityTemp = rb.velocity;
        rb.velocity = new Vector2(x, y).normalized * rollVelocity;

        StartCoroutine(SwitchCapsuleColliderSize());
        StartCoroutine(PreRoll());

        yield return new WaitForSeconds(0.3f); // Duración del roll
        doingRoll = false;
        EndRoll();
    }

    private IEnumerator PreRoll()
    {
        StartCoroutine(FloorRoll());
        //rb.gravityScale = 0;
        doingRoll = true;

        yield return new WaitForSeconds(0.3f);
        //rb.gravityScale = gravityScale;
        doingRoll = false;
        EndRoll();
    }

    public void EndRoll()
    {
        anim.SetBool("Roll", false);
        canRoll = true; // Permitir que se pueda hacer roll nuevamente
    }
    private IEnumerator FloorRoll()
    {
        yield return new WaitForSeconds(0.15f);
        if (isGrounded)
       // if (coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            canRoll = false;
        }
    }
    private IEnumerator SwitchCapsuleColliderSize()
    {
        yield return new WaitForSeconds(.1f);
        // capsuleCollider.size = capsuleColliderSize * new Vector2(1, 0.05f);
        capsuleCollider.size = new Vector2(.79f, 0.0001f);
        //if (doingRoll) capsuleCollider.offset = new Vector2(0,-.14f);
        yield return new WaitForSeconds(.3f);
        capsuleCollider.size = capsuleColliderSize;
        capsuleCollider.offset = capsuleColliderOffset;
        //rb.velocity = rbVelocityTemp;

    }
    private void Smash(float x, float y)
    {
        ghostController.enabled = true;
        // spriteRenderer.color = Color.red;
        anim.SetBool("Smash", true);
        canSmash = true;
        rb.velocity = Vector2.zero;
        StartCoroutine(PreSmash());
    }
    private IEnumerator PreSmash()
    {
        StartCoroutine(FloorSmash());
        rb.gravityScale = 0;
        doingSmash = true;

        yield return new WaitForSeconds(.3f);
        rb.gravityScale = gravityScale;
        doingSmash = false;
        EndSmash();
    }
    private IEnumerator FloorSmash()
    {
        yield return new WaitForSeconds(0.15f);
        if (isGrounded)
        {
            canSmash = false;
        }
    }
    public void EndSmash()
    {
        //UserData.health = 0;
        gameObject.GetComponent<PlayerController>().SaludAmount = 0;
        gameObject.GetComponent<PlayerController>().uiSalud.saludCount = 0;
        gameObject.GetComponent<PlayerController>().uiSalud.UpdateSalud(0);
        //spriteRenderer.color = Color.white;
        anim.SetBool("Smash", false);
        ghostController.enabled = false;
    }
    public void StartCameraShake(float tiempo)
    {
        if (cameraShake != null)
        {
            StopCoroutine(cameraShake);
        }
        cameraShake = StartCoroutine(CameraShake(tiempo));
    }
    public void StartHearthBeathCameraShake(float tiempo)
    {
        if (heartbeatShakeSequence != null)
        {
            StopCoroutine(heartbeatShakeSequence);
        }
        heartbeatShakeSequence = StartCoroutine(CameraShake(tiempo));
    }
    private IEnumerator CameraShake(float tiempo)
    {
        //doingShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 15;

        yield return new WaitForSeconds(tiempo);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        //doingShake = false;
    }

    private IEnumerator HeartbeatShakeSequence()
    {
        // Tres latidos
        if (playerController.isDrugged)
        {
            doingShake = true;
            for (int i = 0; i < 3; i++)
            {
                yield return StartCoroutine(CameraShake(.1f)); // Temblor por 1 segundo
                yield return new WaitForSeconds(.3f); // Pausa por 1 segundo
            }
            doingShake = false;
        }
        else
        {
            yield break;
        }
    }
    #endregion
    #region FallingMde
    private void FallingMovement()
    {

        direction = new Vector2(x, 0);

        int movimientosIzquierda = 0;
        int movimientosDerecha = 0;
        int maxMovimientos = 2;

        if (!isGrounded)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Walk", false);
            if (isPC)
            {
                if (inputsEnabled)
                {
                    //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    //{
                    //    if (movimientosIzquierda < maxMovimientos)
                    //    {
                    //        foreach (FallingLevelColliders collider in fallingColliders)
                    //        {
                    //            if (!collider.isFallColliding)
                    //            {
                    //                Mover(Vector3.left + new Vector3(0, -.25f, 0));
                    //            }
                    //        }
                    //        movimientosIzquierda++;
                    //        movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                    //    }
                    //}
                    //if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    //{

                    //    if (movimientosDerecha < maxMovimientos)
                    //    {
                    //        foreach (FallingLevelColliders collider in fallingColliders)
                    //        {
                    //            if (!collider.isFallColliding)
                    //            {
                    //                Mover(Vector3.right + new Vector3(0, -.25f, 0));
                    //            }
                    //        }                            
                    //        movimientosDerecha++;
                    //        movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                    //    }
                    //}

                    if (Input.GetMouseButtonDown(0))
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        Mover(Vector3.left + new Vector3(0, -.25f, 0));
                        movimientosIzquierda++;
                        movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        Mover(Vector3.right + new Vector3(0, -.25f, 0));
                        movimientosDerecha++;
                        movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                    }
                }
            }
            else if (!isPC)
            {
                if (inputsEnabled)
                {
                    float screenWidth = Screen.width;
                    //if (Input.touchCount > 0)
                    //{
                    //    Touch touch = Input.GetTouch(0);

                    //    if (touch.phase == TouchPhase.Began)
                    //    {
                    //        tapDetected = true;
                    //        tapStartTime = Time.time;
                    //        tapStartPos = touch.position;
                    //    }
                    //    // Detectar el final del toque
                    //    else if (touch.phase == TouchPhase.Ended && tapDetected)
                    //    {
                    //        tapDetected = false;
                    //        float tapEndTime = Time.time;
                    //        Vector2 tapEndPos = touch.position;
                    //        float tapDuration = tapEndTime - tapStartTime;
                    //        float swipeDistance = Vector2.Distance(tapStartPos, tapEndPos);

                    //        if (tapDuration < tapTimeThreshold && swipeDistance < swipeDistanceThreshold && !playerController.isCocaMetaHero)
                    //        {
                    //            // Tap
                    //            //Detactar si el usuario hace tap en la izquierda de la pantalla o en la derecha
                    //            if (tapEndPos.x < screenWidth / 2)
                    //            {
                    //                transform.localScale = new Vector3(-1, 1, 1);
                    //                Mover(Vector3.left + new Vector3(0, -.25f, 0));
                    //                movimientosIzquierda++;
                    //                movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                    //            }
                    //            else
                    //            {
                    //                transform.localScale = new Vector3(1, 1, 1);
                    //                Mover(Vector3.right + new Vector3(0, -.25f, 0));
                    //                movimientosDerecha++;
                    //                movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                    //            }
                    //        }
                    //    }
                    //}

                    if (Input.touchCount > 0)
                    {
                        if (Input.touchCount == 1 && inputsEnabled)
                        {
                            touch = Input.GetTouch(0);
                            if (touch.phase == TouchPhase.Began)
                            {
                                tapDetected = true;
                                tapStartTime = Time.time;
                                tapStartPos = touch.position;

                                // esto es tap
                                if (touch.position.x < screenWidth / 2)
                                {
                                    transform.localScale = new Vector3(-1, 1, 1);
                                    Mover(Vector3.left + new Vector3(0, -.25f, 0));
                                    movimientosIzquierda++;
                                    movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                                }
                                else
                                {
                                    transform.localScale = new Vector3(1, 1, 1);
                                    Mover(Vector3.right + new Vector3(0, -.25f, 0));
                                    movimientosDerecha++;
                                    movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                movementMode = MovementMode.RunnerMode;
            }
        }
    }
    public void DoMove()
    {
        int movimientosIzquierda = 0;
        int movimientosDerecha = 0;
        int maxMovimientos = 2;

        if (isPC)
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Mover(Vector3.left + new Vector3(0, -.25f, 0));
                movimientosIzquierda++;
                movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                transform.localScale = new Vector3(1, 1, 1);
                Mover(Vector3.right + new Vector3(0, -.25f, 0));
                movimientosDerecha++;
                movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
            }
        }
        else
        {
            float screenWidth = Screen.width;
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    tapDetected = true;
                    tapStartTime = Time.time;
                    tapStartPos = touch.position;
                }
                // Detectar el final del toque
                else if (touch.phase == TouchPhase.Ended && tapDetected)
                {
                    tapDetected = false;
                    float tapEndTime = Time.time;
                    Vector2 tapEndPos = touch.position;
                    float tapDuration = tapEndTime - tapStartTime;
                    float swipeDistance = Vector2.Distance(tapStartPos, tapEndPos);

                    if (tapDuration < tapTimeThreshold && swipeDistance < swipeDistanceThreshold && !playerController.isCocaMetaHero)
                    {
                        // Tap
                        //Detactar si el usuario hace tap en la izquierda de la pantalla o en la derecha
                        if (tapEndPos.x < screenWidth / 2)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                            Mover(Vector3.left + new Vector3(0, -.25f, 0));
                            movimientosIzquierda++;
                            movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                        }
                        else
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                            Mover(Vector3.right + new Vector3(0, -.25f, 0));
                            movimientosDerecha++;
                            movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                        }
                    }
                }
            }
        }
    }
    private void Mover(Vector2 direccion)
    {

        //Vector3 nuevaPosicion = transform.position + new Vector3(direccion.x, direccion.y,0).normalized * fallingModeMovementAmmount;
        Vector3 nuevaPosicion = new Vector3(rb.position.x, rb.position.y, 0) + new Vector3(direccion.x, direccion.y, 0).normalized * fallingModeMovementAmmount;
        rb.DOMove(nuevaPosicion, .1f);
    }
    #endregion
    #region FlappyMode
    private void FlappyMovement()
    {
        //rb.gravityScale = gravityScale;
        //rb.gravityScale = 4f;
        //print("Gravity "+rb.gravityScale);
        //if (!playerController.isCannabis)
        //{
        //    rb.gravityScale = 4f;
        //    jumpFlappyStrength = 10;
        //}
        //else
        //{
        //    rb.gravityScale = 1f;
        //    jumpFlappyStrength = 6;

        //}
        direction = new Vector2(.75f, 1);
        Walk();
        print(rb.gravityScale);
        anim.SetBool("Flappy", true);
        if (isPC)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(FlapptyJump(0));

                //if (!playerController.isCannabis)
                //{
                //    StartCoroutine(FlapptyJump(0));
                //}
                //else
                //{
                //    StartCoroutine(FlapptyJump(0.25f));

                //}
            }
        }
        else if (!isPC)
        {
            //if (Input.touchCount > 0)
            //{
            //    Touch touch = Input.GetTouch(0);

            //    if (touch.phase == TouchPhase.Began)
            //    {
            //        tapDetected = true;
            //        tapStartTime = Time.time;
            //        tapStartPos = touch.position;
            //    }
            //    // Detectar el final del toque
            //    else if (touch.phase == TouchPhase.Ended && tapDetected)
            //    {
            //        tapDetected = false;
            //        float tapEndTime = Time.time;
            //        Vector2 tapEndPos = touch.position;
            //        float tapDuration = tapEndTime - tapStartTime;
            //        float swipeDistance = Vector2.Distance(tapStartPos, tapEndPos);

            //        if (tapDuration < tapTimeThreshold && swipeDistance < swipeDistanceThreshold)
            //        {
            //            // Esto es un tap
            //            StartCoroutine(FlapptyJump(0));

            //            //if (!playerController.isCannabis)
            //            //{
            //            //    StartCoroutine(FlapptyJump(0));
            //            //}
            //            //else
            //            //{
            //            //    StartCoroutine(FlapptyJump(0.25f));

            //            //}

            //        }
            //        else
            //        {
            //            // Esto es un swipe
            //            // HandleSwipe(tapStartPos, tapEndPos);

            //        }
            //    }
            //}

            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1 && inputsEnabled)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        tapDetected = true;
                        tapStartTime = Time.time;
                        tapStartPos = touch.position;

                        // esto es tap
                        StartCoroutine(FlapptyJump(0));
                    }                  

                }
            }
            }
    }
    public void DoFlappy()
    {
        StartCoroutine(FlapptyJump(0));
    }
    private IEnumerator FlapptyJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpFlappyStrength;
    }
    #endregion
    #region Die
    public void Die()
    {
        isDiying = true;

        AudioManager.Instance.PlaySfx("Die");
        AudioManager.Instance.PlayMusic("Die_Panel",0);


        rb.bodyType = RigidbodyType2D.Static;
        isMoving = false; // Detener el movimiento
        anim.SetBool("SlowWalk", false); // Desactivar animación de caminar
        anim.SetBool("Walk", false); // Desactivar animación de caminar


        StartCoroutine(Diying());
        //if (!canMove)
        //{

        //    StartCoroutine(Diying());
        //}
    }
    private IEnumerator Diying()
    {

        GetComponent<GhostController>().enabled = false;
        //float startValue = material.GetFloat("_DissolveAmmount");
        // anim.Play("Die");
        // material.SetFloat("_DissolveAmmount", Mathf.Lerp(0, 1, Time.deltaTime * .5f));
        //StartCoroutine(PlayerDisolve());
        // Buscar todos los objetos activos con el nombre "Ghost(Clone)"
        GameObject[] objetosGhost = GameObject.FindGameObjectsWithTag("Ghost");

        // Iterar sobre los objetos encontrados y desactivarlos
        foreach (GameObject ghost in objetosGhost)
        {
            ghost.SetActive(false);
        }
        //StartCoroutine(DieAnim2());
        direction = Vector2.zero;
        playerController.isDie = true;
        playerController.escudo = false;
        playerController.saltoDoble = false;
        playerController.vidaExtra = false;
        playerController.paracaidas = false;
        //PlayerDisolve();

        //UserData.piezaA_N1 = false;
        //UserData.piezaB_N1 = false;
        //UserData.piezaC_N1 = false;
        //UserData.piezaD_N1 = false;   

        //UserData.piezaA_N2 = false;
        //UserData.piezaB_N2 = false;
        //UserData.piezaC_N2 = false;
        //UserData.piezaD_N2 = false;

        //UserData.piezaA_N3 = false;
        //UserData.piezaB_N3 = false;
        //UserData.piezaC_N3 = false;
        //UserData.piezaD_N3 = false;


        yield return new WaitForSeconds(.2f);
        //levelManager.ResetLevel();
        levelManager.GameOver();
        //canMove = false;
        //SceneManager.LoadScene("Test");

    }
    private IEnumerator PlayerDisolve()
    {
        AudioManager.Instance.PlaySfx("Dissolve");

        float dissolveAmount = 0;
        float duration = .5f;  // Duración total de la animación en segundos
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            dissolveAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            elapsedTime += Time.deltaTime;
            print(material.GetFloat("_DissolveAmmount"));
            yield return null;  // Esperar al siguiente frame
        }

        // Asegurarse de que el valor final sea exactamente 1
        material.SetFloat("_DissolveAmmount", 1);
    }
    public void DieMaterialAnim()
    {
        Time.timeScale = .5f;
        rb.simulated = false;
        StartCoroutine(DieAnim2());
    }
    private IEnumerator DieAnimation()
    {
        dissolveAmount += .01f;
        material.SetFloat("_DissolveAmmount", dissolveAmount);
        yield return new WaitWhile(() => material.GetFloat("_DissolveAmmount") < 1);
        material.SetFloat("_DissolveAmmount", 1);
        print("simon");

    }
    private IEnumerator DieAnim2()
    {
        dissolveAmount = 0;
        while (dissolveAmount < 1)
        {
            dissolveAmount += .1f;
            material.SetFloat("_DissolveAmmount", dissolveAmount);
            print(material.GetFloat("_DissolveAmmount"));

            yield return new WaitForSeconds(.05f);
            // yield return null;
        }
        // Die();
    }
    #endregion
}

