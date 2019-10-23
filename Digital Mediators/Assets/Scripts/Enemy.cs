using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform Player;
    public float MoveSpeed = 5f;
    public int MaxDist = 10;
    public int MinDist = 5;

    public GameObject basketball;

    private Rigidbody2D m_Rigidbody2D;

    public Animator animator;
    public int health;
    public GameObject deathEffect;
    public int phaseNumber = 1;

    public bool m_FacingRight = false;
    public bool flipped = false;

    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    void Move()
    {
        // If the player is further than a specific distance then follow them at a reduced speed
        // Flips the enemy to face the player
        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {

            if (Player.transform.position.x < transform.position.x)
            {
                m_FacingRight = false;
                m_Rigidbody2D.velocity = new Vector2(-MoveSpeed / 5, 0f);
                if (flipped == true)
                {
                    flipped = false;
                    Flip();
                }
                
            }
            else
            {
                m_FacingRight = true;
                m_Rigidbody2D.velocity = new Vector2(MoveSpeed / 5, 0f);
                if (flipped == false)
                {
                    flipped = true;
                    Flip();
                }
            }
            
            // If the enemy is within a specific distance of the player, it can do specific actions
            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                print("Chasing Player");
                if (Player.transform.position.x < transform.position.x)
                {
                    m_FacingRight = false;
                    m_Rigidbody2D.velocity = new Vector2(-MoveSpeed, 0f);
                    if (flipped == true)
                    {
                        flipped = false;
                        Flip();
                    }
                }
                else
                {
                    m_FacingRight = true;
                    m_Rigidbody2D.velocity = new Vector2(MoveSpeed, 0f);
                    if (flipped == false)
                    {
                        flipped = true;
                        Flip();
                    }
                }
                //Here Call any function U want Like Shoot at here or something
            }

        }
    }

    void AttackAI()
    {
        // Changing the boss' phase depending on their health
        if (health <= 3)
        {
            phaseNumber = 3;
            animator.SetInteger("phaseNumber", phaseNumber);
        }
        else if (health <= 6)
        {
            phaseNumber = 2;
            animator.SetInteger("phaseNumber", phaseNumber);
        }

        // Enemy AI Depending on phase
        if (phaseNumber == 1)
        {

        }

    }

    void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    public void Update()
    {
        Move();
        AttackAI();
    }

}
