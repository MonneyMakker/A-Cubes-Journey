using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : MonoBehaviour
{
    public int Health = 5;
    AudioSource audioSource;
    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioClip chargeSound;
    public AudioSource WinningMusic;
    public bool WinningSong = false;
    public GameObject winMenu;
    public GameObject loseMenu;
    public int health { get { return slimeHealth; } }
    int slimeHealth;
    Rigidbody2D rb;
    public float Speed;
    public float JumpForce;
    bool isGrounded = false;
    public float DashForce;
    public float StartDashTimer;
    float CurrentDashTimer;
    bool isDashing;
    public float cooldownTime = 2.5f;
    private float nextFireTime = 0;
    float DashDirection;
    public GameObject heart1, heart2, heart3, heart4, heart5, staminaBar;
    public ParticleSystem Particeles;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        heart4 = GameObject.Find("HeartContain4");
        heart5 = GameObject.Find("HeartContain5");
        staminaBar = GameObject.Find("DashBar");
    }

    void Update()
    {
        float movX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movX * Speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(1) && movX != 0)
            {
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = (int)movX;
                Particeles.Play();
                Debug.Log("ability used");
                audioSource.clip = dashSound;
                audioSource.Play();
                staminaBar.SetActive(false);
            }
            if (isDashing)
            {
                rb.velocity = transform.right * DashDirection * DashForce;
                CurrentDashTimer -= Time.deltaTime;

                if (CurrentDashTimer <= 0)
                {
                    isDashing = false;
                    StartCoroutine("Dash");
                }
            }
        }
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
            Time.timeScale = 0f;
        }
        if (health == 0)
        {
            heart5.SetActive(false);
            heart4.SetActive(false);
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        if (health == 1)
        {
            heart5.SetActive(false);
            heart4.SetActive(false);
            heart3.SetActive(false);
            heart2.SetActive(false);
            heart1.SetActive(true);
        }
        if (health == 2)
        {
            heart5.SetActive(false);
            heart4.SetActive(false);
            heart3.SetActive(false);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }
        if (health == 3)
        {
            heart5.SetActive(false);
            heart4.SetActive(false);
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }
        if (health == 4)
        {
            heart5.SetActive(false);
            heart4.SetActive(true);
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }
        if (health == 5)
        {
            heart5.SetActive(true);
            heart4.SetActive(true);
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(true);
        }
}

    IEnumerator Dash()
    {
        nextFireTime = Time.time + cooldownTime;
        yield return new WaitForSeconds(2.5f);
        audioSource.clip = chargeSound;
        audioSource.Play();
        staminaBar.SetActive(true);
    }
    void Jump()
    {
        rb.AddForce(transform.up * JumpForce);
        audioSource.clip = jumpSound;
        audioSource.Play();
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }

    public void ChangeHealth(int amount)
    {
        slimeHealth = Mathf.Clamp(slimeHealth + amount, 0, Health);
        Debug.Log(slimeHealth + "/" + Health);
    }
    private void Flip()
    {

    }
}
