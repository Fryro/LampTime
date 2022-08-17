using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    // ==== Basic Movement Floats ==== //
    // Walking - Running
    private float horizSpeed;
    private float runningSpeed;
    private float slowSpeed;
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
    // Hitstun
    //private float hitstunSpent;
    private float hitstunMax;
    private float hitLaunchX;
    private float hitLaunchY;
    private float xLaunch;

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
    // Hitstun
    private bool hitstun;
    private bool recovering;

    // ==== References to related objects ==== //
    private Rigidbody2D rb;
    private Animator animator;

    // ==== Unity Specific Jump Conditions ==== //
    [Header("Jump Conditions")]
    [SerializeField] private Transform frontCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    
    // ==== Speed and Direction Storage ==== //
    public float xSnapshot;
    public bool facingSnapshot;

    // ==== Time Storage ==== //
    public float timeAnchor;
    public float timeNow;

    // ==== Input Storage ==== //
    private float horizInput;
    private float vertInput;
    private float runningInput;
    private bool jumpInput;

    // ==== Movement Values ==== //
    private float horizMovement;
    private float vertMovement;
    private float runningMovement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Setting Variables to Defaults
        facingRight = true;
        hitstun = false;
        recovering = true;

        hitstunMax = 0.33f;
        hitLaunchX = 3.0f;
        hitLaunchY = 0.5f;
        xLaunch = 0.0f;

        horizSpeed = 2.0f;
        runningSpeed = 3.5f;
        slowSpeed = 1.0f;
        firstJumpForce = 3.5f;
        secondJumpForce = 3.0f;

        haveDoubleJump = false;    
        wallJumping = false;
        xWallForce = 2.0f;
        yWallForce = 2.5f;
        wallJumpTime = 0.20f;

        slidingSpeed = 0.5f;
        timeNeededToClimb = 0.75f;
        wallClimbingSpeed = 1.5f;
        haveBeenSliding = 0.0f;
        wasSliding = false;
    }

    // Update is called once per frame
    // Handles input for physics
    void Update()
    {
        // Set gravity to 1.0
        rb.gravityScale = 1.0f;


        // Time Storage
        timeNow = Time.time;

        // Movement Left/Right/Jumping
        horizInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        runningInput = Input.GetAxisRaw("Run");

        // Looking Up/Down
        vertInput = Input.GetAxisRaw("Vertical");

        // Setting Values to Inputs
        horizMovement = horizInput;
        vertMovement = vertInput;
        runningMovement = runningInput;
        
        // Setting other values based on environmental variables
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        touchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

        // ==== Wall Movement and Sliding checks ==== //
        // If moving into a wall, and not on the ground...
        if (touchingFront && !grounded && Mathf.Abs(horizMovement) > 0)
        {
            sliding = true;
            //rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
            // If you were already sliding...
            // *Begin storing the time spent sliding.
            if (wasSliding)
            {
                haveBeenSliding += timeNow - timeAnchor;
                timeAnchor = timeNow;
                //print(haveBeenSliding);

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
            recovering = false;
        }

        // Vertical Movement
        // If Jump + grounded, do first jump.
        // Else if Jump + have double jump, do second jump.

        // Hitstun Lockout
        if (recovering)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        else if (hitstun)
        {
            // Use snapshotted direction to determine X velocity.
            if (facingSnapshot)
            {
                xLaunch = hitLaunchX * -1;
            }
            else
            { 
                xLaunch = hitLaunchX * 1;
            }

            if (xSnapshot != 0)
            {
                rb.velocity = new Vector2(hitLaunchX * -xSnapshot, hitLaunchY);
            }
            else
            {
                rb.velocity = new Vector2(xLaunch, hitLaunchY);
            }
            Invoke("SetHitstunFalse", hitstunMax);
        }
        // Grounded jump
        else if (jumpInput && grounded) 
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
        // If sliding, have NOT been sliding long enough to climb, and moving horizontally...
        if (hitstun || recovering)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
        else if (sliding && (haveBeenSliding < timeNeededToClimb) && Mathf.Abs(horizMovement) > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -slidingSpeed, float.MaxValue));
        }
        // If sliding, HAVE been sliding long enough to climb, and moving horizontally...
        else if (sliding && (haveBeenSliding >= timeNeededToClimb) && Mathf.Abs(horizMovement) > 0)
        {
            if (vertMovement > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, wallClimbingSpeed);
            }
            else if (vertMovement < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -slidingSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.gravityScale = 0.0f;
                print("Not Moving!");
            }
            // TODO: Animation
            //animator.SetTrigger("climbing");
        }
        // If running or slowwalking...
        else if (runningMovement != 0 && grounded)
        {
            if (runningMovement > 0)
            {
                rb.velocity = new Vector2(horizMovement * runningSpeed, rb.velocity.y);

            }
            else if (runningMovement < 0)
            {
                rb.velocity = new Vector2(horizMovement * slowSpeed, rb.velocity.y);
            }
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

    public void GetHit()
    {
        hitstun = true;
        recovering = false;
        xSnapshot = horizMovement;
        facingSnapshot = facingRight;
    }

    private void SetHitstunFalse()
    {
        this.recovering = true;
        this.hitstun = false;
        this.facingSnapshot = true;
        this.xLaunch = 0.0f;
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
