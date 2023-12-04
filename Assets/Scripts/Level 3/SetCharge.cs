using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharge : MonoBehaviour
{
    public GameObject Explosions;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player") //CHANGE TO E INPUT IN INPUT SYSTEM
        {
            Explosions.GetComponent<ExplosionTrigger>().charges++;
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<SpriteRenderer>().color = Color.green;
        }

    }
}
