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
    public int health { get { return slimeHealth; } }
    int slimeHealth;
    Rigidbody2D rigidbody2d;

    public GameObject heart1;

    public float publicSpeed;
    public GameObject heart2;
    public GameObject heart3;

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

    private void Awake()
    {
        heart1 = GameObject.Find("HeartContain1");
        heart2 = GameObject.Find("HeartContain2");
        heart3 = GameObject.Find("HeartContain3");
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (slimeHealth == 0)
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
        if (health == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        if (health == 1)
        {
            heart3.SetActive(false);
            heart2.SetActive(false);
            heart1.SetActive(true);
        }
        if (health == 2)
        {
            heart3.SetActive(false);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }
        if (health == 3)
        {
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(true);
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
