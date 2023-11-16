using System;
using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class LobWeaponBehaviour : WeaponBehaviour
    {
        // maximum distance between the player and the enemy, so the lob weapon will tracking the enemy's position
        [SerializeField] private float fMaxDistance = 10f; 
        [SerializeField] private float fTime = 0.5f; // Lobbing time
        [SerializeField] private LayerMask lmGround;

        private Vector3 _vecHitPos;
        private Transform _tEnemy;


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
            AudioControl.Instance.PlayLob();
            Pw.OnDisableLobPosition();
            
            iTween.Init(gameObject);
            Vector3[] path = new Vector3[3];
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = _vecHitPos;
            Vector3 midPosition = (startPosition + targetPosition) / 2.0f;
            midPosition.y += 1f;
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
            yield return new WaitForSeconds(time-0.2f);
            Destroy(gameObject);
        }
    }
}