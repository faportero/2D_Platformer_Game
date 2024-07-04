using System.Collections;
using UnityEngine;
using Cinemachine;
using static SwipeDetector;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerMovementNew : MonoBehaviour
{
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
    private CapsuleCollider2D capsuleCollider;
    [HideInInspector] public Animator anim;
    private CinemachineVirtualCamera cm;
    private SpriteRenderer spriteRenderer;
   // private PlayerController playerController;
    private PlayerControllerNew playerController;
    private GhostController ghostController;
    private CameraFollowObject cameraFollowObject;
    public GameObject cameraFollowGo;
    private Coroutine heartbeatShakeSequence;

    [Header("Level Colisions")]
    [SerializeField] private List<FallingLevelColliders> fallingColliders;
    [SerializeField] private GameObject flappyCollider;
    [SerializeField] private GameObject faillingCollider;
    [SerializeField] private Vector2 down;
    [SerializeField] private float collisionRatio;
    [SerializeField] private LayerMask layerFloor;

    [Header("Input")]
    public SwipeDetector swipeDetector;
    [HideInInspector]public Vector2 direction;
    

    [Header("Movement Parameters")]
    [SerializeField] private float gravityScale;
    [SerializeField] private float velocity = 10;
    [SerializeField] private float jumpStrength = 5;
    [SerializeField] private float jumpFlappyStrength = 5;
    [SerializeField] private float rollVelocity = 20;
    [SerializeField] private float smashVelocity = 20;
    [SerializeField] private float slowFallGravity = 1;
    [SerializeField] private float fallingGravity = 1;
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
    private Vector2 capsuleColliderSize;
    private Vector2 capsuleColliderOffset;
    private Vector3 screenPosition;
    [HideInInspector]public Vector3 targetPosition;
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
    #endregion
    #region Unity Callbacks
    private void Awake()

    {
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

        // StartCoroutine(DieAnimation());

        //print(material.GetFloat("_DissolveAmmount"));
        if (canMove)
        {
            switch (movementMode)
            {
                case MovementMode.TapMode:
                    isFallingMode = false;
                    if (inputsEnabled) 
                    {
                        LerpYDamping();
                        TapMovement();
                    }
                    else
                    {
                        anim.SetBool("SlowWalk", false);
                        anim.Play("Idle");
                    }
                    //print(inputsEnabled);
                    //CheckGround();
                    break;
                case MovementMode.RunnerMode:
                  
                        isFallingMode = false;
                        LerpYDamping();
                        RunnerMovement();
                        CheckGround();
                   
                    break;
                case MovementMode.FallingMode:
                    isFallingMode = true;
                    GetInputDirection();
                    if(inputsEnabled)GetDirecction();
                    //if(inputsEnabled)TurnCheck();
                    CheckGround();
                    FallingMovement();
                    break;
                case MovementMode.FlappyMode:
                    isFallingMode = false;
                    FlappyMovement();
                    break;
            }
        }

    }

    private void FixedUpdate()
    {
        if (movementMode == MovementMode.FlappyMode)
        {

            transform.rotation = Quaternion.Euler(0, 0, rb.velocity.y * .75f);
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
    private  void GetDirecction()
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
            if (swipeDetector.TapPerformed == true)
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
    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            cameraFollowObject.CallTurn();
        }
    }
    private void TurnCheck()
    {
        GetDirecction();
        if (clicDirection > 0 && !isFacingRight)
        {
            Turn();
        }
        else if (clicDirection < 0 && isFacingRight)
        {
            Turn();
        }
    }
    private void TapMovement()
    {        
        rb.gravityScale = gravityScale;
        direction = new Vector2(x, y);
        TurnCheck();           
        StartCoroutine(MovetoTarget());
    }
    private IEnumerator MovetoTarget()
    {
        //anim.SetBool("Walk", false);
        anim.SetBool("SlowWalk", false);
        rb.position = Vector3.MoveTowards(rb.position, targetPosition, clickMoveSpeed * Time.deltaTime);
        yield return new WaitWhile(() => rb.position.x == targetPosition.x);
        anim.SetBool("SlowWalk", true);
    }
    #endregion
    #region RunnerMode
    private void RunnerMovement()
    {
        rb.gravityScale = gravityScale;
        if (!doingSmash && !doingRoll && !playerController.isCannabis) direction = new Vector2(1.2f, 1);
        else if (playerController.isCannabis) direction = new Vector2(.75f, 1);
        else if (doingSmash) direction = new Vector2(smashVelocity, 0);
        else if (doingRoll && isGrounded) direction = new Vector2(2.5f, 0);
        if (anim.GetBool("SlowFall")) direction = new Vector2(slowFallGravity, .75f);
        //if (playerController.paracaidas) direction = new Vector2(slowFallGravity, 0);      


        Walk();
        if (isGrounded) anim.SetBool("Walk", true);
        else anim.SetBool("Jump", true);

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
            if (Input.GetMouseButtonDown(0) && inputsEnabled)
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }


            //if (Input.GetKeyDown(KeyCode.Space) && !playerController.isCocaMetaHero)
            if (jumpBufferCounter > 0 && !playerController.isCocaMetaHero)
            {

                if (coyoteTimeCounter > 0)
                //if (isGrounded)
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
                        StartCoroutine(Jump(.5f));
                        jumpBufferCounter = 0;
                    }

                    if (playerController.isAlcohol && !doingRoll)
                    {
                        canDoubleJump = false;
                        StartCoroutine(Roll(0, -1, 0));
                        //Roll(0, -1);
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
            if (Input.GetKeyUp(KeyCode.Space)) coyoteTimeCounter = 0;

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

            if (playerController.isTabaco && !doingShake)
            {
                // StartCoroutine(CameraShake(1f));
                StartCoroutine(HeartbeatShakeSequence());
                //else StopCoroutine("HeartbeatShakeSequence");
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


           // if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && !doingRoll)
            if (Input.GetMouseButtonDown(1) && !doingRoll && inputsEnabled)
            {
                if (!playerController.isCannabis)
                {
                    if (!playerController.isAlcohol)
                    {
                        //direction = direction * 1.5f;
                        StartCoroutine(Roll(0, -1, 0));
                        //Roll(0, -1);
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
      
            if (Input.touchCount > 0 && !tutorialActive)
            {
                if (Input.touchCount == 1)
                {
                    touch = Input.GetTouch(0);


                    if (touch.phase == TouchPhase.Began)
                    {
                        jumpBufferCounter = jumpBufferTime;
                    }
                    else
                    {
                        jumpBufferCounter -= Time.deltaTime;
                    }


                    if (touch.phase == TouchPhase.Began)
                    {
                        tapDetected = true;
                        tapStartTime = Time.time;
                        tapStartPos = touch.position;


                    }
                    // Detectar el final del toque
                    else if (touch.phase == TouchPhase.Ended && tapDetected)
                    {
                        //coyoteTimeCounter = 0;

                        tapDetected = false;
                        float tapEndTime = Time.time;
                        Vector2 tapEndPos = touch.position;
                        tapDuration = tapEndTime - tapStartTime;
                        float swipeDistance = Vector2.Distance(tapStartPos, tapEndPos);


                        if (jumpBufferCounter > 0 && tapDuration < tapTimeThreshold && swipeDistance < swipeDistanceThreshold && !playerController.isCocaMetaHero)
                        {
                            // Esto es un tap
                            //if (isGrounded)
                            if (coyoteTimeCounter > 0)
                            {
                                swipeDetector.TapPerformed = false;
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
                                    StartCoroutine(Jump(.5f));
                                }

                                if (playerController.isAlcohol && !doingRoll)
                                {
                                    canDoubleJump = false;
                                    StartCoroutine(Roll(0, -1, 0));
                                    //Roll(0, -1);
                                }
                            }

                            else if (playerController.saltoDoble)
                            {
                                if (jumpBufferCounter > 0 && !playerController.isCocaMetaHero)
                                {
                                    if (canDoubleJump)
                                    {
                                        //swipeDetector.TapPerformed = false;
                                        StartCoroutine(Jump(0));
                                        canDoubleJump = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Esto es un swipe

                            if (swipeDetector.swipeDirection == SwipeDirection.Down && !doingRoll)
                            {
                                swipeDetector.TapPerformed = false;
                                if (!playerController.isCannabis)
                                {
                                    if (!playerController.isAlcohol)
                                    {
                                        StartCoroutine(Roll(0, -1, 0));
                                        //Roll(0, -1);
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


                                swipeDetector.swipeDirection = SwipeDirection.None;
                            }

                            if (canSmash && swipeDetector.swipeDirection == SwipeDirection.Right && !doingSmash)
                            {
                                Smash(1, 0);
                                swipeDetector.TapPerformed = false;
                                swipeDetector.swipeDirection = SwipeDirection.None;
                            }
                        }
                        if (!swipeDetector.IsPressing) coyoteTimeCounter = 0;
                    }
                }

                else if (Input.touchCount == 2)
                {
                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);
                    if (!isGrounded)
                    {

                        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                        {
                            twoFingerTapDetected = true;
                        }
                        else if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
                        {
                            twoFingerTapDetected = false;
                            // ExecuteTwoFingerTapEnd();
                            anim.SetBool("SlowFall", false);
                            anim.SetBool("Walk", true);
                            rb.gravityScale = gravityScale;
                        }

                        if (twoFingerTapDetected && (touch0.phase == TouchPhase.Stationary || touch0.phase == TouchPhase.Moved) && (touch1.phase == TouchPhase.Stationary || touch1.phase == TouchPhase.Moved))
                        {
                            // ExecuteTwoFingerTap();
                            if (playerController.paracaidas)
                            {
                                if (!isGrounded)
                                {
                                    anim.SetBool("Walk", false);
                                    anim.SetBool("SlowFall", true);
                                    //rb.velocity = new Vector2(.2f, rb.velocity.y);
                                    rb.gravityScale = slowFallGravity;
                                }
                            }
                        }
                    }
                    else
                    {
                        anim.SetBool("SlowFall", false);
                        anim.SetBool("Walk", true);
                        rb.gravityScale = gravityScale;
                    }
                }
                else
                {
                    twoFingerTapDetected = false;
                }
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

            if (playerController.isTabaco && !doingShake)
            {
                // StartCoroutine(CameraShake(1f));
                StartCoroutine(HeartbeatShakeSequence());
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
        //if (tutorialActive)
        //    return;

        if (coyoteTimeCounter > 0)
        {
            StartCoroutine(Jump(0));  // Iniciar el salto
        }
        else if (playerController.saltoDoble && canDoubleJump)
        {
            StartCoroutine(Jump(0));  // Iniciar el doble salto si está disponible
            canDoubleJump = false;     // Desactivar el doble salto después de usarlo
        }
    }
    private IEnumerator Jump(float delay)
    {
        if (!playerController.isCannabis)
        {

            StartCoroutine(SwitchCapsuleColliderSize());
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpStrength;
        }
        else
        {

            anim.SetBool("Walk", true);
            anim.SetBool("Jump", false);
            yield return new WaitForSeconds(delay);
            anim.SetBool("Walk", false);
            anim.SetBool("Jump", true);
            StartCoroutine(SwitchCapsuleColliderSize());
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpStrength;
            //if(canDoubleJump) canDoubleJump = false;
        }

    }
    public void EndJump()
    {
        anim.SetBool("Jump", false);
    }
    private void TapFloor()
    {
        anim.SetBool("Jump", false);
    }
    public void DoRoll()
    {
        //if (tutorialActive)
        //    return;

        if (playerController.isCannabis)
        {
            StartCoroutine(Roll(0, -1, .25f));  // Iniciar el roll especial si está bajo efecto de cannabis
        }
        else
        {
            StartCoroutine(Roll(0, -1, 0));     // Iniciar el roll estándar
        }

    }
    private IEnumerator Roll(float x, float y, float time)
    {

        if (!playerController.isCannabis)
        {
            anim.SetBool("Roll", true);
            // Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
            //Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);
            //StartCoroutine(CameraShake());
            canRoll = true;
            rb.velocity += new Vector2(x, y).normalized * rollVelocity;
            StartCoroutine(SwitchCapsuleColliderSize());
            StartCoroutine(PreRoll());
        }
        else
        {
            yield return new WaitForSeconds(time);
            anim.SetBool("Roll", true);
            canRoll = true;
            rb.velocity += new Vector2(x, y).normalized * rollVelocity;
            StartCoroutine(SwitchCapsuleColliderSize());
            StartCoroutine(PreRoll());
        }

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
    }
    private IEnumerator FloorRoll()
    {
        yield return new WaitForSeconds(0.15f);
        if (isGrounded)
        {
            canRoll = false;
        }
    }
    private IEnumerator SwitchCapsuleColliderSize()
    {
        yield return new WaitForSeconds(.1f);
       // capsuleCollider.size = capsuleColliderSize * new Vector2(1, 0.05f);
        capsuleCollider.size = new Vector2(.79f, 0.0001f);
        if (doingRoll) capsuleCollider.offset = new Vector2(0,-.34f);
        yield return new WaitForSeconds(.3f);
        capsuleCollider.size = capsuleColliderSize;
        capsuleCollider.offset = capsuleColliderOffset;
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
                    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    {
                        if (movimientosIzquierda < maxMovimientos)
                        {
                            foreach (FallingLevelColliders collider in fallingColliders)
                            {
                                if (!collider.isFallColliding)
                                {
                                    Mover(Vector3.left + new Vector3(0, -.25f, 0));
                                }
                            }
                            movimientosIzquierda++;
                            movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    {

                        if (movimientosDerecha < maxMovimientos)
                        {
                            foreach (FallingLevelColliders collider in fallingColliders)
                            {
                                if (!collider.isFallColliding)
                                {
                                    Mover(Vector3.right + new Vector3(0, -.25f, 0));
                                }
                            }                            
                            movimientosDerecha++;
                            movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                        }
                    }
                }
            }
            else if (!isPC)
            {
                if (inputsEnabled)
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
                                    transform.localScale = new Vector3(-2, 2, 2);
                                    Mover(Vector3.left + new Vector3(0, -.25f, 0));
                                    movimientosIzquierda++;
                                    movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                                }
                                else
                                {
                                    transform.localScale = new Vector3(2, 2, 2);
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
    private void Mover(Vector2 direccion)
    {
        
        //Vector3 nuevaPosicion = transform.position + new Vector3(direccion.x, direccion.y,0).normalized * fallingModeMovementAmmount;
        Vector3 nuevaPosicion = new Vector3(rb.position.x, rb.position.y,0) + new Vector3(direccion.x, direccion.y,0).normalized * fallingModeMovementAmmount;
        rb.DOMove(nuevaPosicion, .1f);
    }
    #endregion
    #region FlappyMode
    private void FlappyMovement()
    {
        rb.gravityScale = gravityScale;
        direction = new Vector2(1, 1);
        Walk();

        anim.SetBool("Flappy", true);
        if (isPC)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity += Vector2.up * jumpFlappyStrength;

            }
        }
        else if (!isPC)
        {
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

                    if (tapDuration < tapTimeThreshold && swipeDistance < swipeDistanceThreshold)
                    {
                        // Esto es un tap
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.velocity += Vector2.up * jumpFlappyStrength;
                    }
                    else
                    {
                        // Esto es un swipe
                        // HandleSwipe(tapStartPos, tapEndPos);

                    }
                }
            }
        }
    }
    #endregion
    #region Die
    public void Die()
    {
        StartCoroutine(Diying());
        //if (!canMove)
        //{

        //    StartCoroutine(Diying());
        //}
    }
    private IEnumerator Diying()
    {
        //float startValue = material.GetFloat("_DissolveAmmount");
       // anim.Play("Die");
       // material.SetFloat("_DissolveAmmount", Mathf.Lerp(0, 1, Time.deltaTime * .5f));
        direction = Vector2.zero;
        playerController.isDie = true;
        playerController.escudo = false;
        playerController.saltoDoble = false;
        playerController.vidaExtra = false;
        playerController.paracaidas = false;
        yield return new WaitForSeconds(1);
        levelManager.GameOver();
        //canMove = false;
        //SceneManager.LoadScene("Test");

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
        while (dissolveAmount<1)
        {
            dissolveAmount += .1f;
            material.SetFloat("_DissolveAmmount", dissolveAmount);
             yield return new WaitForSeconds(.05f);
           // yield return null;
        }
        Die();
    }
    #endregion
}

