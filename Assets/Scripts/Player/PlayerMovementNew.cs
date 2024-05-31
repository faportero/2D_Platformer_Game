using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using static SwipeDetector;
using static Enemy;
using System;

using System.Collections.Generic;

using DG.Tweening;

public class PlayerMovementNew : MonoBehaviour
{

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
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private CinemachineVirtualCamera cm;
    private CinemachineCameraOffset camOffset;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    [Header("Level Colisions")]
    public GameObject flappyCollider;
    public GameObject faillingCollider;
    public Vector2 down;
    public float collisionRatio;
    public LayerMask layerFloor;

    [Header("Input")]
    public SwipeDetector swipeDetector;
    private Vector2 direction;
    private Touch theTouch;

    [Header("Movement Parameters")]
    public float velocity = 10;
    public float jumpStrength = 5;
    public float jumpFlappyStrength = 5;
    public float rollVelocity = 20;
    public float gravityScale;
    public float slowFallGravity = 2;
    public float clickMoveSpeed = 5;
    public float fallingGravity = 1;
    public float fallingVelocity = 20;
    public float smashVelocity = 20;
    public float rotationFallingSpeed = 10;
    public float fallingModeMovementAmmount;

    [Header("Input Parameters")]
    private Vector2 capsuleColliderSize;
    private Vector2 capsuleColliderOffset;
    private Vector3 screenPosition;
    private Vector3 targetPosition;
    private float x, y;
    private float xRaw, yRaw;


    [Header("Bools")]
    public bool canMove = true;
    public bool isGrounded;
    private bool canDoubleJump;
    public bool canRoll;
    public bool doingRoll;
    public bool canSmash;
    public bool doingSmash;
    public bool tapFloor;
    public bool doingShake = false;
    public bool doingJump;
    public bool mouseWalk;
    public bool flappyMode;

    public bool isPC;
    private bool canJump = true;
    private bool tapDetected;
    private float tapStartTime;
    private Vector2 tapStartPos;
    private float tapTimeThreshold = .3f;
    private float swipeDistanceThreshold = 150;


    public List<GameObject> faillingTargets;
    [SerializeField] LevelManager levelManager;
    private bool isSlowFalling;

    private void Awake()

    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        //camOffset = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineCameraOffset>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

    }

    void Start()
    {
        isPC = InputManager.isPC;
        capsuleColliderSize = capsuleCollider.size;
        capsuleColliderOffset = capsuleCollider.offset;
        targetPosition = transform.position;



    }

    private IEnumerator MovetoTargetFalling(Vector2 direccion)
    {
        //isFaillingMove = true;
        targetPosition = rb.position + direccion * 10;
        //anim.SetBool("Walk", false);
        rb.position = Vector3.MoveTowards(rb.position, targetPosition, 1 * Time.deltaTime);
        yield return new WaitWhile(() => rb.position.x == targetPosition.x);
        //isFaillingMove = false;
        //anim.SetBool("Walk", true);
    }
    private void Update()
    {
        if(canMove)
        {
            switch (movementMode)
            {
                case MovementMode.TapMode:
                    TapMovement();
                    break;
                case MovementMode.RunnerMode:
                    RunnerMovement();
                    CheckGround();
                    break;
                case MovementMode.FallingMode:
                    GetInputDirection();
                    GetDirecction();
                    CheckGround();
                    FallingMovement();
                    break;
                case MovementMode.FlappyMode:
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

    private void TapMovement()
    {

        rb.gravityScale = gravityScale;
        direction = new Vector2(x, y);
        Walk();
        if (isPC)
        {
            if (Input.GetMouseButtonDown(0))
            {

                screenPosition = Input.mousePosition;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                float clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;

                if (clicDirection < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (clicDirection > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
            StartCoroutine(MovetoTarget());
        }
        else if (!isPC)
        {

            #region old Mobiole TapMovement
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
            //            //screenPosition = Input.mousePosition;
            //            //screenPosition.z = Camera.main.nearClipPlane + 25;
            //            Vector3 screenPosition = new Vector3(tapEndPos.x, tapEndPos.y, Camera.main.nearClipPlane + 25);
            //            targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            //            targetPosition.y = transform.position.y;
            //            targetPosition.z = transform.position.z;

            //            //float clicDirection = targetPosition.x;
            //            float clicDirection = targetPosition.x - transform.position.x;

            //            clicDirection = clicDirection - transform.position.x;
            //            print("screenPosAux = " + clicDirection);

            //            if (clicDirection < 0 && transform.localScale.x > 0)
            //            {
            //                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //            }
            //            else if (clicDirection > 0 && transform.localScale.x < 0)
            //            {
            //                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            //            }
            //            // Iniciar la corrutina de movimiento
            //            StopCoroutine(MovetoTarget()); // Detener la corrutina anterior si está corriendo
            //            StartCoroutine(MovetoTarget());
            //        }
            //    }
            //}

            #endregion


            if (swipeDetector.TapPerformed == true)
            {
                Touch touch = Input.GetTouch(0);
                screenPosition = touch.position;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                float clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;
                print("screenPosAux = " + clicDirection);

                if (clicDirection < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (clicDirection > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
            //StopAllCoroutines();
            StartCoroutine(MovetoTarget());
            swipeDetector.TapPerformed = false;
        }
    }
    private IEnumerator MovetoTarget()
    {
        anim.SetBool("Walk", false);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, clickMoveSpeed * Time.deltaTime);
        yield return new WaitWhile(() => transform.position.x == targetPosition.x);
        anim.SetBool("Walk", true);
    }

    private void RunnerMovement()
    {
        rb.gravityScale = gravityScale;
        if (!doingSmash && !doingRoll && !playerController.isCannabis) direction = new Vector2(1.2f, 1);
        else if (playerController.isCannabis) direction = new Vector2(.75f, 1);
        else if (doingSmash) direction = new Vector2(smashVelocity, 0);
        else if (doingRoll && isGrounded) direction = new Vector2(2.5f, 0);
        if (anim.GetBool("SlowFall")) direction = new Vector2(slowFallGravity, 0);
        //if (playerController.paracaidas) direction = new Vector2(slowFallGravity, 0);      


        Walk();
        if (isGrounded) anim.SetBool("Walk", true);
        else anim.SetBool("Jump", true);
        if (isPC)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !playerController.isCocaMetaHero)
            {
                if (isGrounded)
                {
                    anim.SetBool("Jump", true);
                    canDoubleJump = true;

                    if (!playerController.isCannabis && !playerController.isAlcohol)
                    {
                        StartCoroutine(Jump(0));
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
                    if (Input.GetKeyDown(KeyCode.Space) && !playerController.isCocaMetaHero)
                    {
                        if (canDoubleJump)
                        {
                            StartCoroutine(Jump(0));
                            canDoubleJump = false;
                        }
                    }
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

            if (playerController.paracaidas)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (!isGrounded)
                    {

                        anim.SetBool("Walk", false);
                        anim.SetBool("SlowFall", true);
                        rb.velocity = new Vector2(.2f, rb.velocity.y);
                        rb.gravityScale = slowFallGravity;
                    }

                }
                else if (Input.GetMouseButtonUp(1))
                {
                    anim.SetBool("SlowFall", false);
                    anim.SetBool("Walk", true);
                    rb.gravityScale = 10;
                }
            }


            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && !doingRoll)
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
                        // Esto es un tap

                        if (isGrounded)
                        {
                            swipeDetector.TapPerformed = false;
                            anim.SetBool("Jump", true);
                            canDoubleJump = true;
                            if (!playerController.isCannabis && !playerController.isAlcohol)
                            {
                                StartCoroutine(Jump(0));
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
                            if (!isGrounded && swipeDetector.TapPerformed == true && !playerController.isCocaMetaHero)
                            {
                                if (canDoubleJump)
                                {
                                    swipeDetector.TapPerformed = false;
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

            if (playerController.paracaidas)
            {
                if (!isGrounded && Input.touchCount > 0 && swipeDetector.IsPressing)
                {
                    if (!isSlowFalling)
                    {
                        isSlowFalling = true;
                        anim.SetBool("Walk", false);
                        anim.SetBool("SlowFall", true);
                        rb.velocity = new Vector2(.2f, rb.velocity.y);
                        rb.gravityScale = slowFallGravity;

                    }
                }



                //if (playerController.paracaidas)
                // {                                       
                //     anim.SetBool("Walk", false);
                //     anim.SetBool("SlowFall", true);
                //     rb.velocity = new Vector2(.2f, rb.velocity.y);
                //     rb.gravityScale = slowFallGravity;
                // }
            }

            if (Input.touchCount == 0 && isSlowFalling)
            {

                if (isSlowFalling)
                {
                    isSlowFalling = false;
                    anim.SetBool("SlowFall", false);
                    anim.SetBool("Walk", true);
                    rb.gravityScale = 10;
                }
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
        doingShake = true;
        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(CameraShake(.1f)); // Temblor por 1 segundo
            yield return new WaitForSeconds(.3f); // Pausa por 1 segundo
        }
        doingShake = false;
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
    private IEnumerator FloorRoll()
    {
        yield return new WaitForSeconds(0.15f);
        if (isGrounded)
        {
            canRoll = false;
        }
    }
    public void EndRoll()
    {
        anim.SetBool("Roll", false);
    }

    private void Smash(float x, float y)
    {
        spriteRenderer.color = Color.red;
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
        spriteRenderer.color = Color.white;
        anim.SetBool("Smash", false);
    }

    public void Die()
    {
        if (!canMove)
        {

            StartCoroutine(Diying());
        }
    }
    private IEnumerator Diying()
    {
        anim.Play("Die");
        yield return new WaitForSeconds(1);
        playerController.isDie = true;
        playerController.escudo = false;
        playerController.saltoDoble = false;
        playerController.vidaExtra = false;
        playerController.paracaidas = false;
        canMove = false;
        levelManager.GameOver();
        //SceneManager.LoadScene("Test");

    }

    private IEnumerator FaillingMoveToTarget(float moveAmount)
    {
        //isFaillingMove = true;
        anim.SetBool("Walk", false);
        anim.SetBool("Jump", true);

        Vector3 targetPosition = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
        yield return new WaitWhile(() => transform.position.x == targetPosition.x);
        anim.SetBool("Jump", false);
        //isFaillingMove = false;
    }
    void Mover(Vector2 direccion)
    {

        // Calcula la nueva posición del objeto
        //rb.position = faillingTargets[2].transform.position;
       // Vector2 nuevaPosicion = rb.position + direccion.normalized * fallingModeMovementAmmount;
        Vector3 nuevaPosicion = transform.position + new Vector3(direccion.x, direccion.y,0).normalized * fallingModeMovementAmmount;


        // Mueve el Rigidbody a la nueva posición
       // rb.MovePosition(nuevaPosicion);
        //rb.MovePosition(nuevaPosicion);
       // rb.DOMove(nuevaPosicion, .2f);
        transform.DOMove(nuevaPosicion, .2f);
        //rb.MovePosition(Vector2.MoveTowards(rb.position, nuevaPosicion, 2 * Time.deltaTime));
    }
    private void FallingMovement()
    {
        //camOffset.m_Offset = new Vector3(0, 0, -15);
        direction = new Vector2(x, 0);
        //CinemachineTransposer transposer = cm.GetCinemachineComponent<CinemachineTransposer>();

        //if (transposer != null)
        //{
        //    // Deshabilitar el seguimiento en el eje X
        //    transposer.m_FollowOffset.x = 0f;
        //}

        int movimientosIzquierda = 0;
        int movimientosDerecha = 0;
        int maxMovimientos = 2;

        if (!isGrounded)
        {
            anim.SetBool("Jump", true);
            anim.SetBool("Walk", false);
            if (isPC)
            {

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {

                    if (movimientosIzquierda < maxMovimientos)
                    {
                        Mover(Vector3.left + new Vector3 (0,-.25f,0));
                        movimientosIzquierda++;
                        movimientosDerecha = Mathf.Max(0, movimientosDerecha - 1);
                    }
                }


                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {

                    if (movimientosDerecha < maxMovimientos)
                    {
                        Mover(Vector3.right + new Vector3(0, -.25f, 0));
                        movimientosDerecha++;
                        movimientosIzquierda = Mathf.Max(0, movimientosIzquierda - 1);
                    }
                }

            }
            else if (!isPC)
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
            else
            {

                movementMode = MovementMode.RunnerMode;
            }

        }

        #region oldFailling
        //rb.gravityScale = gravityScale;
        //direction = new Vector2(x, .8f);
        //Walk();
        //if (isPC)
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {

        //        screenPosition = Input.mousePosition;
        //        screenPosition.z = Camera.main.nearClipPlane + 25;
        //        targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //        //targetPosition.y = 20 * Time.deltaTime;
        //        targetPosition.z = transform.position.z;

        //        float clicDirection = targetPosition.x;
        //        clicDirection = clicDirection - transform.position.x;

        //        if (clicDirection < 0 && transform.localScale.x > 0)
        //        {
        //            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //        }
        //        else if (clicDirection > 0 && transform.localScale.x < 0)
        //        {
        //            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //        }
        //    }
        //    StartCoroutine(MovetoTarget());
        //}


        //// rb.gravityScale = 0;
        //direction = new Vector2(xRaw,fallingGravity);
        //Walk();


        //if (Input.GetKeyDown(KeyCode.LeftArrow) && !isFaillingMove)
        //{
        //    Vector2 targetPosition = new Vector2(rb.position.x + 30000, rb.position.y);
        //    rb.position = Vector3.MoveTowards(rb.position, targetPosition, 5 * Time.deltaTime);
        //   // StartCoroutine(FaillingTarget(-200));
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow) && !isFaillingMove)
        //{
        //    //StartCoroutine(FaillingTarget(200));
        //}

        //if (isPC)
        //{
        //    if (!isGrounded)
        //    {


        //    }
        //    else
        //    {
        //        movementMode = MovementMode.RunnerMode;

        //    }
        //}





        //if(isPC)
        //{


        //    if (!isGrounded)
        //    {
        //        anim.SetBool("Jump", true);
        //        anim.SetBool("Walk", false);
        //        direction = new Vector2(0, y);    
        //        rb.gravityScale = fallingGravity;        
        //        rb.velocity += new Vector2(x, y).normalized * fallingVelocity;

        //        if (x < 0 && transform.localScale.x > 0)
        //        {
        //            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //        }
        //        else if (x > 0 && transform.localScale.x < 0)
        //        {                  
        //            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //        }

        //    }
        //    else if (isGrounded) 
        //    {  
        //        movementMode = MovementMode.RunnerMode;
        //    }
        //}
        //else if (!isPC)
        //{
        //    if (!isGrounded)
        //    {
        //        screenPosition = Input.mousePosition;
        //        screenPosition.z = Camera.main.nearClipPlane + 25;
        //        targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //        targetPosition.y = transform.position.y;
        //        targetPosition.z = transform.position.z;

        //        float clicDirection = targetPosition.x;
        //        clicDirection = clicDirection - transform.position.x;

        //        anim.SetBool("Jump", true);
        //        anim.SetBool("Walk", false);;
        //        direction = new Vector2(0, y);
        //        rb.gravityScale = fallingGravity;
        //        rb.velocity += new Vector2(clicDirection, y).normalized * fallingVelocity;

        //        if (swipeDetector.TapPerformed == true)
        //        {     
        //            if (clicDirection < 0 && transform.localScale.x > 0)
        //            {
        //                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        //            }
        //            else if (clicDirection > 0 && transform.localScale.x < 0)
        //            {
        //                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //            }                    

        //        print(clicDirection);
        //        }
        //    }
        //    else if (isGrounded)
        //    {
        //        movementMode = MovementMode.RunnerMode;
        //    }
        //}
        #endregion
    }

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

  
}