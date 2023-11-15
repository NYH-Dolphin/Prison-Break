using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranspTrigger : MonoBehaviour
{
    public TransparentWall[] transparentObjects;
    public float transparency;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            foreach(TransparentWall t in transparentObjects)
            {
                t.transparency = transparency;
            }
        }
    }
}
