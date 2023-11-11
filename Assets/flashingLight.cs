using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashingLight : MonoBehaviour
{
    private float counter;
    private float timer = 5f;
    private bool flip = false;

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<Light>().intensity >= 1.83f)
        {
            flip = true;
        }
        if(this.gameObject.GetComponent<Light>().intensity <= 1.2f)
        flip = false;

        if(flip)
        {
            this.gameObject.GetComponent<Light>().intensity -= 0.00042f;
        }
        else this.gameObject.GetComponent<Light>().intensity += 0.0003f;
    }
}
