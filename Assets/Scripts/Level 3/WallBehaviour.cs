using System;
using Enemy;
using UnityEngine;

namespace Level_3
{
    public class WallBehaviour : MonoBehaviour
    {
        public bool bActivate;
       

        private void OnCollisionEnter(Collision other)
        {
            if (bActivate)
            {
                GameObject obj = other.gameObject;
                if (obj.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    obj.GetComponent<PlayerEnemy>().Kill();
                }
                else if (obj.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    obj.GetComponent<EnemyBehaviour>().OnHit(2, false);
                }
            }
        }
    }
}