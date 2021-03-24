using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    public static int health;
    AudioSource audioSource;
    public AudioSource WinningMusic;
    public AudioClip coinClip, hurtClip, healthClip, jumpSound, dashSound,shootingClip,chargeSound;
    public bool WinningSong = false;
    public GameObject winMenu;
    public GameObject loseMenu;
    Rigidbody2D rb;
    public float Speed;
    public float JumpForce;
    bool isGrounded = false;
    public float DashForce;
    public float StartDashTimer;
    float CurrentDashTimer;
    bool isDashing;
    private bool facingRight;
    public float cooldownTime = 2.5f;
    private float nextFireTime = 0;
    float DashDirection;
    public Transform firePoint;
    float movX;
    public GameObject bullet;
    public GameObject heart1, heart2, heart3, heart4, heart5, staminaBar;
    public ParticleSystem Particeles;
    public int maxAmmo = 10;
    public int ammo;
    public bool isFiring;
    public Text ammoDisplay;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = 5;
        audioSource = GetComponent<AudioSource>();
        facingRight = true;
        ammo = maxAmmo;
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
    private void Flip(float movX)
    {
        if (movX > 0 && !facingRight || movX < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            firePoint.Rotate(0, 180f, 0);
        }
    }
    void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector3(movX * Speed, rb.velocity.y);
        Flip(movX);

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
                rb.velocity = Vector3.zero;
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
        if (health == 0)
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
        if (Input.GetMouseButtonDown(2) && !isFiring && ammo > 0)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            PlaySound(shootingClip);
            Debug.Log("Firing");
            isFiring = true;
            ammo--;
            isFiring = false;
            ammoDisplay.text = ammo.ToString();
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

    void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "Ammo")
            {
                ammo = maxAmmo;
                Destroy(collision.gameObject);
                ammoDisplay.text = ammo.ToString();
                PlaySound(coinClip);
            }
            if (collision.gameObject.tag == "Coins")
            {
                CoinCounter.coinAmount += 1;
                Destroy(collision.gameObject);
                PlaySound(coinClip);
            }
            if (collision.gameObject.tag == "Health")
            {
            if (health < 5)
                {
                health += 1;
                Destroy(collision.gameObject);
                PlaySound(healthClip);
            }
            }
            if (collision.gameObject.tag =="Spikes")
            {
                health -= 1;
                PlaySound(hurtClip);
             }
        }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
             isGrounded = true;
        }
        if(collision.gameObject.tag == "Death")
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}