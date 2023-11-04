using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentController : MonoBehaviour
{
    public Transform playerHead;
    public SpriteRenderer sp;
    private SpriteRenderer lastSP;

    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        lastSP = sp;
    }

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
                
                sp = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                sp.color = new Color(1f,1f,1f,0.05f);

            }
        }
        else
            sp.color = new Color(1f,1f,1f,1f);
    }

}
