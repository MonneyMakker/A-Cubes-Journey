using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeCounter : MonoBehaviour
{
    Text slime;
    public static int slimeAmount;
    void Start()
    {
        slime = GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        slime.text = slimeAmount.ToString();
    }
}