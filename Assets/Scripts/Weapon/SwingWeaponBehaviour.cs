using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class SwingWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fSwingTime = 1f;

        private AudioControl SFX;
        //public string weaponName;

        void Start()
        {
            SFX = GameObject.Find("AudioController").GetComponent<AudioControl>();
        }

        public override void OnAttack()
        {
            SwingBehaviour();
            SFX.PlaySwing();
        }

        private void SwingBehaviour()
        {
            bAttack = true;
            Pc.OnAttackPerformed(weaponInfo.eAttackType);
            StartCoroutine(SwingCountdown(fSwingTime));
        }

        IEnumerator SwingCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            bAttack = false;
        }
    }
}