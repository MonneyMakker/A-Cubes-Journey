using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NarrativeWait : MonoBehaviour
{
    // Start is called before the first frame update
    public float wait_Time = 35f;
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(wait_Time);
        SceneManager.LoadScene("LoadingScreen");
    }
}
