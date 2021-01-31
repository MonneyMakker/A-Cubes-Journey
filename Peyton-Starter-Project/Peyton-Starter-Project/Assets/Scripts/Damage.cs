using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        SlimeController slimy = other.GetComponent<SlimeController >();

        if (slimy != null)
        {
            slimy.ChangeHealth(-1);
        }
    }

}
