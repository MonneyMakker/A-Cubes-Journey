using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed Shoot Button");
            Shoot();
            BulletCounter.BulletAmount -= 1;
        
        if (BulletCounter.BulletAmount == 0)
            {
                Debug.Log("Out of Ammo");
            }
        }
    }

    void Shoot ()
    {
        Instantiate(bulletObject, firePoint.position, firePoint.rotation);
    }
}