    using System.Collections;
using UnityEngine;
using Cinemachine;
using static SwipeDetector;
using static Enemy;
using System;

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
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private CinemachineVirtualCamera cm;
    private SpriteRenderer spriteRenderer;

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
    public float rollVelocity = 20;
    public float gravityScale = 10;
    public float slowFallGravity = 2;
    public float clickMoveSpeed = 5;
    public float fallingGravity = 1;
    public float fallingVelocity = 20;
    public float smashVelocity = 20;
    public float rotationFallingSpeed = 10;

    [Header("Input Parameters")]
    private Vector2 capsuleColliderSize;
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



    private void Awake()

    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        isPC = InputManager.isPC;
        capsuleColliderSize = capsuleCollider.size;
        targetPosition = transform.position;
    }

    private void Update()
    {
        switch (movementMode)
        {
            case MovementMode.TapMode:
                print("TapMovement");
                TapMovement();
                break;
            case MovementMode.RunnerMode:
                print("RunnerMode");
                RunnerMovement();
                CheckGround();
                break;
            case MovementMode.FallingMode:
                print("FallingMode");
                //if (xRaw != 0 || yRaw != 0)
                //{
                GetInputDirection();
                CheckGround();
                FallingMovement(xRaw, yRaw);
               // }
                break;
            case MovementMode.FlappyMode:
                print("FlappyMode");
                FlappyMovement();
                break;
        }
    }

    private void FixedUpdate()
    {
        if(movementMode == MovementMode.FlappyMode)
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
            rb.velocity = new Vector2(direction.x * velocity,rb.velocity.y);

                GetDirecction();
            if (direction != Vector2.zero)
            {
                //if (!isGrounded)
                //{
                //    anim.SetBool("Jump", true);
                //}
                //else if (isGrounded)
                //{
                //    anim.SetBool("Walk", true);
                //}

            }
            //else
            //{
            //    anim.SetBool("Walk", false);
            //}
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
            StartCoroutine(MovetoTarget());
        }
        else if (!isPC)
        {
            if (swipeDetector.TapPerformed == true)
            {

                screenPosition = Input.mousePosition;
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
            StartCoroutine(MovetoTarget());
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
        //direction = new Vector2(1, 1);
        if (!doingSmash && !doingRoll) direction = new Vector2(1, 1);
        else if (doingSmash) direction = new Vector2(smashVelocity, 0);
        if (anim.GetBool("SlowFall")) direction = new Vector2(slowFallGravity, 0);
        //else if (doingRoll && !isGrounded) rb.velocity += new Vector2(1, yRaw).normalized * rollVelocity;


        Walk();        
        anim.SetBool("Walk", true);
        if (isPC)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {            
                if (isGrounded)
                {
                    anim.SetBool("Jump", true);
                    canDoubleJump = true;
                    Jump();                      
                }
                else
                {                       
                    //anim.SetBool("Jump", false);
                    //anim.SetBool("Walk", true);
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (canDoubleJump)
                        {
                            Jump();
                            canDoubleJump = false;                               

                        }
                    }
                }               

            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && !doingRoll)
            {

                //direction = new Vector2(0, y);
                Roll(0, -1);
               
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && !doingSmash)
            {
                
               
                Smash(1, 0);
                
            }

            if (Input.GetMouseButtonDown(1))
            {
                print("SlowFall");

                if (!isGrounded)
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("SlowFall", true);
                    //direction = new Vector2(.2f, 0);
                    //rb.velocity = Vector2.zero;
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
        
        else if(!isPC)
        {
            if (swipeDetector.TapPerformed == true)
            {
                if (isGrounded)
                {
                    swipeDetector.TapPerformed = false;
                    anim.SetBool("Jump", true);
                    canDoubleJump = true;
                    Jump();
                }
                else
                {
                    //anim.SetBool("Jump", false);
                    //anim.SetBool("Walk", true);
                    if (swipeDetector.TapPerformed == true)
                    {
                        if (canDoubleJump)
                        {
                            swipeDetector.TapPerformed = false;
                            Jump();
                            canDoubleJump = false;

                        }
                    }
                }

            }

            if (swipeDetector.swipeDirection == SwipeDirection.Down && !doingRoll)
                {
                    //direction = new Vector2(0, y);
                    Roll(0, -1);
                swipeDetector.TapPerformed = false;
                swipeDetector.swipeDirection = SwipeDirection.None;

                }

            if (swipeDetector.swipeDirection == SwipeDirection.Right && !doingSmash)
            {
                Smash(1, 0);
                swipeDetector.TapPerformed = false;
                swipeDetector.swipeDirection = SwipeDirection.None;
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
    private IEnumerator SwitchCapsuleColliderSize()
    {
        yield return new WaitForSeconds(.1f);
        capsuleCollider.size = capsuleColliderSize * new Vector2(1, 0.5f);
        yield return new WaitForSeconds(.3f);
        capsuleCollider.size = capsuleColliderSize;
    }

    private void Jump()
    {
        StartCoroutine(SwitchCapsuleColliderSize());
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpStrength;
    }
    public void EndJump()
    {
        anim.SetBool("Jump", false);

    }
    private void TapFloor()
    {
        //canRoll = false;
        //doingRoll = false;
        anim.SetBool("Jump", false);
        //anim.SetBool("Flappy", false);
    }


    private void Roll(float x, float y)
    {
        anim.SetBool("Roll", true);
        // Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        //Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);
        //StartCoroutine(CameraShake());

        canRoll = true;
        //rb.velocity = Vector2.zero;
        //direction = new Vector2(0, y);
        rb.velocity += new Vector2(x, y).normalized * rollVelocity;
        StartCoroutine(SwitchCapsuleColliderSize());
        StartCoroutine(PreRoll());
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
       // rb.velocity += new Vector2(x, y).normalized * smashVelocity;
        //rb.velocity += new Vector2(smashVelocity, rb.velocity.y);
        StartCoroutine(PreSmash());
    }
    private IEnumerator PreSmash()
    {
        StartCoroutine(FloorSmash());
        rb.gravityScale = 0;
        //rb.velocity = new Vector2(direction.x * smashVelocity, rb.velocity.y);
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
        spriteRenderer.color = Color.white;
        anim.SetBool("Smash", false);
    }

    public void Die()
    {
        if (!canMove)
        {
            StartCoroutine(Diying());
            print("Murio");

        }
    }
    private IEnumerator Diying()
    {

        anim.Play("Die");
        yield return new WaitForSeconds(3);
    }

    private void FallingMovement(float x, float y)
    {
        if(isPC)
        {
            if (!isGrounded)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("Walk", false);
                direction = new Vector2(0, y);
                //rb.velocity = new Vector2(0,-5);
                //rb.gravityScale = 0;                
                rb.gravityScale = fallingGravity;        
                rb.velocity += new Vector2(x, y).normalized * fallingVelocity;

                if (x < 0 && transform.localScale.x > 0)
                {
                    //xRaw = 1;
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (x > 0 && transform.localScale.x < 0)
                {
                    //xRaw = -1;
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }

            }
            else if (isGrounded) 
            {                
               // print("waaaaaaaa");
                movementMode = MovementMode.RunnerMode;
            }
        }
        else if (!isPC)
        {
            if (!isGrounded)
            {
                screenPosition = Input.mousePosition;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                float clicDirection = targetPosition.x;
                clicDirection = clicDirection - transform.position.x;

                anim.SetBool("Jump", true);
                anim.SetBool("Walk", false);
                //rb.velocity = rb.velocity / 2;
                direction = new Vector2(0, y);
                rb.gravityScale = fallingGravity;
                rb.velocity += new Vector2(clicDirection, y).normalized * fallingVelocity;

                //swipeDetector.TapPerformed = true;
                if (swipeDetector.TapPerformed == true)
                {                    
                   // print("screenPosAux = " + clicDirection);

                    if (clicDirection < 0 && transform.localScale.x > 0)
                    {
                        // xRaw = 1;
                        
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    }
                    else if (clicDirection > 0 && transform.localScale.x < 0)
                    {
                        // xRaw = -1;
                        //clicDirection = 1;
                        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    }

                    

                print(clicDirection);
                }
            }
            else if (isGrounded)
            {
                movementMode = MovementMode.RunnerMode;
            }
        }
    }

    private void FlappyMovement()
    {
        rb.gravityScale = gravityScale;
        direction = new Vector2(1, 1);
        Walk();
        
        //anim.SetBool("Walk", false);
        anim.SetBool("Flappy", true);
        if (isPC)
        {      
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity += Vector2.up * jumpStrength;
                
            }
        }
    }
}