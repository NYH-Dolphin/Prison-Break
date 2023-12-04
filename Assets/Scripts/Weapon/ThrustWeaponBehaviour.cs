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
            base.OnAttack();
            bAttack = true;
            AudioControl.Instance.PlayThrust();
            StartCoroutine(ThrustCountdown(fThrustTime));
        }

        IEnumerator ThrustCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            bAttack = false;
        }
    }
}