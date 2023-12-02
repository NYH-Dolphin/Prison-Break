using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class SlamWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fSlamTime = 1f;

        public override void OnAttack()
        {
            base.OnAttack();
            SlamBehaviour();
        }

        private void SlamBehaviour()
        {
            bAttack = true;
            // TODO doesn't contain this function yet
            // AudioControl.Instance.PlaySlam();
            StartCoroutine(SlamCountdown(fSlamTime));
        }

        IEnumerator SlamCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            bAttack = false;
        }
    }
}