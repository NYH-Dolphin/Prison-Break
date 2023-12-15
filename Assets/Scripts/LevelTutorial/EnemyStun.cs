using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace LevelTutorial
{
public class EnemyStun : MonoBehaviour
{
    private bool first = true;
    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<EnemyBehaviour>().notStunned && first)
        {
            LockDoorBehaviour.Instance.OnOpenDoor();
            first = false;
        }
            

    }
}
}
