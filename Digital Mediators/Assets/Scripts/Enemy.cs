using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Rigidbody2D m_Rigidbody2D;
    private float moveTimer = 3f;
    private float moveCount = 0f;
    private bool movingRight = false;

    public int health = 100;

	public GameObject deathEffect;

    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
	public void TakeDamage (int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	void Die ()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

    void Move()
    {
        moveCount += Time.deltaTime;
        
        if (moveCount > moveTimer)
        {
            moveCount = 0;
            if (movingRight)
            {
                movingRight = false;
                m_Rigidbody2D.AddForce(new Vector2(300f, 0f));
            } else
            {
                movingRight = true;
                m_Rigidbody2D.AddForce(new Vector2(-300f, 0f));
            }
            
        }
        
    }

    public void Update()
    {
        Move();
    }

}
