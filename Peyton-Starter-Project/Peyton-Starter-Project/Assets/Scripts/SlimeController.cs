using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    public static int health;
    public Slider staminaBarShift;
    public int maxStamina = 100;
    private static float currentStamina;
    private Coroutine regen;
    AudioSource audioSource;
    public AudioSource WinningMusic;
    public AudioClip coinClip, hurtClip, healthClip, jumpSound,
    dashSound, shootingClip;
    public bool WinningSong = false;
    public AudioSource moving;
    public GameObject winMenu;
    public GameObject loseMenu;
    Rigidbody2D rb;
    public float Speed;
    public float JumpForce;
    bool doubleJumpAllowed = false;
    bool coroutineAllowed, isGrounded = false;
    public float DashForce;
    public float StartDashTimer;
    float CurrentDashTimer;
    bool isDashing;
    private bool facingRight;
    public float cooldownTime = 1.5f;
    private float nextFireTime = 0;
    float DashDirection;
    public Transform firePoint;
    public Transform particlePoint;
    public Transform jumpPoint;
    public Transform spawnPoint;
    public GameObject bullet;
    public GameObject heart1, heart2, heart3, heart4, heart5, dashIcon, JumpIcon, Transition;
    public ParticleSystem Particles;
    public ParticleSystem particlesJump;
    public ParticleSystem particlesDeath;
    public int maxAmmo = 10;
    public int ammo;
    public bool isFiring;
    public Text ammoDisplay;
    public GameObject Audio;
    public GameObject dustCloud;
    Vector3 cameraInitialPosition;
    public float shakeMagnitude = 0.10f, shakeTime = 0.4f;
    public Camera mainCamera;
    bool isMoving;
    bool Dashing;
    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = 5;
        audioSource = GetComponent<AudioSource>();
        facingRight = true;
        SlimeController cc = GetComponent<SlimeController>();
        cc.enabled = false;
        ammo = maxAmmo;
        currentStamina = maxStamina;
        staminaBarShift.maxValue = maxStamina;
        staminaBarShift.value = maxStamina;
        Transition.SetActive(false);
        isDashing = false;
        animator = GetComponent<Animator>();
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
        dashIcon = GameObject.Find("DashIcon");
        JumpIcon = GameObject.Find("JumpIcon");
        Transition = GameObject.Find("Object");
    }
    private void Flip(float movX)
    {
        if (movX > 0 && !facingRight || movX < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            firePoint.Rotate(0, 180f, 0);
            particlePoint.Rotate(0, 180f, 0);
            jumpPoint.Rotate(0, 180f, 0);
        }
    }
    void Update()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(movX * Speed, rb.velocity.y);
        Flip(movX);
            if(isGrounded == true)
            {
            doubleJumpAllowed = true;
            JumpIcon.SetActive(true);
            }
            if (rb.velocity.x !=0)
            isMoving = true;
            else isMoving = false;
            if (isMoving && isGrounded)
            {
                if(!moving.isPlaying)
                moving.Play();
            }
            else 
                moving.Stop();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        else if (doubleJumpAllowed && Input.GetKeyDown(KeyCode.Space))
        {
            DoubleJump();
            doubleJumpAllowed = false;
        }
        
        if (isGrounded && movX !=0 && coroutineAllowed)
        {
            StartCoroutine("SpawnCloud");
            coroutineAllowed = false;
        }
        if (movX == 0 && isGrounded)
        {
            StopCoroutine("SpawnCloud");
            coroutineAllowed = true;
        }

        if(movX == 0 && isGrounded == true)
        {
            animator.SetBool("isIdle", true);
        }
        else
            animator.SetBool("isIdle", false);

        if(movX !=0 && isGrounded == true)
        {
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(1) && movX != 0 && isDashing == false)
            {
                isDashing = true;
                CurrentDashTimer = StartDashTimer;
                rb.velocity = Vector2.zero;
                DashDirection = movX;
                Particles.Play();
                Debug.Log("ability used");
                audioSource.clip = dashSound;
                audioSource.Play();
                dashIcon.SetActive(false);
            }
            if (isDashing)
            {
                rb.velocity = transform.right * DashDirection * DashForce;
                CurrentDashTimer -= Time.deltaTime;
                cameraInitialPosition = mainCamera.transform.position;
                InvokeRepeating("StartCameraShaking", 0f, 0.0005f);
                Invoke("StopCameraShaking", shakeTime);
        

                if (CurrentDashTimer <= 0)
                {
                    isDashing = false;
                    StartCoroutine("Dash");
                }

            }

        }
        if (currentStamina > 0.40f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                UseStamina(0.40f);
                Speed = 12f;
            }
            else
            {
                Speed = 7f;
            }
        }
        else if (currentStamina < 0.40f)
        {
            staminaBarShift.value = currentStamina;
            Speed = 7f;
        }
        
        if (health == 0)
        {
            Debug.Log("You Have Died!");
            SlimeController cc = GetComponent<SlimeController>();
            cc.enabled = false;
            CoinCounter.coinAmount = 0;
            SceneManager.LoadScene("SampleScene");
        }

        if (CoinCounter.coinAmount >= 5)
        {
            SlimeController cc = GetComponent<SlimeController>();
            cc.enabled = false;
            WinningSong = true;
            Audio.SetActive(false);
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
    public void UseStamina(float amount)
    {
        if(currentStamina - amount >= 0 )
        {
            currentStamina -= amount;
            staminaBarShift.value = currentStamina;

            if(regen !=null)
            {
                StopCoroutine(regen);
            }
            regen = StartCoroutine(RegenStamina());
        }
    }
    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(1f);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100;
            staminaBarShift.value = currentStamina;
            yield return new WaitForSeconds(0.05f);
        }
    }
    void StartCameraShaking()
    {
        float cameraShakingX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float cameraShakingY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        Vector3 cameraposition = mainCamera.transform.position;
        cameraposition.x += cameraShakingX;
        cameraposition.y += cameraShakingY;
        mainCamera.transform.position = cameraposition;
    }
    void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }

    IEnumerator Dash()
    {
        nextFireTime = Time.time + cooldownTime;
        yield return new WaitForSeconds(1.5f);
        dashIcon.SetActive(true);
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(transform.up * JumpForce);
        audioSource.clip = jumpSound;
        audioSource.Play();
        isGrounded = false;
    }
    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(transform.up * JumpForce);
        audioSource.clip = jumpSound;
        audioSource.Play();
        JumpIcon.SetActive(false);
        particlesJump.Play();
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
            if (collision.gameObject.tag == "Spikes")
            {
            gameObject.GetComponent<Renderer>().enabled = false;
            animator.SetTrigger("isDead");
            dustCloud.SetActive(false);
            particlesDeath.Play();
            SlimeController cc = GetComponent<SlimeController>();
            cc.enabled = false;
            health -= 1;
            PlaySound(hurtClip);
            StartCoroutine("WaitforDeath");
        }
        if(collision.gameObject.tag =="Transition")
        {
            Transition.SetActive(false);
            SlimeController cc = GetComponent<SlimeController>();
            cc.enabled = true;
        }
        }
        IEnumerator WaitforDeath()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<Renderer>().enabled = true;
        Transition.SetActive(true);
        dustCloud.SetActive(true);
        transform.position = spawnPoint.transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
             isGrounded = true;
             Instantiate(dustCloud, jumpPoint.transform.position, dustCloud.transform.rotation);
             coroutineAllowed = true;
        }
        if(collision.gameObject.tag == "Death")
        {
            animator.SetTrigger("isDead");
            gameObject.GetComponent<Renderer>().enabled = false;
            particlesDeath.Play();
            Particles.Stop();
            health -= 1;
            Speed = 0;
            PlaySound(hurtClip);
            SlimeController cc = GetComponent<SlimeController>();
            cc.enabled = false;
            StartCoroutine("WaitforDeath");
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = false;
            coroutineAllowed = false;
        }
    }
    IEnumerator SpawnCloud()
    {
        while (isGrounded)
        {
            Instantiate(dustCloud, particlePoint.transform.position, transform.rotation);
            yield return new WaitForSeconds(0.10f);
        }
    }
}