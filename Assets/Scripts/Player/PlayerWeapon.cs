using Enemy;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private GameObject objHitBox;

        [Header("Holding Weapon")] [SerializeField]
        public Transform tHoldWeaponTransform;

        [SerializeField] [Range(0, 1)] private float fHoldWeaponScale;

        [Header("Shiv Attack without Weapon")] [SerializeField]
        private float fShivTime;

        private bool _bShivAttack;

        [Header("Sprint")] [SerializeField] private float fSprintDetectionRange;
        [SerializeField] private float fSprintDistance;
        [SerializeField] [Range(0.01f, 0.5f)] private float fSprintTime;

        [Header("Weapon Attack Effects")] [SerializeField]
        private GameObject objLobRange;

        private GameObject _objLobRangeSprite;
        [SerializeField] private float fDirLineLength;

        // private properties
        private PlayerWeaponEffect _effect;
        private LineRenderer _lrDir; // TODO might change the way to indicate the direction
        private GameObject _enemyDetected;
        private GameObject _weaponSelected;
        public GameObject WeaponEquipped { get; private set; }

        private InputControls _inputs;
        private PlayerController _pc;


        private void Awake()
        {
            _lrDir = GetComponent<LineRenderer>();
            _pc = GetComponent<PlayerController>();
            _objLobRangeSprite = objLobRange.transform.GetChild(0).gameObject;
            _effect = GetComponent<PlayerWeaponEffect>();
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
            DisableWeaponEffects();
        }


        private void Update()
        {
            WeaponEffectUpdate();
            WeaponDetectionUpdate();
            EnemyDetectionUpdate();
        }


        #region SceneDetection

        // Current weapon behaviour no longer need enemy detection
        private void EnemyDetectionUpdate()
        {
            Collider[] hitColliders;
            hitColliders = Physics.OverlapSphere(transform.position, fEnemyDetectionRange, lmEnemy);

            if (hitColliders != null && hitColliders.Length != 0)
            {
                GameObject enemy = GetMinimumDistanceCollider(hitColliders).gameObject;
                if (_enemyDetected != enemy && enemy != null)
                {
                    // enemy.GetComponent<EnemyBehaviour>().OnSelected();
                    //if (_enemyDetected != null) _enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = enemy;
                }
            }
            else
            {
                if (_enemyDetected != null)
                {
                    //_enemyDetected.GetComponent<EnemyBehaviour>().OnNotSelected();
                    _enemyDetected = null;
                }
            }
        }

        private void WeaponDetectionUpdate()
        {
            // when player is holding the weapon and the weapon is on attack, can not detect the weapon
            bool detected = WeaponEquipped == null ||
                            (WeaponEquipped != null && !WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack);

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
            if (WeaponEquipped != null && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack) return;
            OnSwitchWeapon();
        }

        private void OnSwitchWeapon()
        {
            if (_weaponSelected != null)
            {
                if (WeaponEquipped != null)
                {
                    FusionCheck();
                    OnDropWeapon();
                }

                OnHoldWeapon();
            }
            else
            {
                if (WeaponEquipped != null)
                {
                    OnDropWeapon();
                }
            }
        }

        private void FusionCheck()
        {
            WeaponInfo weaponEquippedInfo = WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo;
            // use a basic weapon
            if (!weaponEquippedInfo.bFused)
            {
                GameObject fusedWeapon = FusionSystem.Instance.GetFusionWeapon(WeaponEquipped, _weaponSelected);
                if (fusedWeapon != null)
                {
                    Destroy(_weaponSelected);
                    Destroy(WeaponEquipped);
                    _weaponSelected = fusedWeapon;
                }
            }
        }

        private void OnHoldWeapon()
        {
            if (_weaponSelected != null)
            {
                WeaponEquipped = _weaponSelected;
                WeaponEquipped.GetComponent<WeaponBehaviour>().OnHold(this);
                Vector3 pos = tHoldWeaponTransform.position;
                WeaponEquipped.transform.position = pos;
                WeaponEquipped.transform.parent = tHoldWeaponTransform;
                WeaponEquipped.transform.localScale *= fHoldWeaponScale;
            }
        }

        private void OnDropWeapon()
        {
            if (WeaponEquipped != null)
            {
                WeaponEquipped.transform.localScale *= 1 / fHoldWeaponScale;
                Vector3 dropDir = transform.GetComponent<PlayerController>().VecDir;
                WeaponEquipped.GetComponent<WeaponBehaviour>().OnDrop(dropDir);
                WeaponEquipped = null;
            }
        }

        #endregion


        #region WeaponEffect

        private void WeaponEffectUpdate()
        {
            if (WeaponEquipped == null)
            {
                DisableWeaponEffects();
            }
        }

        void DisableWeaponEffects()
        {
            OnCancelDrawWeaponDir();
        }

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

        
        
        public (GameObject[], GameObject[]) OnGetLobRangeEnemy()
        {
            return objLobRange.GetComponent<LobRangeWeaponEffect>().GetDetectedEnemies();
        }


        public void DevShowLobRange()
        {
            objLobRange.GetComponent<LobRangeWeaponEffect>().ShowLobRange();
        }

        #endregion
        

        #endregion


        #region Attack

        private void OnAttackPerformed(InputAction.CallbackContext value)
        {
            if (WeaponEquipped != null)
            {
                if (!WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack)
                {
                    WeaponEquipped.GetComponent<WeaponBehaviour>().OnAttack();
                }
            }
            else
            {
                if (!_bShivAttack)
                {
                    OnAttackWithoutWeapon();
                }
            }

            SprintIn();
        }

        private void SprintIn()
        {
            if (_enemyDetected != null)
            {
                if (WeaponEquipped == null ||
                    WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee)
                {
                    Vector3 playerPos = transform.position;
                    Vector3 enemyPos = _enemyDetected.transform.position;
                    float dist = Vector3.Distance(playerPos, enemyPos);
                    if (dist < fSprintDetectionRange)
                    {
                        Vector3 dir = (enemyPos - playerPos).normalized;
                        dir.y = 0;
                        // if the desire position exceed the enemy position, will simply use enemy position
                        Vector3 targetPos = playerPos + dir * fSprintDistance;
                        if (dist < Vector3.Distance(playerPos, targetPos))
                        {
                            targetPos = enemyPos;
                        }

                        _pc.SprintMove(targetPos, fSprintTime);
                    }
                }
            }
        }

        /// <summary>
        /// Perform Melee Attack
        /// </summary>
        private void OnAttackWithoutWeapon()
        {
            setEnemyAttackedWithoutWeapon = new();
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

        private HashSet<GameObject> setEnemyAttackedWithoutWeapon;

        // the trigger enter will be calculate when the hit box is activated
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;

            bool meleeWeapon = WeaponEquipped
                               && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack
                               && WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee;

            bool meleeWithoutWeapon = !WeaponEquipped && objHitBox.GetComponent<Collider>().enabled &&
                                      other.gameObject.layer == LayerMask.NameToLayer("Enemy");
            
            if (meleeWeapon)
            {
                // check the whether the enemy has been attacked yet
                if (!WeaponEquipped.GetComponent<WeaponBehaviour>().setEnemyAttacked.Contains(other.gameObject))
                {
                    other.GetComponent<Knockback>().PlayFeedback(_pc.VecDir.normalized);
                    WeaponEquipped.GetComponent<WeaponBehaviour>().setEnemyAttacked.Add(other.gameObject);
                    if (WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Blunt)
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                    else
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, true);
                }

                WeaponEquipped.GetComponent<WeaponBehaviour>().OnUseMeleeWeapon();
            }
            else if (meleeWithoutWeapon)
            {
                if (!setEnemyAttackedWithoutWeapon.Contains(other.gameObject))
                {
                    setEnemyAttackedWithoutWeapon.Add(other.gameObject);
                    other.gameObject.GetComponent<EnemyBehaviour>()?.OnHit(1, false);
                }
            }
        }

        #endregion
    }
}