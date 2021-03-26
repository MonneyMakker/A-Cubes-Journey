using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform checkpoint;
    public Text distanceText;
    private float distance;
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        distance = (checkpoint.transform.position.x - transform.position.x);
        distanceText.text = "Distance: " + distance.ToString("F1") + " Meters to Finish";

        if (distance <= 0)
        {
            distanceText.text = "Finish";
        }
    }
}

