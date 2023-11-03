using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private LayerMask lmWeapon;

        private GameObject _weaponSelected; // current weapon detected
        private GameObject _weaponEquipped; // current weapon used
        private InputControls _inputs;


        private void OnEnable()
        {
            if (_inputs == null)
            {
                _inputs = new InputControls();
            }

            _inputs.Gameplay.Weapon.Enable();
            _inputs.Gameplay.Weapon.performed += OnWeaponPerformed;
        }

        private void OnDisable()
        {
            _inputs.Gameplay.Weapon.Disable();
            _inputs.Gameplay.Weapon.performed -= OnWeaponPerformed;
        }


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

        private void OnWeaponPerformed(InputAction.CallbackContext value)
        {
            OnSwitchWeapon();
        }

        private void OnSwitchWeapon()
        {
            if (_weaponSelected != null)
            {
                if (_weaponEquipped != null)
                {
                    OnDropWeapon();
                }

                OnHoldWeapon();
            }
            else
            {
                if (_weaponEquipped != null)
                {
                    OnDropWeapon();
                }
            }
        }

        private void OnHoldWeapon()
        {
            if (_weaponSelected != null)
            {
                _weaponEquipped = _weaponSelected;
                _weaponEquipped.layer = LayerMask.NameToLayer("Player");
                _weaponEquipped.GetComponent<WeaponBehaviour>().OnNotSelected();
                Vector3 pos = transform.position;
                pos.y = _weaponEquipped.transform.position.y;
                _weaponEquipped.transform.position = pos;
                _weaponEquipped.transform.parent = transform;
            }
        }

        private void OnDropWeapon()
        {
            if (_weaponEquipped != null)
            {
                _weaponEquipped.layer = LayerMask.NameToLayer("Weapon");
                _weaponEquipped.transform.parent = GameObject.Find("[Weapon]").transform;
                _weaponEquipped = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);
        }
    }
}