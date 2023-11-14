using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class LobWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fMaxDistance;
        [SerializeField] private float fSpeed;

        private Transform _tEnemy;


        public override void OnAttack()
        {
            if (Pw.GetEnemyDetected() != null)
            {
                LobBehaviour();
            }
        }


        private void Update()
        {
            if (bAttack)
            {
                float distance = (_tEnemy.position - Pw.transform.position).magnitude;
                if (distance < fMaxDistance)
                {
                    Vector3 dir = Vector3.Normalize(_tEnemy.position - transform.position);
                    transform.position += dir * (fSpeed * Time.deltaTime);
                }
                else
                {
                    Destroy(this);
                }
            }
        }

        private void LobBehaviour()
        {
            bAttack = true;
            _tEnemy = Pw.GetEnemyDetected().transform;
            AudioControl.Instance.PlayLob();
            iTween.Init(gameObject);
            //StartCoroutine(DestroyCountDown(0.6f)); // Start this countdown in case weapon doesn't hit the enemy
        }

        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}