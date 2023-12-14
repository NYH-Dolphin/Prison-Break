using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killswitch : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
                Destroy(enemy);
        }
    }
}
