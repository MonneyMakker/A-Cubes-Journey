using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public float speed;
    public int waitTime;
    Vector2 platform;
    void Start()
    {
        platform = pos2.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, platform, speed * Time.fixedDeltaTime);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pos1"))
        {
            StartCoroutine("MoveRight");
        }
        if (collision.gameObject.CompareTag("pos2"))
        {
            StartCoroutine("MoveLeft");
        }
    }
    public IEnumerator MoveRight()
    {
        yield return new WaitForSeconds(waitTime);
        platform = pos2.position;
    }
    public IEnumerator MoveLeft()
    {
        yield return new WaitForSeconds(waitTime);
        platform = pos1.position;
    }
}
