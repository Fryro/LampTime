using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;

    private bool facingRight = true;

    public float vertSpeed = 2.0f;
    public float horizSpeed = 2.0f;

    public float horizMovement;
    public float vertMovement;


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
        horizMovement = Input.GetAxisRaw("Horizontal");
        vertMovement = Input.GetAxisRaw("Vertical");
    }

    // Handles running the physics
    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(horizMovement * horizSpeed, vertMovement * vertSpeed);
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
}
