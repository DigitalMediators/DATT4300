using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("General Variables")]
    public Transform Player;
    public float MoveSpeed = 5f;
    public int MaxDist = 10;
    public int MinDist = 5;

    public Slider healthBar;

    private Rigidbody2D m_Rigidbody2D;

    public Animator animator;
    public int health;
    private int baseHealth;
    public GameObject deathEffect;
    public int phaseNumber = 1;

    public bool m_FacingRight = false;
    public bool flipped = false;
    public bool isVulnerable = false;
    private SpriteRenderer spriteRenderer;

    public Image hpBarColor;

    // Phase 1 variables
    [Header("Phase 1")]
    public int ballCount = 0;
    public GameObject goalArea;
    public GameObject basketballs;
    public GameObject middleNoPlatforms;
    public GameObject middlePlatformsPhase2;
    public GameObject middlePlatformsPhase3;


    // Phase 2 variables
    // To have shake on jump, need to change the cinemachine virtual camera settings
    // Body on do nothing, aim on composer, set lookat to player
    // After breaking orbs, makes the boss vulnerable for a certain amount of time, spawns falling rocks
    // After breaking 5 orbs, makes the boss permanently vulnerable but spawns infinite falling rocks
    private bool phase2VariableChange = false;
    private float phase2Speed = 5;
    private int phase2MaxDist = 7;
    public int breakableCount = 0;
    public GameObject Breakable1;
    public GameObject Breakable2;
    public GameObject Breakable3;
    public bool breakableOnField = false;
    public float vulnerabilityTimer = 5f;
    public GameObject rockPrefab;
    private bool spawnedRock = false;

    // Phase 3 variables
    private bool phase3VariableChange = false;
    private float phase3Speed = 1;
    private int phase3MaxDist = 7;


    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        baseHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                m_Rigidbody2D.velocity = new Vector2(-MoveSpeed / 2, 0f);
                if (flipped == true)
                {
                    flipped = false;
                    Flip();
                }
                
            }
            else
            {
                m_FacingRight = true;
                m_Rigidbody2D.velocity = new Vector2(MoveSpeed / 2, 0f);
                if (flipped == false)
                {
                    flipped = true;
                    Flip();
                }
            }
            
            // If the enemy is within a specific distance of the player, it can do specific actions
            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                //print("Chasing Player");
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

        healthBar.value = health;

        // Changing the boss' colour depending on vulnerability
        if (!isVulnerable)
        {
            spriteRenderer.color = new Color(0, 0.92f, 1, 0.8f);
            hpBarColor.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        } else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            hpBarColor.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }

        // Changing the boss' phase depending on their health
        if (health <= baseHealth / 3f)
        {
            phaseNumber = 3;
            animator.SetInteger("phaseNumber", phaseNumber);
        }
        else if (health <= 2 * baseHealth / 3f)
        {
            phaseNumber = 2;
            animator.SetInteger("phaseNumber", phaseNumber);
        }

        // Enemy AI Depending on phase
        if (phaseNumber == 1)
        {
            // Make enemy vulnerable once 2 balls have been scored
            if (ballCount >= 2 && !isVulnerable)
            {
                basketballs.SetActive(false);
                goalArea.SetActive(false);
                isVulnerable = true;
            }
        } else if (phaseNumber == 2)
        {
            // Phase 2 AI
            if (!phase2VariableChange)
            {
                MoveSpeed = phase2Speed;
                MaxDist = phase2MaxDist;
                middleNoPlatforms.SetActive(false);
                middlePlatformsPhase2.SetActive(true);

                // Spawning random breakable
                SpawnRandomBreakable();

                isVulnerable = false;
                breakableOnField = true;
                phase2VariableChange = true;
            }
            // If no breakable on field, make vulnerability timer decay and spawn rocks
            if (!breakableOnField)
            {
                isVulnerable = true;
                vulnerabilityTimer -= Time.deltaTime;

                // Spawning rocks (4 per spawn cycle)
                if (!spawnedRock)
                {
                    Instantiate(rockPrefab, new Vector3(17, 29, 0), Quaternion.identity);
                    Instantiate(rockPrefab, new Vector3(17, 29, 0), Quaternion.identity);
                    Instantiate(rockPrefab, new Vector3(17, 29, 0), Quaternion.identity);
                    Instantiate(rockPrefab, new Vector3(17, 29, 0), Quaternion.identity);
                    spawnedRock = true;
                }
                
                // Less than x breakables broken
                if (breakableCount <= 5)
                {
                    // Once vulnerability timer finishes, spawn another breakable
                    if (vulnerabilityTimer <= 0)
                    {
                        print("Spawned breakable");
                        isVulnerable = false;
                        spawnedRock = false;
                        SpawnRandomBreakable();
                        vulnerabilityTimer = 5f;
                        breakableCount++;
                    }
                }
                
            }

            
        } else if (phaseNumber == 3)
        {
            // Phase 3 AI
            if (!phase3VariableChange)
            {
                MoveSpeed = phase3Speed;
                MaxDist = phase3MaxDist;
                middlePlatformsPhase2.SetActive(false);
                middlePlatformsPhase3.SetActive(true);

                // Deleting all of the falling rocks
                var foundObjects = FindObjectsOfType<FallingRock>();
                foreach (FallingRock fallingRock in foundObjects)
                {
                    Destroy(fallingRock.gameObject);
                }

                isVulnerable = false;
                phase3VariableChange = true;
            }
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

    public void SpawnRandomBreakable()
    {
        int spawnRandom = Random.Range(1, 4);
        
        if (spawnRandom == 1)
        {
            Breakable1.SetActive(true);
        }
        else if (spawnRandom == 2)
        {
            Breakable2.SetActive(true);
        }
        else if (spawnRandom == 3)
        {
            Breakable3.SetActive(true);
        }
        breakableOnField = true;
    }


}
