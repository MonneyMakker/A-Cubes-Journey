using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Slider staminaBarShift;
    public int maxStamina = 100;
    [SerializeField] float currentStamina;
    private Coroutine regen;
    AudioSource audioSource;
    public AudioClip jumpSound, transitionSound, dashSound;
    public AudioSource moving;
    public AudioSource backgroundMusic, oceanMusic;
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
    public GameObject dashIcon, JumpIcon;
    public ParticleSystem Particles;
    public ParticleSystem particlesJump;
    public GameObject dustCloud;
    Vector3 cameraInitialPosition;
    public float shakeMagnitude = 0.10f, shakeTime = 0.4f;
    public Camera mainCamera;
    bool isMoving;
    bool Dashing;
    private Animator animator;
    bool isSprinting;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        facingRight = true;
        currentStamina = maxStamina;
        staminaBarShift.maxValue = maxStamina;
        staminaBarShift.value = maxStamina;
        isDashing = false;
        animator = GetComponent<Animator>();
        Time.timeScale = 1;
    }
        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
        private void Awake()
    {
        dashIcon = GameObject.Find("DashIcon");
        JumpIcon = GameObject.Find("JumpIcon");
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
    
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "ground")
        {
             isGrounded = true;
             Instantiate(dustCloud, jumpPoint.transform.position, dustCloud.transform.rotation);
             coroutineAllowed = true;
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