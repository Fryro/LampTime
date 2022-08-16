using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    // ==== Basic Movement Floats ==== //
    // Walking
    private float horizSpeed;
    // Jumping - Double Jumping - Wall Jumping
    public float jumpForce;
    private float firstJumpForce;
    private float secondJumpForce;
    private float xWallForce;
    private float yWallForce;
    private float wallJumpTime;
    // Wall Sliding - Wall Climbing
    private float slidingSpeed;
    private float haveBeenSliding;
    private float wallClimbingSpeed;
    private float timeNeededToClimb;

    // ==== Basic Movement Booleans ==== //
    // Walking
    private bool facingRight = true;
    // Jumping - Double Jumping - Wall Jumping
    private bool grounded;
    private bool touchingFront;
    public bool haveDoubleJump;  

    // Wall Sliding - Wall Climbing
    private bool sliding;
    private bool wasSliding;
    private bool wallJumping;

    // ==== References to related objects ==== //
    private Rigidbody2D rb;
    private Animator animator;

    // ==== Unity Specific Jump Conditions ==== //
    [Header("Jump Conditions")]
    [SerializeField] private Transform frontCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    
    // ==== Speed Storage ==== //
    public float xSnapshot;

    // ==== Time Storage ==== //
    public float timeAnchor;
    public float timeNow;

    // ==== Input Storage ==== //
    private float horizInput;
    private float vertInput;
    private bool jumpInput;

    // ==== Movement Values ==== //
    private float horizMovement;
    private float vertMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Setting Variables to Defaults
        facingRight = true;

        horizSpeed = 2.0f;
        firstJumpForce = 3.5f;
        secondJumpForce = 3.0f;
        slidingSpeed = 0.5f;

        haveDoubleJump = false;    
        wallJumping = false;
        xWallForce = 2.0f;
        yWallForce = 2.5f;
        wallJumpTime = 0.20f;

        timeNeededToClimb = 0.75f;
        wallClimbingSpeed = 1.5f;
        haveBeenSliding = 0.0f;
        wasSliding = false;
    }

    // Update is called once per frame
    // Handles input for physics
    void Update()
    {
        // Time Storage
        timeNow = Time.time;

        // Movement Left/Right/Jumping
        horizInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");

        // Looking Up/Down
        vertInput = Input.GetAxisRaw("Vertical");

        // Setting Values to Inputs
        horizMovement = horizInput;
        vertMovement = vertInput;
        
        // Setting other values based on environmental variables
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        touchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        // ==== Wall Movement and Sliding checks ==== //
        // If moving into a wall, and not on the ground...
        if (touchingFront && !grounded)
        {
            sliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
            // If you were already sliding...
            // *Begin storing the time spent sliding.
            if (wasSliding)
            {
                haveBeenSliding += timeNow - timeAnchor;
                timeAnchor = timeNow;
                print(haveBeenSliding);

                //TODO: Animation
                //animator.SetTrigger("sliding");
            }
            // In all other cases...
            else
            {
                wasSliding = true;
                timeAnchor = timeNow;
                haveBeenSliding = 0.0f;

                //TODO: Animation
                //animator.SetTrigger("startsliding");
            }
        }
        // In all other cases...
        else
        {
            sliding = false;
            wasSliding = false;
            timeAnchor = 0.0f;
            haveBeenSliding = 0.0f;
        }

        // Setting haveDoubleJump if grounded or sliding
        // *For cases where a player falls off a platform, but does not jump.
        // *Or when touching a wall.
        if (grounded || sliding)
        {
            haveDoubleJump = true;
        }

        // Vertical Movement
        // If Jump + grounded, do first jump.
        // Else if Jump + have double jump, do second jump.

        // Grounded jump
        if (jumpInput && grounded) 
        {
            jumpForce = firstJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpForce = 0;

            // Animation
            animator.SetTrigger("jump");
        }
        // If already wall jumping....
        else if (wallJumping)
        {
            //rb.velocity = new Vector2(xWallForce * -horizMovement, yWallForce);
            rb.velocity = new Vector2(xSnapshot, yWallForce);
        }
        // Begin a wall jump
        else if (jumpInput && !grounded && sliding)
        {
            wallJumping = true;
            xSnapshot = xWallForce * -horizMovement;
            Invoke("SetWallJumpingFalse", wallJumpTime);

            // TODO: Animation
            // animator.setTrigger("walljump");
        }
        // Double jump
        else if (jumpInput && !grounded && haveDoubleJump && !sliding)
        {
            jumpForce = secondJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpForce = 0;
            haveDoubleJump = false;
            
            // TODO: Animation
            //animator.SetTrigger("doublejump");
        }

        // Sliding Animation Handling

        // Falling Animation Handling
        if (rb.velocity.y < 0)
        {
            animator.SetBool("falling", true);
        }
        else if (rb.velocity.y >= 0)
        {
            animator.SetBool("falling", false);
        }
    }

    // Handles running the physics
    private void FixedUpdate()
    {
        // Horizontal Movement
        // If sliding, have been sliding long enough to climb, and not moving down...
        if (sliding && (haveBeenSliding > timeNeededToClimb) && (vertMovement >= 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, wallClimbingSpeed);

            // TODO: Animation
            //animator.SetTrigger("climbing");
        }
        // If not wall jumping...
        else if (!wallJumping)
        {
            rb.velocity = new Vector2(horizMovement * horizSpeed, rb.velocity.y);
        }

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

    private void SetWallJumpingFalse()
    {
        this.wallJumping = false;
        xSnapshot = 0.0f;
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
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
        Gizmos.DrawSphere(frontCheck.position, checkRadius);
    }
}
