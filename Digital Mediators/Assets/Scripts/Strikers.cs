using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strikers : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody2D.velocity = new Vector2(5f, 0f);
    }
}
