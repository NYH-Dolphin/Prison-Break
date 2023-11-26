using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCube : MonoBehaviour
{
    public float transparency;
    // Update is called once per frame
    void Update()
    {
        CameraTransparent(transparency);
    }

    public void CameraTransparent(float t)
    {
        Material wallMat = this.GetComponent<MeshRenderer>().material;
        wallMat.SetFloat("_Transparency", t);

    }

}
