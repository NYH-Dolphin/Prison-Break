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
        
        Vector3 _startPosition;
        Vector3 _targetPosition;
        Vector3 _midPosition;

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
            _startPosition = transform.position;
            _targetPosition = _vecHitPos;

            Collider[] enemiesInsideArea = Physics.OverlapSphere(_targetPosition, radius);
            foreach (var col in enemiesInsideArea)
            {
                if (col.gameObject.tag == "Enemy") {
                    _targetPosition = col.gameObject.transform.position;
                    _targetPosition.y += 1f;
                    Coll.enabled = true;
                    break;
                }
            }

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
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, false);
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