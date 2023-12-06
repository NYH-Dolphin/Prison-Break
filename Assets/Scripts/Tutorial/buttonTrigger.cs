using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource openSFX;
    public GameObject door;
    private bool open = false;
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {

        if(col.tag == "Enemy")
        {
            openSFX.Play();
            open = true;
        }
    }

    void Update()
    {
        if(open)
        {
            Vector3 endPoint = door.transform.eulerAngles;
            endPoint.y = 0;
            door.transform.eulerAngles = Vector3.Lerp(door.transform.eulerAngles, endPoint, 0.02f);
        }
    }
}
