using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    public int Health = 3;
    AudioSource audioSource;
    public AudioSource WinningMusic;
    public bool WinningSong = false;

    public GameObject winMenu;
    public GameObject loseMenu;
    public int health { get { return slimeHealth; }}
    int slimeHealth;
    Rigidbody2D rigidbody2d;

    float horizontal;
    float vertical;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        slimeHealth = Health;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StopingObject());
    }

    IEnumerator StopingObject()
    {
        SlimeController cc = GetComponent<SlimeController>();
        cc.enabled = false;
        yield return new WaitForSeconds(2);
        cc.enabled = true;
    }

    public void PlaySound(AudioClip clip)
{
        audioSource.PlayOneShot(clip);
    }
    public void Music()
    {
        WinningSong = false;
    }
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (slimeHealth <= 0)
        {
            Debug.Log("You Have Died!");
            SlimeController cc = GetComponent<SlimeController>();
            AudioSource background = gameObject.GetComponent<AudioSource>();
            background.Stop();
            cc.enabled = false;
            loseMenu.SetActive(true);
            Time.timeScale = 0f;
            CoinCounter.coinAmount = 0;

        }
        if (CoinCounter.coinAmount >= 5)
        {
            SlimeController cc = GetComponent<SlimeController>();
            AudioSource background = gameObject.GetComponent<AudioSource>();
            background.Stop();
            cc.enabled = false;
            WinningSong = true;
            WinningMusic.Play();
            winMenu.SetActive(true);
        }

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
