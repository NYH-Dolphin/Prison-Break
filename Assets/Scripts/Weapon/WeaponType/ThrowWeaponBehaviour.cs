﻿using System.Collections;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class ThrowWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrowForce = 20f;
        [SerializeField] private float fThrowTime = 1f;

        private Vector3 _vecThrowDir;
        private bool _bLock;

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
                else
                {
                    Effect.DisableDirHint();
                }
            }

            if (iDurability == 0)
            {
                Destroy(gameObject);
            }
        }
        
        public override void OnAttack()
        {
            base.OnAttack();
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
            _bLock = true;

            StartCoroutine(DestroyCountDown(fThrowTime));
        }


        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);

            if (_bLock)
            {
                _bLock = false;
                iDurability -= 1;
                if (iDurability == 0)
                {
                    Destroy(gameObject);
                }
            }

            if (iDurability > 0)
            {
                bAttack = false;
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                gameObject.transform.position = Pw.tHoldWeaponTransform.position;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (bAttack && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(weaponInfo.eSharpness == Sharpness.Blunt)
                {
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                    iDurability -= 1;
                }
                    
                else
                {
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, false);
                    iDurability -= 1;
                }
                    
            }
            else if (bAttack && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") &&
                     Rb.velocity != Vector3.zero)
            {
                if (_bLock)
                {
                    _bLock = false;
                    iDurability -= 1;
                }
            }
        }
    }
}