using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public float minX;
    public float maxX;

    public float maxY;
    public float minY;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < minY)
        {
            rb.velocity = Vector2.zero;
            transform.localPosition = new Vector3(Random.Range(minX, maxX), maxY, 0f);
        }
    }

}
