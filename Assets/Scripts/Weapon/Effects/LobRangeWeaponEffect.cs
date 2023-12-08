using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapon.Effects
{
    public class LobRangeWeaponEffect : MonoBehaviour
    {
        [SerializeField] private float fLargeRadius;
        [SerializeField] private float fSmallRadius;
        [SerializeField] private LayerMask lmEnemy;


        private HashSet<GameObject> _setEnemiesDetected = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _setEnemiesDetected.Add(other.gameObject);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _setEnemiesDetected.Remove(other.gameObject);
            }
        }


        public void ShowLobRange()
        {
            Vector3 pos = transform.position;

            // Large
            Debug.DrawLine(pos, pos + Vector3.forward * fLargeRadius, Color.yellow);
            Debug.DrawLine(pos, pos + Vector3.left * fLargeRadius, Color.yellow);
            Debug.DrawLine(pos, pos + Vector3.right * fLargeRadius, Color.yellow);
            Debug.DrawLine(pos, pos + Vector3.back * fLargeRadius, Color.yellow);

            Debug.DrawLine(pos, pos + Vector3.forward * fLargeRadius, Color.red);
            Debug.DrawLine(pos, pos + Vector3.left * fSmallRadius, Color.red);
            Debug.DrawLine(pos, pos + Vector3.right * fSmallRadius, Color.red);
            Debug.DrawLine(pos, pos + Vector3.back * fSmallRadius, Color.red);
        }

        
        public (GameObject[], GameObject[]) GetDetectedEnemies()
        {
            Vector3 pos = transform.position;
            List<GameObject> largeEnemies = new List<GameObject>();
            List<GameObject> smallEnemies = new List<GameObject>();
            foreach (var enemy in _setEnemiesDetected)
            {
                if (enemy != null)
                {
                    float dist = Vector3.Distance(pos, enemy.transform.position);
                    if (dist < fSmallRadius)
                    {
                        smallEnemies.Add(enemy);
                    }
                    else if (dist < fLargeRadius)
                    {
                        largeEnemies.Add(enemy);
                    }
                }
            }
            return (largeEnemies.ToArray(), smallEnemies.ToArray());
        }


        private void OnDisable()
        {
            _setEnemiesDetected.Clear();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fSmallRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, fLargeRadius);
        }
    }
}