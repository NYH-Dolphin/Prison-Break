using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentController : MonoBehaviour
{
    public Transform playerHead;
    public Material wallMat;
    

    private RaycastHit hit;


    // Update is called once per frame
    void Update()
    {
        CameraTransparent();
    }

    private void CameraTransparent()
    {

        Debug.DrawLine(transform.position, playerHead.position, Color.green);
        if(Physics.Linecast(transform.position, playerHead.position, out hit))
        {
            if(hit.collider.CompareTag("Wall"))
            {
                
                wallMat = hit.collider.gameObject.GetComponent<SpriteRenderer>().material;
                wallMat.SetFloat("_Transparency", 0f);

            }
            ;
        }
        //else
            //wallMat.SetFloat("_Transparency", 1f);
    }

}
