using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource openSFX;
    private bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject enemies = GameObject.FindGameObjectWithTag("Enemy");
        if(enemies.layer != LayerMask.NameToLayer("Enemy"))
        {
            if(first) 
            {
                openSFX.Play();
                first = false;
            }
            Vector3 endPoint = transform.eulerAngles;
            endPoint.y = 0;
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endPoint, 0.02f);
        }
    }
}
