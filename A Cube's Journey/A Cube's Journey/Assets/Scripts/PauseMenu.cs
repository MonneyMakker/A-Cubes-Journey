using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenu;
    public GameObject pause;
    public AudioSource audioSource;
    public AudioSource pauseMusic;
    public GameObject player;
    void Start()
    {
    }

    void Update()
    {
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        pause.SetActive(true);
        player.SetActive(true);
        audioSource.Play();
        pauseMusic.Stop();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        player.SetActive(false);
        audioSource.Pause();
        pauseMusic.Play();
    }
    public void LoadMainMenu(string MainMenu)
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void RestartScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
        Time.timeScale = 1f;
    }
    
}
