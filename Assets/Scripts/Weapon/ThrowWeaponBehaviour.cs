using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class ThrowWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrowForce = 20f;

        public override void OnAttack(Transform startTransform, Transform targetTransform, Vector3 facingDir)
        {
            BAttack = true;
            Rb.drag = 0f;
            Rb.angularDrag = 0f;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            Rb.AddForce(facingDir * fThrowForce, ForceMode.Impulse);
            StartCoroutine(DestroyCountDown(5f));
        }
        
        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}