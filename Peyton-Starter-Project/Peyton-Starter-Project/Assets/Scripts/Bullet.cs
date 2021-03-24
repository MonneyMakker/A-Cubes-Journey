using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 25f;
    public Rigidbody2D rb;
    public float destroyTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        
    }
}
