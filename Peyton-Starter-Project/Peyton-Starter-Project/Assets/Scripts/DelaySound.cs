using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaySound : MonoBehaviour
{
    // Start is called before the first frame update
   
    void Start()
    {
        AudioSource background = gameObject.GetComponent<AudioSource>();
        background.PlayDelayed(2.0f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
