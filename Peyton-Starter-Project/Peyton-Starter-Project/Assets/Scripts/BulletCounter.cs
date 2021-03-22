using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounter : MonoBehaviour
{
    Text bullet;
    public static BulletCounter instance;
    public static float BulletAmount = 10;
    void Start()
    {
        bullet = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bullet.text = BulletAmount.ToString("");
    }
}