using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using System;
using Unity.VisualScripting;
public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private Vector2 direction;
    private CinemachineVirtualCamera cm;

    private SpriteRenderer spriteRenderer;
    public GameObject flappyCollider;
    public GameObject faillingCollider;

    [Header("Statistics")]
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

    private Vector2 capsuleColliderSize;
    private Vector3 screenPosition;
    private Vector3 targetPosition;


    [Header("Collisions")]
    public Vector2 down;    
    public float collisionRatio;
    public LayerMask layerFloor;
    
    [Header("Bools")]
    public bool canMove;
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
        capsuleColliderSize = capsuleCollider.size;
        targetPosition = transform.position;

    }

    void Update()
    {
        //if (!canSmash)
        //{
        //    Movement();
        //}
        Movement();
        CheckGround();

       // rb.MovePosition(targetPosition);
      // if(Mathf.Abs(rb.velocity.x) > 10f)  print(rb.velocity);


    }
    private void FixedUpdate()
    {
        
    }


    private IEnumerator CameraShake()
    {
        doingShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5;

        yield return new WaitForSeconds(0.3f);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        doingShake = false;
    }

    private IEnumerator CameraShake(float tiempo)
    {
        doingShake = true;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 5;

        yield return new WaitForSeconds(tiempo);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        doingShake = false;
    }


    private void Roll(float x, float y) 
    {
        anim.SetBool("Roll",true);
       // Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        //Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);
        //StartCoroutine(CameraShake());

        canRoll = true; 
        //rb.velocity = Vector2.zero;
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
        if(isGrounded)
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
        //Vector3 smashPosition = new Vector3(transform.position.x, transform.position.y + 550,0);
        rb.velocity += new Vector2(x, 0).normalized * smashVelocity;

        //Vector2 m_NewForce = new Vector2(smashVelocity, .0f);
        //rb.AddForce(m_NewForce, ForceMode2D.Impulse);

        //transform.position = Vector3.MoveTowards(transform.position, smashPosition, 200 * Time.deltaTime);
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
        spriteRenderer.color = Color.white  ;
        anim.SetBool("Smash", false);
    }

    private void TapFloor()
    {
        canRoll=false;
        doingRoll=false;
        anim.SetBool("Jump", false);



        anim.SetBool("Flappy", false);
    }

    private void SlowFall()
    {
        if(!isGrounded)
        {
            rb.gravityScale = 3;
        }
        else
        {
            rb.gravityScale = 10;
        }
    }

    private void Failling(float x, float y)
    {

        rb.gravityScale = fallingGravity;
        //  rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(x, y).normalized * fallingVelocity;
        //transform.rotation = Quaternion.Euler(0,0, rb.velocity.y * rotationFallingSpeed);
    }

    private IEnumerator MovetoTarget()
    {
        //rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        anim.SetBool("Walk", false);       
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, clickMoveSpeed * Time.deltaTime);
        yield return new WaitWhile(()=> transform.position.x == targetPosition.x);
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        //rb.constraints = RigidbodyConstraints2D.None;
        anim.SetBool("Walk", true);
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        

        direction = new Vector2(x, y);

        Walk();
        //BetterJump();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!flappyCollider.GetComponent<FlappyCollider>().isFlappy)
            {

                if (isGrounded)
                {
                    anim.SetBool("Jump", true);
                    canDoubleJump = true;
                    Jump();
                }
                else
                {
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
            else
            {
                canDoubleJump = false;
                anim.SetBool("Walk", true);
                anim.SetBool("Jump", false);
                anim.SetBool("Flappy", true);
                rb.gravityScale = 10;
                FlappyJump();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !doingRoll)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Roll(xRaw, yRaw);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !doingSmash)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Smash(xRaw, yRaw);
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

        if (Input.GetMouseButtonDown(1))
        {
            print("SlowFall");

            if (!isGrounded)
            {
                anim.SetBool("SlowFall", true);
                rb.velocity = Vector2.zero;
                rb.gravityScale = slowFallGravity;
            }
           
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("SlowFall", false);
            rb.gravityScale = 10;
        }

        if (mouseWalk)
        {

            if (Input.GetMouseButtonDown(0))
            {

                screenPosition = Input.mousePosition;
                screenPosition.z = Camera.main.nearClipPlane + 25;
                targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                targetPosition.y = transform.position.y;
                targetPosition.z = transform.position.z;

                if (targetPosition.x < 0 && transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (targetPosition.x > 0 && transform.localScale.x < 0)
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                }
            }
            StartCoroutine(MovetoTarget());
        }

        if (faillingCollider.GetComponent<FallingCollider>().isFalling)
        {

            if (xRaw != 0 || yRaw != 0)
            {
                Failling(xRaw, yRaw);
            }else if (isGrounded)
            {
                faillingCollider.GetComponent<FallingCollider>().isFalling = false;
                rb.gravityScale = gravityScale;
            }
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
            if(velocity == -1) EndJump();  
        }
    }

    private void BetterJump() 
    {        
        //if (rb.velocity.y < 0) 
        //{
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        //}
        //else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))  
        //{
        //    rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;

        //}
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(((Vector2)transform.position) + down, collisionRatio, layerFloor);
    }
    private void FlappyJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpStrength;
    }

    private void Jump() 
    {
        StartCoroutine(SwitchCapsuleColliderSize());
        rb.velocity = new Vector2 (rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpStrength;        
    }

    public void EndJump()
    {        
        anim.SetBool("Jump", false);
        
    }
    private IEnumerator SwitchCapsuleColliderSize()
    {
        yield return new WaitForSeconds(.1f);
        capsuleCollider.size = capsuleColliderSize * new Vector2(1,0.5f);
        yield return new WaitForSeconds(.3f);
        capsuleCollider.size = capsuleColliderSize;
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
        if(canMove && !doingRoll && !doingSmash) 
        {
            rb.velocity = new Vector2(direction.x * velocity, rb.velocity.y);           

            if (direction != Vector2.zero) 
            {
                if (!isGrounded && !flappyCollider.GetComponent<FlappyCollider>().isFlappy)
                {
                    
                    anim.SetBool("Jump", true);
                }
                else if (isGrounded && !flappyCollider.GetComponent<FlappyCollider>().isFlappy)
                {
                    anim.SetBool("Walk", true);
                }
                else if (!isGrounded && flappyCollider.GetComponent<FlappyCollider>().isFlappy)
                {
                   // anim.SetBool("Flappy", true);
                    anim.SetBool("Jump", true);
                }

                    GetDirecction();
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }
}
