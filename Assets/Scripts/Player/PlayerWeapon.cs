using Enemy;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using System.Collections;
using Weapon.Effects;

namespace Player
{
    [RequireComponent(typeof(PlayerController), typeof(LineRenderer))]
    public class PlayerWeapon : MonoBehaviour
    {
        [Header("Basic Component")] [SerializeField]
        private Animator animator;

        [Header("Weapon and Enemy Detect")] [SerializeField]
        private LayerMask lmWeapon;

        [SerializeField] private LayerMask lmEnemy;
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private float fEnemyDetectionRange;

        [Header("Holding Weapon")] [SerializeField]
        public Transform tHoldWeaponTransform;

        [SerializeField] [Range(0, 1)] private float fHoldWeaponSacle;

        [Header("Shiv Attack without Weapon")] [SerializeField]
        private float fShivTime;

        private bool _bShivAttack;

        [Header("Weapon Attack Effects")] [SerializeField]
        private GameObject objLobRange;

        private GameObject _objLobRangeSprite;
        [SerializeField] private GameObject objHitBox;
        [SerializeField] private float fDirLineLength;

        // private properties
        private LineRenderer _lrDir; // TODO might change the way to indicate the direction
        private GameObject _enemyDetected;
        private GameObject _weaponSelected;
        private GameObject _weaponEquipped;
        private InputControls _inputs;
        private PlayerController _pc;


        private void Awake()
        {
            _lrDir = GetComponent<LineRenderer>();
            _pc = GetComponent<PlayerController>();
            _objLobRangeSprite = objLobRange.transform.GetChild(0).gameObject;
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
            // EnemyDetectionUpdate();
        }


        #region SceneDetection

        // Current weapon behaviour no longer need enemy detection
        private void EnemyDetectionUpdate()
        {
            // TODO the function of detection need to be re-designed
            bool detected = true;

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
                GameObject fusedWeapon = FusionSystem.Instance.GetFusionWeapon(_weaponEquipped, _weaponSelected);
                if (fusedWeapon != null)
                {
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
                Vector3 pos = tHoldWeaponTransform.position;
                _weaponEquipped.transform.position = pos;
                _weaponEquipped.transform.parent = tHoldWeaponTransform;
                _weaponEquipped.transform.localScale *= fHoldWeaponSacle;
            }
        }

        private void OnDropWeapon()
        {
            if (_weaponEquipped != null)
            {
                _weaponEquipped.transform.localScale *= 1 / fHoldWeaponSacle;
                Vector3 dropDir = transform.GetComponent<PlayerController>().VecDir;
                _weaponEquipped.GetComponent<WeaponBehaviour>().OnDrop(dropDir);
                _weaponEquipped = null;
            }
        }

        #endregion


        #region WeaponDirectionEffect

        public void OnDrawWeaponDir(Vector3 dir)
        {
            _lrDir.positionCount = 2;
            Vector3 startPos = transform.position;
            startPos.y = 1f;
            Vector3 endPos = startPos + dir * fDirLineLength;
            _lrDir.SetPosition(0, startPos);
            _lrDir.SetPosition(1, endPos);
        }

        public void OnCancelDrawWeaponDir()
        {
            _lrDir.positionCount = 0;
        }

        #endregion


        #region LobRangeEffect

        public void OnDisableLobPosition()
        {
            _objLobRangeSprite.SetActive(false);
        }

        public void OnDrawLobPosition(Vector3 position)
        {
            _objLobRangeSprite.SetActive(true);
            Vector3 pos = position;
            pos.y += 0.1f;
            objLobRange.transform.position = pos;
        }

        public (GameObject[], GameObject[]) OnGetLobRangeEnemy()
        {
           
            return objLobRange.GetComponent<LobRangeWeaponEffect>().GetDetectedEnemies();
        }

        // TODO developer only
        public void DevShowLobRange()
        {
            objLobRange.GetComponent<LobRangeWeaponEffect>().ShowLobRange(); 
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
                if (!_bShivAttack)
                {
                    OnAttackWithoutWeapon();
                }
            }
        }


        /// <summary>
        /// Perform Melee Attack
        /// </summary>
        private void OnAttackWithoutWeapon()
        {
            ShivBehaviour();
        }

        private void ShivBehaviour()
        {
            _bShivAttack = true;
            AudioControl.Instance.PlaySwing(); //TODO change to shiv
            animator.SetTrigger("Swing"); //TODO change to shiv
            StartCoroutine(ShivCountdown(fShivTime));
        }

        IEnumerator ShivCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            _bShivAttack = false;
        }

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fEnemyDetectionRange);
        }


        #region MeleeWeaponDetection

        private void OnTriggerEnter(Collider other)
        {
            bool detected = _weaponEquipped != null
                            && _weaponEquipped.GetComponent<WeaponBehaviour>().bAttack
                            && _weaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee;
            if (detected && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.GetComponent<Knockback>().PlayFeedback(_pc.VecDir.normalized);
                if (_weaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Blunt)
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                else
                    other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, true);

                if (_weaponEquipped) _weaponEquipped.GetComponent<WeaponBehaviour>().OnUseMeleeWeapon();
            }
            else
            {
                if (objHitBox.GetComponent<Collider>().enabled &&
                    other.gameObject.layer == LayerMask.NameToLayer("Enemy") && _weaponEquipped == null)
                {
                    if (other != null)
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHit(1, false);
                    }
                }
            }
        }

        #endregion
    }
}