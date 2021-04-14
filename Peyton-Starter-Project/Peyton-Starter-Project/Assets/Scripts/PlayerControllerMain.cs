
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerControllerMain : MonoBehaviour
{
    public static int health;
    private int key, key2, key3, key4, key5;    
    public Slider staminaBarShift;
    public int maxStamina = 100;
    [SerializeField] float currentStamina;
    private Coroutine regen;
    AudioSource audioSource;
    public AudioClip hurtClip, healthClip, jumpSound, transitionSound, dashSound, launchClip, crunchClip, checkPointClip;
    public AudioSource moving;
    public AudioSource backgroundMusic, rainMusic;
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
    private bool jumpAllowed;
    public float launchForce;
    public float cooldownTime = 1.5f;
    private float nextFireTime = 0;
    float DashDirection;
    public Transform particlePoint;
    public Transform jumpPoint;
    public Transform spawnPoint;
    public Transform checkPoint, checkPoint2;
    public GameObject normalFlag, normalFlag2;
    public GameObject touchedFlag, touchedFlag2;
    Vector3 respawnPoint;
    public GameObject heart1, heart2, heart3, heart4, heart5, dashIcon, JumpIcon, Transition, keys1, keys2, keys3, keys4, keys5, 
    endingTransition;
    public ParticleSystem Particles;
    public ParticleSystem particlesJump;
    public ParticleSystem particlesDeath;
    public ParticleSystem checkPointFlag, checkPointFlag2;
    public GameObject dustCloud;
    Vector3 cameraInitialPosition;
    public float shakeMagnitude = 0.10f, shakeTime = 0.4f;
    public Camera mainCamera;
    bool isMoving;
    bool Dashing;
    private Animator animator;
    bool isSprinting;
     public GameObject exitdialogBox1;
    public TMP_Text exitdialog1Text;
    public GameObject exitdialogBox2;
    public TMP_Text exitdialog2Text;
    public bool playerInRange;
    public AudioClip signSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = 5;
        audioSource = GetComponent<AudioSource>();
        facingRight = true;
        currentStamina = maxStamina;
        staminaBarShift.maxValue = maxStamina;
        staminaBarShift.value = maxStamina;
        Transition.SetActive(false);
        endingTransition.SetActive(false);
        isDashing = false;
        animator = GetComponent<Animator>();
        Time.timeScale = 1;
        key = 0;
        key2 = 0;
        key3 = 0;
        key4 = 0;
        key5 = 0;
        normalFlag.SetActive(true);
        respawnPoint = spawnPoint.transform.position;
    }
        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
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
        endingTransition = GameObject.Find("EndingTransition");
        keys1 = GameObject.Find("KeyPickup1");
        keys2 = GameObject.Find("KeyPickup2");
        keys3 = GameObject.Find("KeyPickup3");
        keys4 = GameObject.Find("KeyPickup4");
        keys5 = GameObject.Find("KeyPickup5");
    }
    private void Flip(float movX)
    {
        if (movX > 0 && !facingRight || movX < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
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
            jumpAllowed = true;
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

        if(movX !=0 && isGrounded == true && Speed == 7)
        {
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);
        if(Speed == 12 && movX !=0 && isGrounded == true)
        {
            animator.SetBool("isShifting", true);
        }
        else
            animator.SetBool("isShifting", false);
        if(isDashing == true && movX !=0)
        {
            animator.SetBool("isDashing", true);
        }
        else 
            animator.SetBool("isDashing", false);
        if (jumpAllowed == true && isGrounded == false && doubleJumpAllowed == true && Input.GetKeyDown(KeyCode.Space))
            animator.SetBool("isJumping", true);
        else
        {
            animator.SetBool("isJumping", false);
        }
        if(doubleJumpAllowed == false && Input.GetKeyDown(KeyCode.Space) && isGrounded == false)
        {
            animator.SetBool("isDoubleJumping", true);
        }
        else
            animator.SetBool("isDoubleJumping", false);

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
            isSprinting = true;
            if (Input.GetKey(KeyCode.LeftShift) && isSprinting == true && movX != 0)
            {
                UseStamina(0.40f);
                Speed = 12f;
            }
            else
            {
                Speed = 7f;
            }
        }
        else if (currentStamina < 0.40f )
        {
            staminaBarShift.value = currentStamina;
            Speed = 7f;
            isSprinting = false;
        }

        if(key == 1)
        {
            keys1.SetActive(true);
        }
        else
            keys1.SetActive(false);
        if(key2 ==1)
        {
            keys2.SetActive(true);
        }
        else
            keys2.SetActive(false);
        if(key3 == 1)
        {
            keys3.SetActive(true);
        }
        else
            keys3.SetActive(false);
        if(key4 == 1)
        {
            keys4.SetActive(true);
        }
        else
            keys4.SetActive(false);
         if(key5 == 1)
        {
            keys5.SetActive(true);
        }
        else
            keys5.SetActive(false);
        
        if (health == 0)
        {
            Debug.Log("You Have Died!");
            PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
            cc.enabled = false;
            Time.timeScale = 0;
            backgroundMusic.Pause();
            loseMenu.SetActive(true);
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
      
       public void LoadNextScene(int SceneIndex)
    {
        backgroundMusic.Pause();
        endingTransition.SetActive(true);
        StartCoroutine("SceneTransition");
    }
    public void StayInScene()
    {
                playerInRange = false;
                exitdialogBox2.SetActive(false);
                PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
                cc.enabled = true;
                PlaySound(signSound);
                isGrounded = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "Health")
            {
            if (health < 5)
                {
                health += 1;
                Destroy(collision.gameObject);
                PlaySound(healthClip);
            }
            }
            if(collision.gameObject.tag  == "keys")
            {
                if(key < 1)
                {
                key += 1;
                PlaySound(crunchClip);
                Destroy(collision.gameObject);
                }
            }
            if(collision.gameObject.tag  == "keys2")
            {
                if(key2 < 1)
                {
                key2 += 1;
                PlaySound(crunchClip);
                Destroy(collision.gameObject);
                }
            }
            if(collision.gameObject.tag  == "keys3")
            {
                if(key3 < 1)
                {
                key3 += 1;
                PlaySound(crunchClip);
                Destroy(collision.gameObject);
                }
            }
            if(collision.gameObject.tag  == "keys4")
            {
                if(key4 < 1)
                {
                key4 += 1;
                PlaySound(crunchClip);
                Destroy(collision.gameObject);
                }
            }
            if(collision.gameObject.tag  == "keys5")
            {
                if(key5 < 1)
                {
                key5 += 1;
                PlaySound(crunchClip);
                Destroy(collision.gameObject);
                }
            }
        if (collision.gameObject.tag == "CheckPoint")
        {
            PlaySound(checkPointClip);
            checkPointFlag.Play();
            respawnPoint = checkPoint.transform.position;
            normalFlag.SetActive(false);
            touchedFlag.SetActive(true);
        }
        if(collision.gameObject.tag == "CheckPoint2")
            {
            PlaySound(checkPointClip);
            checkPointFlag2.Play();
            respawnPoint = checkPoint2.transform.position;
            normalFlag2.SetActive(false);
            touchedFlag2.SetActive(true);
            }
            if (collision.gameObject.tag == "Spikes")
            {
            rb.isKinematic = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            animator.SetTrigger("isDead");
            dustCloud.SetActive(false);
            particlesDeath.Play();
            PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
            cc.enabled = false;
            health -= 1;
            PlaySound(hurtClip);
            StartCoroutine("WaitforDeath");
            }

            if (collision.gameObject.tag == "PinkSpikes")
            {
            rb.isKinematic = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            animator.SetTrigger("isDead");
            dustCloud.SetActive(false);
            particlesDeath.Play();
            PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
            cc.enabled = false;
            health -= 1;
            PlaySound(hurtClip);
            StartCoroutine("WaitforDeath");
            }
        if(collision.gameObject.tag =="Transition")
        {
            Transition.SetActive(false);
            PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
            cc.enabled = true;
            dustCloud.SetActive(true);
            isDashing = false;
        }
    }
        IEnumerator WaitforDeath()
    {
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.2f);
        rb.isKinematic = false;
        gameObject.GetComponent<Renderer>().enabled = true;
        Transition.SetActive(true);
        currentStamina = maxStamina;
        staminaBarShift.value = currentStamina;
        PlaySound(transitionSound);
        isDashing = true;
        dashIcon.SetActive(true);
        transform.position = respawnPoint;
        if(health == 0 )
            {
            loseMenu.SetActive(true);
            backgroundMusic.Pause();
            }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
             isGrounded = true;
             Instantiate(dustCloud, jumpPoint.transform.position, dustCloud.transform.rotation);
             coroutineAllowed = true;
        }
        if(collision.gameObject.tag == "MovingPlatform")
        {
            gameObject.transform.parent = collision.gameObject.transform;
            isGrounded = true;
        }
        if(collision.gameObject.tag == "Death")
        {
            rb.isKinematic = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            animator.SetTrigger("isDead");
            dustCloud.SetActive(false);
            particlesDeath.Play();
            PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
            cc.enabled = false;
            health -= 1;
            PlaySound(hurtClip);
            StartCoroutine("WaitforDeath");
        }
        if (collision.gameObject.tag == "ExitDoor")
        {
            if (key == 1 && key2 == 1 && key3 == 1 && key4 == 1 && key5 == 1)
            {
                playerInRange = true;
                exitdialogBox2.SetActive(true);
                PlayerControllerMain cc = GetComponent<PlayerControllerMain>();
                cc.enabled = false;
                PlaySound(signSound);
                rb.velocity = Vector2.zero;
                rb.isKinematic = false;
                isGrounded = false;
                animator.SetBool("isIdle", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isShifting", false);

            }
            else
            {
                playerInRange = false;
                exitdialogBox1.SetActive(true);
                PlaySound(signSound);
            }
        }
        if(collision.gameObject.tag == "Trampoline")
        {
            isGrounded = true;
            PlaySound(launchClip);
            rb.velocity = Vector3.up * launchForce;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = false;
            coroutineAllowed = false;
        }
        if(collision.gameObject.tag == "ExitDoor")
        {
            exitdialogBox1.SetActive(false);
        }
         if(collision.gameObject.tag == "MovingPlatform")
        {
            gameObject.transform.parent = null;
        }
    }

    IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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