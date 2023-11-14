using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class ThrowWeaponBehaviour : WeaponBehaviour
    {
        [SerializeField] private float fThrowForce = 20f;
        [SerializeField] private float fThrowTime = 3f;
        [SerializeField] private LayerMask lmGround;

        private Vector3 _vecThrowDir;

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

        public override void OnAttack()
        {
            ThrowBehaviour(_vecThrowDir);
        }


        private void ThrowBehaviour(Vector3 facingDir)
        {
            bAttack = true;
            AudioControl.Instance.PlayThrow();
            
            Pc.OnSetAttackDir(_vecThrowDir);
            Pc.OnAttackPerformed(weaponInfo.eAttackType);

            Rb.drag = 0f;
            Rb.angularDrag = 0f;
            Rb.constraints = RigidbodyConstraints.FreezePositionY;
            Rb.AddForce(facingDir * fThrowForce, ForceMode.Impulse);

            StartCoroutine(DestroyCountDown(fThrowTime));
        }

        IEnumerator DestroyCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}