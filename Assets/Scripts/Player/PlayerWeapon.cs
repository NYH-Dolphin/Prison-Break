using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private Transform tHoldWeaponTransform;
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, fWeaponGrabRange, lmWeapon);
            if (hitColliders.Length != 0)
            {
                GameObject weapon = GetMinimumDistanceCollider(hitColliders).gameObject;
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
        
        /// <summary>
        /// Gets the closest weapon to the player
        /// </summary>
        /// <param name="hitColliders"></param>
        /// <returns></returns>
        private Collider GetMinimumDistanceCollider(Collider[] hitColliders)
        {
            Collider minCollider = hitColliders[0];
            float minimumDistance = float.MaxValue;
            foreach (var coll in hitColliders)
            {
                float distance = Vector3.Distance(coll.gameObject.transform.position, transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    minCollider = coll;
                }
            }
            return minCollider;
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
                _weaponEquipped.GetComponent<WeaponBehaviour>().OnHold();
                // Attach Weapon Position to User
                Vector3 pos = tHoldWeaponTransform.position;
                _weaponEquipped.transform.position = pos;
                _weaponEquipped.transform.parent = tHoldWeaponTransform;
            }
        }

        private void OnDropWeapon()
        {
            if (_weaponEquipped != null)
            {
                Vector3 dropDir = transform.GetComponent<PlayerController>().vecDir;
                _weaponEquipped.GetComponent<WeaponBehaviour>().OnDrop(dropDir);
                _weaponEquipped = null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);
        }
    }
}