using System;
using UnityEngine;

namespace LevelTutorial
{
    public class EnemyDead : MonoBehaviour
    {
        

        private void OnDestroy()
        {
            LockDoorBehaviour.Instance.OnOpenDoor();
        }
    }
}