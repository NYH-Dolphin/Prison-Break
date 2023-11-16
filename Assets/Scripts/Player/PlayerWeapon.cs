using Enemy;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;

namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(LineRenderer))]
    public class PlayerWeapon : MonoBehaviour
    {
        public float lineMultiplier;
        [SerializeField] public Transform tHoldWeaponTransform;
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private LayerMask lmWeapon;
        [SerializeField] private float fEnemyDetectionRange;
        [SerializeField] private float fHandMeleeRange; // without weapon
        [SerializeField] private LayerMask lmEnemy;
        [SerializeField] private GameObject objLobRangeEffect; // effect specifically for lob behaviour

        private LineRenderer _lrDir; // TODO might change the way to indicate the direction
        private GameObject _enemyDetected; // current enemy detected
        private GameObject _weaponSelected; // current weapon detected
        private GameObject _weaponEquipped; // current weapon used
        private InputControls _inputs;


        private void Awake()
        {
            _lrDir = GetComponent<LineRenderer>();
        }


        private void OnEnable()
        {
            if (_inputs == null)
            {
                _inputs = new InputControls();
            }

            _inputs.Gameplay.Weapon.Enable();
            _inputs.Gameplay.Weapon.performed += OnWeaponPerformed;
            _inputs.Gameplay.Attack.Enable();
            _inputs.Gameplay.Attack.performed += OnAttackPerformed;
        }

        private void OnDisable()
        {
            _inputs.Gameplay.Weapon.Disable();
            _inputs.Gameplay.Weapon.performed -= OnWeaponPerformed;
            _inputs.Gameplay.Attack.Disable();
            _inputs.Gameplay.Attack.performed -= OnAttackPerformed;
        }

        private void Start()
        {
            OnCancelDrawWeaponDir();
            OnDisableLobPosition();
        }


        private void Update()
        {
            WeaponDetectionUpdate();
            EnemyDetectionUpdate();
        }


        #region SceneDetection

        private void EnemyDetectionUpdate()
        {
            // TODO may alter the enemy detection, so far only lob need enemy detection
            bool detected = _weaponEquipped != null &&
                            _weaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eAttackType ==
                            AttackType.Lob;


            Collider[] hitColliders = null;
            if (detected) hitColliders = Physics.OverlapSphere(transform.position, fEnemyDetectionRange, lmEnemy);

            if (detected && hitColliders != null && hitColliders.Length != 0)
            {
                GameObject enemy = GetMinimumDistanceCollider(hitColliders).gameObject;
                if (_enemyDetected != enemy && enemy != null)
                {
                    enemy.GetComponent<EnemyBehaviour>().OnSelected();
                    if (_enemyDetected != null) _enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = enemy;
                }
            }
            else
            {
                if (_enemyDetected != null)
                {
                    _enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = null;
                }
            }
        }

        private void WeaponDetectionUpdate()
        {
            // when player is holding the weapon and the weapon is on attack, can not detect the weapon
            bool detected = _weaponEquipped == null ||
                            (_weaponEquipped != null && !_weaponEquipped.GetComponent<WeaponBehaviour>().bAttack);

            Collider[] hitColliders = null;
            if (detected) hitColliders = Physics.OverlapSphere(transform.position, fWeaponGrabRange, lmWeapon);

            if (detected && hitColliders != null && hitColliders.Length != 0)
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

        #endregion


        #region WeaponEquipped

        private void OnWeaponPerformed(InputAction.CallbackContext value)
        {
            // Can not switch weapon during the attack phase
            if (_weaponEquipped != null && _weaponEquipped.GetComponent<WeaponBehaviour>().bAttack) return;
            OnSwitchWeapon();
        }

        private void OnSwitchWeapon()
        {
            if (_weaponSelected != null)
            {
                if (_weaponEquipped != null)
                {
                    FusionCheck();
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

        private void FusionCheck()
        {
            WeaponInfo weaponEquippedInfo = _weaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo;

            // use a basic weapon
            if (!weaponEquippedInfo.bFused)
            {
                WeaponInfo weaponSelectedInfo = _weaponSelected.GetComponent<WeaponBehaviour>().weaponInfo;
                GameObject fusedWeaponPrefab =
                    FusionSystem.Instance.GetFusionWeapon(weaponEquippedInfo, weaponSelectedInfo);
                if (fusedWeaponPrefab != null)
                {
                    GameObject fusedWeapon = Instantiate(fusedWeaponPrefab);
                    Destroy(_weaponSelected);
                    Destroy(_weaponEquipped);
                    _weaponSelected = fusedWeapon;
                }
            }
        }

        private void OnHoldWeapon()
        {
            if (_weaponSelected != null)
            {
                _weaponEquipped = _weaponSelected;
                _weaponEquipped.GetComponent<WeaponBehaviour>().OnHold(this);
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

        public void OnDrawWeaponDir(Vector3 dir)
        {
            _lrDir.positionCount = 2;
            Vector3 startPos = transform.position;
            startPos.y = 1f;
            Vector3 endPos = startPos + dir * lineMultiplier;
            _lrDir.SetPosition(0, startPos);
            _lrDir.SetPosition(1, endPos);
        }

        public void OnCancelDrawWeaponDir()
        {
            _lrDir.positionCount = 0;
        }


        public void OnDisableLobPosition()
        {
            objLobRangeEffect.SetActive(false);
        }

        public void OnDrawLobPosition(Vector3 position)
        {
            objLobRangeEffect.SetActive(true);
            Vector3 pos = position;
            pos.y += 0.1f;
            objLobRangeEffect.transform.position = pos;
        }

        #endregion


        #region Attack

        private void OnAttackPerformed(InputAction.CallbackContext value)
        {
            if (_weaponEquipped != null)
            {
                if (!_weaponEquipped.GetComponent<WeaponBehaviour>().bAttack)
                {
                    _weaponEquipped.GetComponent<WeaponBehaviour>().OnAttack();
                }
            }
            else
            {
                OnAttackWithoutWeapon();
            }
        }


        /// <summary>
        /// Perform Melee Attack
        /// </summary>
        private void OnAttackWithoutWeapon()
        {
        }


        /// <summary>
        /// Get the direction the player face
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayerVecDir()
        {
            return transform.GetComponent<PlayerController>().vecDir;
        }

        public GameObject GetEnemyDetected()
        {
            return _enemyDetected;
        }

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fEnemyDetectionRange);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(tHoldWeaponTransform.position, fHandMeleeRange);
        }


        #region MeleeWeaponDetection

        private void OnTriggerEnter(Collider other)
        {
            GameObject hitbox = GameObject.Find("[Player]/PlayerSprites/Player Hitbox");
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && hitbox.GetComponent<Collider>().enabled)
            {
                other.gameObject.GetComponent<EnemyBehaviour>().OnHit();
                if (_weaponEquipped) _weaponEquipped.GetComponent<WeaponBehaviour>().OnUseMeleeWeapon();
            }
        }

        #endregion
    }
}