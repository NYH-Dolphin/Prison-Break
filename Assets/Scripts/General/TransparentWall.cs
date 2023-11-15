using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public float transparency;
    // Update is called once per frame
    void Update()
    {
        CameraTransparent(transparency);
    }

    public void CameraTransparent(float t)
    {
        Material wallMat = this.GetComponent<SpriteRenderer>().material;
        wallMat.SetFloat("_Transparency", t);

    }

}
