using UnityEngine;
using Weapon;

namespace Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private LayerMask lmWeapon;

        private GameObject _weaponSelected;


        private void Update()
        {
            WeaponDetectionUpdate();
        }


        private void WeaponDetectionUpdate()
        {
            Collider[] hitColliders = new Collider[1];
            int num = Physics.OverlapSphereNonAlloc(transform.position, fWeaponGrabRange, hitColliders, lmWeapon);
            if (num != 0)
            {
                GameObject weapon = hitColliders[0].gameObject;
                if (_weaponSelected != weapon)
                {
                    weapon.GetComponent<WeaponBehaviour>().OnSelected();
                    if (_weaponSelected != null) _weaponSelected.GetComponent<WeaponBehaviour>().OnNotSelected();
                    _weaponSelected = weapon;
                }
            }
            else
            {
                if (_weaponSelected != null)
                {
                    _weaponSelected.GetComponent<WeaponBehaviour>().OnNotSelected();
                    _weaponSelected = null;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);
        }
    }
}