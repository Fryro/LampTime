using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] protected float speed = 2.0f;
    [SerializeField] protected float direction = 1.0f;

    protected bool facingRight;
    
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer graphics;

    public float maxHealth;
    public float health;
    public float guardMultiplier;
    public bool guarding; 

    public float damage;

    public float iFrames;

    public float timeAnchor;
    public float timeNow;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();
        iFrames = 0.1f;
        timeAnchor = Time.time;
        timeNow = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timeNow = Time.time;
    }


    protected void Move()
    {
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    protected virtual void HandleMovement()
    {
        Move();
    }

    public void TakeDamage(float incoming)
    {
        if (Mathf.Abs(timeNow - timeAnchor) > iFrames)
        {
            if (guarding)
            {
                incoming *= guardMultiplier;
            }
            health -= incoming;
            print("Enemy HP Remaining: " + health + "\t(You did: " + incoming + " damage).");
            timeAnchor = timeNow;  
        }
    }

    protected void TurnAround(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
