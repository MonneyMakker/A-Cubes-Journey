using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 initialPosition;
    bool platformMovesBack;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }
    void Update()
    {
        if(platformMovesBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, 25f * Time.deltaTime);
        }
        if(transform.position.y == initialPosition.y)
        {
            platformMovesBack = false;
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D (Collision2D other)
    {
        if(other.gameObject.tag.Equals ("Player") && !platformMovesBack)
        {
            Invoke("DropPlatform", 0.33f);
        }
    }
    void DropPlatform()
    {
        rb.isKinematic = false;
        Invoke("GetPlatformBack", 3.3f);
    }
    void GetPlatformBack()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        platformMovesBack = true;

    }
}
