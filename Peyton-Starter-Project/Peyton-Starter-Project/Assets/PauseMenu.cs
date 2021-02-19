using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GamePaused = false;
    public GameObject pauseMenu;
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
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
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
