using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class ThrustWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrustTime = 1f;

        public override void OnAttack()
        {
            ThrustBehaviour();
        }

        private void ThrustBehaviour()
        {
            bAttack = true;
            // TODO doesn't contain this function yet
            AudioControl.Instance.PlayThrust();
            Pc.OnAttackPerformed(weaponInfo.eAttackType);
            StartCoroutine(ThrustCountdown(fThrustTime));
        }

        IEnumerator ThrustCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            bAttack = false;
        }
    }
}