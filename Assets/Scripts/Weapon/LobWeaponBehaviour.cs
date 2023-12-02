using System.Collections;
using System.Collections.Generic;
using Enemy;
using MyCameraEffect;
using Player;
using UnityEngine;

namespace Weapon
{
    public class LobWeaponBehaviour : WeaponBehaviour
    {
        // maximum distance between the player and the enemy, so the lob weapon will tracking the enemy's position
        [SerializeField] private float fMaxDistance = 10f;
        [SerializeField] private float fTime = 0.5f; // Lobbing time
        [SerializeField] private float arcHeight;

        private Vector3 _hitPos;
        private bool _bLock;
        private HashSet<GameObject> _setLobEnemies = new();
        
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector3 _midPosition;
        private PlayerWeaponEffect _effect;


        public override void OnAttack()
        {
            base.OnAttack();
            LobBehaviour();
        }


        private void Update()
        {
            if (Pw != null)
            {
                // TODO Development only
                // Pw.DevShowLobRange();
                
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
                            Vector3 objPos = transform.position;
                            // draw the lob position with a specific range
                            if ((hitGround - objPos).magnitude < fMaxDistance)
                            {
                                _hitPos = hitGround;
                                _hitPos.y = 0.01f;
                                Effect.DrawLobPosition(hitGround);
                            }
                            else
                            {
                                Vector3 dir = Vector3.Normalize(hitGround - objPos);
                                Vector3 maxPos = objPos + dir * fMaxDistance;
                                _hitPos = maxPos;
                                _hitPos.y = 0.01f;
                                Effect.DrawLobPosition(maxPos);
                            }
                        }
                    }
                }
            }
        }

        private void LobBehaviour()
        {
            bAttack = true;
            Coll.enabled = false;
            AudioControl.Instance.PlayLob();
            Effect.DisableLobPosition();
            _bLock = true;
            _setLobEnemies.Clear();

            iTween.Init(gameObject);
            Vector3[] path = new Vector3[3];
            _startPosition = transform.position;
            _targetPosition = _hitPos;
            _midPosition = (_startPosition + _targetPosition) / 2.0f;
            _midPosition.y += arcHeight;
            path[0] = _startPosition;
            path[1] = _midPosition;
            path[2] = _targetPosition;
            Hashtable args = new Hashtable();
            args.Add("position", _targetPosition);
            args.Add("path", path);
            args.Add("time", fTime);
            args.Add("easetype", iTween.EaseType.easeOutQuart);
            iTween.MoveTo(gameObject, args);
            StartCoroutine(DestroyCountDown(fTime)); // Start this countdown in case weapon doesn't hit the enemy
        }

        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time - 0.2f);

            // Calculate the enemy in the range and apply damage to them
            RangeEffectCalculation();
            // Play the effect in the end, a little before the death calculation
            Effect.PlayLobEffect(_targetPosition);
            CameraEffect.Instance.GenerateImpulse();

            yield return new WaitForSeconds(0.1f);


            if (_bLock)
            {
                _bLock = false;
                IDurability -= 1;
                if (IDurability == 0)
                {
                    Destroy(gameObject);
                }
            }

            if (IDurability > 0)
            {
                bAttack = false;
                iTween.Stop();
                Rb.constraints = RigidbodyConstraints.FreezeAll;
                gameObject.transform.position = Pw.tHoldWeaponTransform.position;
            }
        }


        void RangeEffectCalculation()
        {
            (GameObject[] largeRangeEnemies, GameObject[] smallRangeEnemies) = Pw.OnGetLobRangeEnemy();
            foreach (var enemies in largeRangeEnemies)
            {
                if (!_setLobEnemies.Contains(enemies))
                {
                    _setLobEnemies.Add(enemies);
                    enemies.GetComponent<EnemyBehaviour>().OnHitBlunt();
                }
            }

            foreach (var enemies in smallRangeEnemies)
            {
                if (!_setLobEnemies.Contains(enemies))
                {
                    _setLobEnemies.Add(enemies);
                    if (weaponInfo.eSharpness == Sharpness.Blunt)
                    {
                        enemies.GetComponent<EnemyBehaviour>().OnHitBlunt();
                    }
                    else
                    {
                        enemies.GetComponent<EnemyBehaviour>().OnHit(2, false);
                    }
                }
            }
        }

        /// <summary>
        /// Drop with a specific direction
        /// </summary>
        /// <param name="dropDir"></param>
        public override void OnDrop(Vector3 dropDir)
        {
            Effect.DisableLobPosition();
            base.OnDrop(dropDir);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (bAttack && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") &&
                Rb.velocity != Vector3.zero)
            {
                if (_bLock)
                {
                    _bLock = false;

                    IDurability -= 1;
                    if (IDurability == 0)
                    {
                        Destroy(gameObject);
                    }

                    if (IDurability > 0)
                    {
                        bAttack = false;
                        iTween.Stop();
                        Rb.constraints = RigidbodyConstraints.FreezeAll;
                        gameObject.transform.position = Pw.tHoldWeaponTransform.position;
                    }
                }
            }
        }
    }
}