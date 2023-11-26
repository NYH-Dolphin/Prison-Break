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


        public (GameObject[], GameObject[]) GetDetectedEnemies()
        {
            Vector3 pos = transform.position;
            Collider[] largeHitColliders = Physics.OverlapSphere(pos, fLargeRadius, lmEnemy);
            Collider[] smallHitColliers = Physics.OverlapSphere(pos, fSmallRadius, lmEnemy);
            HashSet<GameObject> largeHitCollidersSet = largeHitColliders.Select(coll => coll.gameObject).ToHashSet();
            HashSet<GameObject> smallHitCollidersSet = smallHitColliers.Select(coll => coll.gameObject).ToHashSet();
            List<GameObject> large = new();
            foreach (var obj in largeHitCollidersSet)
            {
                if (!smallHitCollidersSet.Contains(obj))
                {
                    large.Add(obj);
                }
            }
            return (large.ToArray(), smallHitCollidersSet.ToArray());
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