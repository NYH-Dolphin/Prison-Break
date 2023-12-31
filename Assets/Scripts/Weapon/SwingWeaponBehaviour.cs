﻿using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class SwingWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fSwingTime = 1f;

        public override void OnAttack()
        {
            base.OnAttack();
            SwingBehaviour();
        }

        private void SwingBehaviour()
        {
            bAttack = true;
            AudioControl.Instance.PlaySwing();
            StartCoroutine(SwingCountdown(fSwingTime));
        }

        IEnumerator SwingCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            bAttack = false;
        }
    }
}