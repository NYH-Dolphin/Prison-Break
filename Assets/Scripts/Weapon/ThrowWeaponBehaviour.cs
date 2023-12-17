using System.Collections;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class ThrowWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrowForce = 20f;
        [SerializeField] private float fThrowTime = 1f;

        private Vector3 _vecThrowDir;
        private bool _bHit;
        private IEnumerator _thread;

        private void Update()
        {
            // Player Hold the Weapon, start detection
            if (Pw != null)
            {
                if (!bAttack)
                {
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lmGround))
                        {
                            // The ray hit an object on the ground layer
                            Vector3 hitGround = hit.point;
                            hitGround.y = 0f;
                            Vector3 playerPos = Pc.transform.position;
                            playerPos.y = 0f;
                            _vecThrowDir = (hitGround - playerPos).normalized;
                            Effect.DrawDirHint(_vecThrowDir);
                        }
                    }
                }
            }
        }

        public override void OnAttack()
        {
            base.OnAttack();
            Effect.DisableDirHint();
            ThrowBehaviour(_vecThrowDir);
        }


        private void ThrowBehaviour(Vector3 facingDir)
        {
            bAttack = true;
            Coll.enabled = false;
            Coll.enabled = true;
            AudioControl.Instance.PlayThrow();
            Rb.drag = 0f;
            Rb.angularDrag = 0f;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            Rb.AddForce(facingDir * fThrowForce, ForceMode.Impulse);
            _thread = DestroyCountDown(fThrowTime);
            StartCoroutine(_thread);
        }


        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            OnThrowEnd();
        }

        void OnThrowEnd()
        {
            iDurability -= 1;
            if (iDurability == 0)
            {
                Destroy(gameObject);
            }
            else if (iDurability > 0)
            {
                // Refresh all the attributes
                bAttack = false;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                gameObject.transform.position = Pw.tHoldWeaponTransform.position;
                _bHit = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (bAttack)
            {
                bool enemy = other.gameObject.layer == LayerMask.NameToLayer("Enemy") &&
                             !setEnemyAttacked.Contains(other.gameObject);

                if (enemy)
                {
                    _bHit = true;
                    setEnemyAttacked.Add(other.gameObject);
                    if (weaponInfo.eSharpness == Sharpness.Blunt)
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                    }
                    else
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, false);
                    }
                    Score.Instance.Attack(gameObject);
                    // once hit the enemy, throw weapon will be ereased
                    if (_thread != null)
                    {
                        StopCoroutine(_thread);
                        _thread = null;
                    }

                    OnThrowEnd();
                }

                bool obstacle = other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("Blocking");
                if (obstacle)
                {
                    _bHit = true;
                    if (_thread != null)
                    {
                        StopCoroutine(_thread);
                        _thread = null;
                    }

                    OnThrowEnd();
                }
            }
        }
    }
}