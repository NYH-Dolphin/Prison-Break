using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class ThrowWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrowForce = 20f;
        [SerializeField] private float fThrowTime = 3f;

        public override void OnAttack()
        {
            ThrowBehaviour(Pw.GetPlayerVecDir());
        }

        private void ThrowBehaviour(Vector3 facingDir)
        {
            bAttack = true;
            Rb.drag = 0f;
            Rb.angularDrag = 0f;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            Rb.AddForce(facingDir * fThrowForce, ForceMode.Impulse);
            Pc.OnAttackPerformed(weaponInfo.eAttackType);
            StartCoroutine(DestroyCountDown(fThrowTime));
        }

        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}