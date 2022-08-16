using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    // CONSTANTS
    private float horizSpeed = 2.0f;
    private float firstJumpForce = 3.5f;
    private float secondJumpForce = 3.0f;

    // References to related objects
    private Rigidbody2D rb2d;
    private Animator animator;

    // Variables used to determine movement/animation changes
    private bool facingRight = true;
    public bool grounded;
    [Header("Jump Conditions")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radOfCircle;
    [SerializeField] private LayerMask whatIsGround;
    
    public bool haveDoubleJump;    

    // Variables used to store Input
    private float horizInput;
    private float vertInput;
    private bool jumpInput;

    // Values that are set based on Input Variables
    private float horizMovement;
    private float vertMovement;

    // Jumping Values
    public float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // Handles input for physics
    void Update()
    {
        // Movement U/L/R
        horizInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        // Looking Up/Down
        vertInput = Input.GetAxisRaw("Vertical");

        // Setting Values to Inputs
        horizMovement = horizInput;
        vertMovement = vertInput;
        
        // Setting other values based on environmental variables
        grounded = Physics2D.OverlapCircle(groundCheck.position, radOfCircle, whatIsGround);



        // Setting haveDoubleJump if grounded
        // *For cases where a player falls off a platform, but does not jump.
        if (grounded)
        {
            haveDoubleJump = true;
        }

        // Vertical Movement
        // If Jump + grounded, do first jump.
        // Else if Jump + have double jump, do second jump.
        if (jumpInput && grounded) 
        {
            jumpForce = firstJumpForce;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpForce = 0;

            // Animation
            animator.SetTrigger("jump");
        }
        else if (jumpInput && !grounded && haveDoubleJump)
        {
            jumpForce = secondJumpForce;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpForce = 0;
            haveDoubleJump = false;
            
            // Animation
            animator.SetTrigger("jump");
        }

        // Falling Animation Handling
        if (rb2d.velocity.y < 0)
        {
            animator.SetBool("falling", true);
        }
        else if (rb2d.velocity.y == 0)
        {
            animator.SetBool("falling", false);
        }
    }

    // Handles running the physics
    private void FixedUpdate()
    {
        // Horizontal Movement
        rb2d.velocity = new Vector2(horizMovement * horizSpeed, rb2d.velocity.y);


        // Animation
        HandleLayers();
        Flip(horizMovement);
        animator.SetFloat("speed", Mathf.Abs(horizMovement));
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && facingRight || horizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    private void HandleLayers()
    {
        if (!grounded)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, radOfCircle);
    }
}
