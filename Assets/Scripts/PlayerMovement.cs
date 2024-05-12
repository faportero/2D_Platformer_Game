using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direction;
    private CinemachineVirtualCamera cm;

    [Header("Statistics")]
    public float velocity = 10;
    public float jumpStrength = 5;
    public float dashVelocity = 20;

    [Header("Collisions")]
    public Vector2 down;    
    public float collisionRatio;
    public LayerMask layerFloor;
    
    [Header("Bools")]
    public bool canMove;
    public bool isGrounded;
    private bool canDoubleJump;
    public bool canDash;
    public bool doingDash;
    public bool tapFloor;
    public bool doingShake = false;
    
    private void Awake()

    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        
    }

    void Update()
    {

        Movement();
        CheckGround();
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


    private void Dash(float x, float y) 
    {
        anim.SetBool("Dash",true);
        Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        Camera.main.GetComponent<RippleEffect>().Emit(playerPosition);
        StartCoroutine(CameraShake());

        canDash = true; 
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(x, y).normalized * dashVelocity;
        StartCoroutine(PreDash());
    }

    private IEnumerator PreDash()
    {
        StartCoroutine(FloorDash());
        rb.gravityScale = 0;
        doingDash = true;  
        
        yield return new WaitForSeconds(0.3f);
        rb.gravityScale = 3;
        doingDash = false;
        EndDash();
    }

    private IEnumerator FloorDash()
    {
        yield return new WaitForSeconds(0.15f);
        if(isGrounded)
        {
            canDash = false;
        }
    }

    public void EndDash()
    {
        anim.SetBool("Dash", false);
    }

    private void TapFloor()
    {
        canDash=false;
        doingDash=false;
        anim.SetBool("Jump", false);
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        direction = new Vector2(x, y);

        Walk();
        BetterJump();

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
                    if ( canDoubleJump)
                    {   
                        Jump();
                        canDoubleJump= false;   
                        
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && !doingDash)
        {
            if(xRaw !=0 || yRaw !=0)
            {
                Dash(xRaw, yRaw);
            }
        }


        if(isGrounded && !tapFloor)
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

    public void EndJump()
    {        
        anim.SetBool("Jump", false);
    }

    private void BetterJump() 
    {        
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))  
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (2.0f - 1) * Time.deltaTime;

        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(((Vector2)transform.position) + down, collisionRatio, layerFloor);
    }
    private void Jump() 
    {
        rb.velocity = new Vector2 (rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpStrength;
    }
    private void Walk()
    {   
        if(canMove && !doingDash) 
        {
            rb.velocity = new Vector2(direction.x * velocity, rb.velocity.y);
            
            if(direction != Vector2.zero) 
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
