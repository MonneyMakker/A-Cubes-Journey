using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    public int Health = 3;
    
    public int health { get { return slimeHealth; }}
    int slimeHealth;
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        slimeHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (slimeHealth <= 0)
        {
            Debug.Log("You Have Died!");
            StartCoroutine("RestartScene");
        }

    }
    public IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("SampleScene");
    }

    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + 5.0f * horizontal * Time.deltaTime;
        position.y = position.y + 5.0f * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        slimeHealth = Mathf.Clamp(slimeHealth + amount, 0, Health);
        Debug.Log(slimeHealth + "/" + Health);
    }
}
