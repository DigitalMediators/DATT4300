using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Enemy enemy;

    public float speedX;
    public float minX;
    public float maxX;

    public float speedY;
    public float minY;
    public float maxY;

    public int health = 5;
    public AudioSource deathSound;
    public GameObject deathEffect;

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x <= minX || transform.localPosition.x > maxX)
        {
            speedX *= -1;
        }
        if (transform.localPosition.y <= minY || transform.localPosition.y > maxY)
        {
            speedY *= -1;
        }
        transform.localPosition = new Vector3(transform.localPosition.x + speedX, transform.localPosition.y + speedY, 0);
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
        deathSound.Play();
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        enemy.breakableOnField = false;
        gameObject.SetActive(false);
    }

}
