using Enemy;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float fEnemyDetectionRange;
        [SerializeField] private LayerMask lmEnemy;
        private GameObject _enemyDetected; // current weapon detected

        private void Update()
        {
            EnemyDetectionUpdate();
        }

        private void EnemyDetectionUpdate()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, fEnemyDetectionRange, lmEnemy);
            if (hitColliders.Length != 0)
            {
                GameObject enemy = GetMinimumDistanceCollider(hitColliders).gameObject;
                if (_enemyDetected != enemy)
                {
                    enemy.GetComponent<EnemyBehaviour>().OnSelected();
                    if (_enemyDetected != null) _enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = enemy;
                }
            }
            else
            {
                if (_enemyDetected != null)
                {
                    _enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = null;
                }
            }
        }

        /// <summary>
        /// Gets the enemy closest to the player
        /// </summary>
        /// <param name="hitColliders"></param>
        /// <returns></returns>
        private Collider GetMinimumDistanceCollider(Collider[] hitColliders)
        {
            Collider minCollider = hitColliders[0];
            float minimumDistance = float.MaxValue;
            foreach (var coll in hitColliders)
            {
                float distance = Vector3.Distance(coll.gameObject.transform.position, transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    minCollider = coll;
                }
            }
            return minCollider;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fEnemyDetectionRange);
        }
    }
}