using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public AudioClip coinClip;


    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeController slimy = other.GetComponent<SlimeController>();
        {
            if (slimy != null)
            {
                CoinCounter.coinAmount += 1;
                Destroy(gameObject);
                slimy.PlaySound(coinClip);
            }
        }
    }
}
