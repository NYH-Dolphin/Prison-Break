using System;
using System.Collections;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class LobWeaponBehaviour : WeaponBehaviour
    {
        // maximum distance between the player and the enemy, so the lob weapon will tracking the enemy's position
        [SerializeField] private float fMaxDistance = 10f;
        [SerializeField] private float fTime = 0.5f; // Lobbing time
        [SerializeField] private LayerMask lmGround;
        [SerializeField] private float arcHeight;

        private Vector3 _vecHitPos;
        private bool _bLock;
        
        Vector3 startPosition;
        Vector3 targetPosition;
        Vector3 midPosition;

        public float radius;


        public override void OnAttack()
        {
            LobBehaviour();
        }


        private void Update()
        {
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
                            Vector3 objPos = transform.position;
                            // draw the lob position with a specific range
                            if ((hitGround - objPos).magnitude < fMaxDistance)
                            {
                                _vecHitPos = hitGround;
                                Pw.OnDrawLobPosition(hitGround);
                            }
                            else
                            {
                                Vector3 dir = Vector3.Normalize(hitGround - objPos);
                                Vector3 maxPos = objPos + dir * fMaxDistance;
                                _vecHitPos = maxPos;
                                Pw.OnDrawLobPosition(maxPos);
                            }
                        }
                    }
                }
            }

            if(gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                int weapLayer = LayerMask.NameToLayer("Weapon");
                gameObject.layer = weapLayer;
            }
        }

        private void LobBehaviour()
        {
            bAttack = true;
            Coll.enabled = false;
            //Coll.enabled = true;
            AudioControl.Instance.PlayLob();
            Pw.OnDisableLobPosition();
            _bLock = true;

            iTween.Init(gameObject);
            Vector3[] path = new Vector3[3];
            startPosition = transform.position;
            targetPosition = _vecHitPos;

            Collider[] enemiesInsideArea = Physics.OverlapSphere(targetPosition, radius);
            foreach (var col in enemiesInsideArea)
            {
                if (col.gameObject.tag == "Enemy") {
                    targetPosition = col.gameObject.transform.position;
                    targetPosition.y += 1f;
                    Coll.enabled = true;
                    break;
                }
            }

            midPosition = (startPosition + targetPosition) / 2.0f;
            midPosition.y += arcHeight;
            path[0] = startPosition;
            path[1] = midPosition;
            path[2] = targetPosition;
            Hashtable args = new Hashtable();
            args.Add("position", targetPosition);
            args.Add("path", path);
            args.Add("time", fTime);
            args.Add("easetype", iTween.EaseType.easeOutQuart);
            iTween.MoveTo(gameObject, args);
            StartCoroutine(DestroyCountDown(fTime)); // Start this countdown in case weapon doesn't hit the enemy
        }



        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time - 0.2f);

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

        /// <summary>
        /// Drop with a specific direction
        /// </summary>
        /// <param name="dropDir"></param>
        public override void OnDrop(Vector3 dropDir)
        {
            Pw.OnDisableLobPosition();
            base.OnDrop(dropDir);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (bAttack && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(weaponInfo.eSharpness == Sharpness.Blunt)
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                else
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHit();
            }
            else if (bAttack && other.gameObject.layer == LayerMask.NameToLayer("Obstacle") &&
                     Rb.velocity != Vector3.zero)
            {
                if (_bLock)
                {
                    IDurability -= 1;
                    if (IDurability == 0)
                    {
                        Destroy(gameObject);
                    }

                    _bLock = false;
                }
            }
        }
    }
}