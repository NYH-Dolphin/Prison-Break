using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelTutorial
{
public class buttonTrigger : MonoBehaviour
{
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {

        if(col.tag == "Player Hitbox")
        {
            LockDoorBehaviour.Instance.OnOpenDoor();
        }
    }

}
}
