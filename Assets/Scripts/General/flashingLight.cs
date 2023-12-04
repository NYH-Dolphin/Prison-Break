using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingLight : MonoBehaviour
{
    private float counter;
    private float timer = 5f;
    private bool flip = false;
    [SerializeField] private float upperIntensity;
    [SerializeField] private float lowerIntensity;
    [SerializeField] private float rate;

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<Light>().intensity >= upperIntensity)
        {
            flip = true;
        }
        if(this.gameObject.GetComponent<Light>().intensity <= lowerIntensity)
        flip = false;

        if(flip)
        {
            this.gameObject.GetComponent<Light>().intensity -= rate;
        }
        else this.gameObject.GetComponent<Light>().intensity += rate;
    }
}
