using System;
using UnityEngine;

namespace Weapon
{
    public class BoomerangWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fDistance = 10f;
        [SerializeField] private float fTime = 0.5f;
        [SerializeField] private LayerMask lmGround;
        
        private Vector3 _vecThrowDir;
        private bool _bLock;
        
        public override void OnAttack()
        {
            BoomerangBehaviour();
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
                            _vecThrowDir = (hitGround - playerPos).normalized;
                            Pw.OnDrawWeaponDir(_vecThrowDir);
                        }
                    }
                }
                else
                {
                    Pw.OnCancelDrawWeaponDir();
                }
            }
        }

        private void BoomerangBehaviour()
        {
            throw new System.NotImplementedException();
        }
    }
}