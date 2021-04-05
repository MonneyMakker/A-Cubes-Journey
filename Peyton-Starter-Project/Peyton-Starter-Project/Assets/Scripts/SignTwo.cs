using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignTwo : MonoBehaviour
{

    public GameObject dialogBox;
    public GameObject rButton;
    public TMP_Text dialogText;
    public bool playerInRange;
    public AudioSource audioSource;
    public AudioClip signSound;
    // Start is called before the first frame update
    void Start()
    {
        rButton.SetActive(false);
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
            }
            else
            {
                dialogBox.SetActive(true);
                PlaySound(signSound);
                rButton.SetActive(false);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            rButton.SetActive(true);
            playerInRange = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogBox.SetActive(false);
            rButton.SetActive(false);
        }
    }
}