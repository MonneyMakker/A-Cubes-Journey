using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
public Image progressBar;

     private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (operation.progress < 1)
        {
            progressBar.fillAmount = operation.progress;
            yield return null;
        }
    }
}
