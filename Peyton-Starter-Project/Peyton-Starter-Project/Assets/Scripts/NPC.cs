using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
 public GameObject dialogBox;
    public GameObject rButton;
    public GameObject chatBubble;
    public TMP_Text dialogText;
    public bool playerInRange;
    public AudioSource audioSource;
    public AudioClip signSound;
    public Rigidbody2D rb;
    public AudioSource backgroundMusic;
    public AudioSource npcMusic;
    // Start is called before the first frame update
    void Start()
    {
        rButton.SetActive(false);
        chatBubble.SetActive(true);
    }

        public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && playerInRange)
        {
            if(dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
                PlaySound(signSound);
                backgroundMusic.Play();
                npcMusic.Stop();
            }
            else
            {
                dialogBox.SetActive(true);
                chatBubble.SetActive(false);
                rButton.SetActive(false);
                PlaySound(signSound);
                backgroundMusic.Pause();
                npcMusic.Play();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            rButton.SetActive(true);
            chatBubble.SetActive(false);
            playerInRange = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
            chatBubble.SetActive(true);
            rButton.SetActive(false);
        }
    }
}

