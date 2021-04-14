using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SwitchScene(string GrayScaleWorld)
    {
        SceneManager.LoadSceneAsync("GrayScaleWorld");
    }
    public void quitgame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void LoadLevel(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
