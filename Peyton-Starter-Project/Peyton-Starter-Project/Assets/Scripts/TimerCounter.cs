using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerCounter : MonoBehaviour
{
    float timer = 0f;
    Text timerText;

    public static TimerCounter instance;
    void Start()
    {
        StartCoroutine(Wait());
        timerText = gameObject.GetComponent<Text > ();
    }

    // Update is called once per frame

IEnumerator Wait()
{
        yield return new WaitForSeconds(2.0f);
        timer = 0;


    }
    public void Music()
    {
        
       
    }
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = "Timer: " + Mathf.Round(timer);
        if (timer >= 10)
        {
            SceneManager.LoadScene("SampleScene");
            CoinCounter.coinAmount = 0;
        }
    }
}
