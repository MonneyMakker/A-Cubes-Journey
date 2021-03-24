using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenu;

    public GameObject player;

    public GameObject pause;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        player.SetActive(true);
        pause.SetActive(true);
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        player.SetActive(false);
    }
    public void LoadMainMenu(string MainMenu)
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
        CoinCounter.coinAmount = 0;
    }
    public void RestartScene(string SampleScene)
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1f;
    }
}
