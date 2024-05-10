using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 direction;

    [Header("Statistics")]
    public float velocity = 10;
    public float jumpStrength = 5;
    
    [Header("Collisions")]
    public Vector2 down;    
    public float collisionRatio;
    public LayerMask layerFloor;
    
    [Header("Bools")]
    public bool canMove;
    public bool isGrounded;
    private bool canDoubleJump;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {

        Movement();
        CheckGround();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

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
        if(canMove) 
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
