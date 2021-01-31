using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    Text coin;
    public static int coinAmount;
    void Start()
    {
        coin = GetComponent<Text> ();
    }

    // Update is called once per frame
    void Update()
    {
        coin.text = coinAmount.ToString();
    }
}
