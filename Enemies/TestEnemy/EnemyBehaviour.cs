using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Enemy
{
    [SerializeField] private GameObject parent;
    // Start is called before the first frame update
    public override void Start()
    {
        maxHealth = 10.0f;
        health = 10.0f;
        guardMultiplier = 0.2f;
        guarding = false;
        damage = 10.0f;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        graphics = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {

        //graphics.color = new Color (graphics.color.Red, graphics.color.Green, graphics.color.Blue, ((health/maxHealth)));

        HandleMovement();
        timeNow = Time.time;

        if (health <= 0)
        {
            print("Enemy Killed.");
            Destroy(parent);
        }
    }
}