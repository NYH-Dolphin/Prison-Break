using System;
using System.Collections;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class BoomerangWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fTime = 0.5f;
        [SerializeField] private float fSpeed = 60f;

        private GameObject _objBoomerangEffect;
        private Vector3 _vecBoomerangDir;
        private bool _bLock;
        private bool _bBack;
        private float _fTimeElapsed;
        private bool _bHit;
        private IEnumerator _thread;
        Vector3 currentEulerAngles;


        public override void OnAttack()
        {
            base.OnAttack();
            BoomerangBehaviour();
            Effect.DisableDirHint();
        }


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
                            _vecBoomerangDir = (hitGround - playerPos).normalized;
                            Effect.DrawDirHint(_vecBoomerangDir);
                        }
                    }
                }
            }
        }

        #region BoomerangBehaviour

        private void BoomerangBehaviour()
        {
            bAttack = true;
            Rb.drag = 0f;
            Rb.angularDrag = 0f;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            AudioControl.Instance.PlayThrow();
            _thread = BoomerangUpdateCor();
            StartCoroutine(_thread);
        }

        IEnumerator BoomerangUpdateCor()
        {
            while (true)
            {
                // Force and Back
                if (!_bBack)
                {
                    _fTimeElapsed += Time.deltaTime;

                    if (_fTimeElapsed < fTime / 2.0f)
                    {
                        Rb.velocity = _vecBoomerangDir * fSpeed;
                        
                    }
                    else
                    {
                        _bBack = true;
                    }
                }
                else
                {
                    Vector3 backVector = Pw.tHoldWeaponTransform.position - transform.position;
                    backVector.y = 0;
                    backVector.Normalize();
                    Quaternion targetRotation = Quaternion.LookRotation(backVector);
                    Rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 3f));
                    Rb.velocity = backVector * fSpeed;

                    if (Vector3.Distance(Pw.tHoldWeaponTransform.position, transform.position) < 1f)
                    {
                        break;
                    }
                }
                currentEulerAngles += new Vector3(0, 0, Time.deltaTime * 500f);
                transform.eulerAngles = currentEulerAngles;
                yield return null;
            }

            OnBoomerangEnd();
        }

        private void OnBoomerangEnd()
        {
            if (_bHit) iDurability -= 1;
            if (iDurability == 0)
            {
                Destroy(gameObject);
            }
            else if (iDurability > 0)
            {
                // Refresh all the attributes
                bAttack = false;
                gameObject.transform.position = Pw.tHoldWeaponTransform.position;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                _bBack = false;
                _fTimeElapsed = 0f;
                _bHit = false;
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (bAttack)
            {
                // TODO check the enemy has dead
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
                }

                bool obstacle = other.gameObject.layer == LayerMask.NameToLayer("Obstacle");
                if (obstacle)
                {
                    _bHit = true;
                    if (_thread != null)
                    {
                        StopCoroutine(_thread);
                        _thread = null;
                    }
                    OnBoomerangEnd();
                }
            }
        }
    }
}