using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public AudioClip healthClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeController slimy = other.GetComponent<SlimeController>();

        if (slimy != null)
        {
            if(slimy.health  < slimy.Health)
            {
                slimy.ChangeHealth(1);
                Destroy(gameObject);
                slimy.PlaySound(healthClip);


            }
        }
    }
}
