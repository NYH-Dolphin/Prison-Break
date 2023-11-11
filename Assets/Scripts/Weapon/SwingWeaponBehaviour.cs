using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class SwingWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fSwingTime = 1f;

        public override void OnAttack()
        {
            SwingBehaviour();
        }

        private void SwingBehaviour()
        {
            bAttack = true;
            Pc.OnAttackPerformed(weaponInfo.eAttackType);
            StartCoroutine(DestroyCountDown(fSwingTime));
        }

        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}