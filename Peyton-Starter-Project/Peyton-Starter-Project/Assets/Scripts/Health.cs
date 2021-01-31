using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeController slimy = other.GetComponent<SlimeController>();

        if (slimy != null)
        {
            if(slimy.health  < slimy.Health)
            {
                slimy.ChangeHealth(1);
                Destroy(gameObject);
            }
        }
    }
}
