using UnityEngine;

namespace Weapon
{
    public class LobWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField]
        private float
            fMaxDistance =
                10f; // maximum distance between the player and the enemy, so the lob weapon will tracking the enemy's position

        [SerializeField] private float fSpeed = 30f;

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
        }
    }
}