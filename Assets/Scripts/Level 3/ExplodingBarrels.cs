using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarells : MonoBehaviour
{
    public GameObject Explosions;
    public GameObject Tower;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Flaming Metal")
        {
            Destroy(Tower);
            Explosions.GetComponent<ExplosionTrigger>().Explode();
        }
            
    }
}
