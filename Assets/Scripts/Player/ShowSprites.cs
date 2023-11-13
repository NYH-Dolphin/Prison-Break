using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSprites : MonoBehaviour
{
    public bool showing = true;

    void Update()
    {
        if(transform.childCount > 0)
        {
            if(showing)
                this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            else
                this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
