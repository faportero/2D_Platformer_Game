using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator anim;
    private Vector2 direction;
    private CinemachineVirtualCamera cm;
    private SpriteRenderer spriteRenderer;

    [Header("Statistics")]
    public float velocity = 10;
    public float jumpStrength = 5;
    public float rollVelocity = 20;
    public float smashVelocity = 20;
    public float gravityScale = 10;
    public float slowFallGravity = 2;
    private Vector2 capsuleColliderSize;
    private Vector2 targetPosition;

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
    private float targetPositionaux;

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
    }

    void Update()
    {
        //if (!canSmash)
        //{
        //    Movement();
        //}
        Movement();
        CheckGround();
      // if(Mathf.Abs(rb.velocity.x) > 10f)  print(rb.velocity);


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
        //rb.velocity += new Vector2(x, y).normalized * smashVelocity;
        Vector2 m_NewForce = new Vector2(smashVelocity, .0f);
        rb.AddForce(m_NewForce, ForceMode2D.Impulse);
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



    private IEnumerator MovetoTarget()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var viewportPoint = Camera.main.ViewportToWorldPoint(targetPosition);
        //targetPositionaux =  transform.position.x - viewportPoint.x;
        targetPositionaux =  transform.localScale.x;
        print(targetPositionaux);
       // rb = false;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
        yield return new WaitForSeconds((.01f));
        targetPosition = Camera.main.ViewportToWorldPoint(viewportPoint);
        //rb.simulated = true;
        yield return new WaitWhile(()=> Vector3.Distance(transform.position, targetPosition)> .05f);
        targetPositionaux = 0;
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrounded) StartCoroutine(MovetoTarget());
            direction = new Vector2(targetPositionaux / targetPositionaux,0).normalized;
        }
        else
        {

        direction = new Vector2(x, y);
        }


        Walk();
        //BetterJump();

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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(isGrounded) StartCoroutine(MovetoTarget());
        //}

            if (isGrounded && !tapFloor)
        {
            TapFloor();
            tapFloor = true;
        }

        if(!isGrounded && tapFloor)
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

    private void Walk()
    {   
        if(canMove && !doingRoll) 
        {
            rb.velocity = new Vector2(direction.x * velocity, rb.velocity.y);           

            if (direction != Vector2.zero) 
            {
                if (!isGrounded)
                {
                    
                    anim.SetBool("Jump", true);
                }
                else
                {
                    
                    
                    anim.SetBool("Walk", true);
                    
                }
                if(direction.x < 0 && transform.localScale.x > 0) 
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if(direction.x > 0 && transform.localScale.x < 0) 
                {
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
              
                }
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }
}
