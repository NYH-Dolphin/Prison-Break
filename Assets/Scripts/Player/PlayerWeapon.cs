using System;
using Enemy;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using System.Collections;
using System.Collections.Generic;
using Effects;
using MyCameraEffect;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Range = Weapon.Range;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerWeapon : MonoBehaviour
    {
        // [Header("Basic Component")] 
        public Animator animator;

        // [Header("Weapon and Enemy Detect")] 
        [SerializeField] private LayerMask lmWeapon;
        [SerializeField] private LayerMask lmBreakableObj;
        [SerializeField] private LayerMask lmEnemy;
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private float fEnemyDetectionRange;
        [SerializeField] private GameObject objHitBox;

        // [Header("Holding Weapon")] 
        [SerializeField] public Transform tHoldWeaponTransform;
        [SerializeField] [Range(0, 1)] private float fHoldWeaponScale;

        // [Header("Stomp Attack")] 
        [SerializeField] private float fStompTime;
        private bool _bStompAttack;

        // [Header("Sprint")] 
        [SerializeField] private float fSprintDetectionRange;
        [SerializeField] private float fSprintDistance;
        [SerializeField] [Range(0.01f, 0.5f)] private float fSprintTime;

        // [Header("Weapon Attack Effects")] 
        [SerializeField] private GameObject objLobRange;

        // public access field
        public GameObject WeaponEquipped { get; private set; }
        public GameObject EnemyDetected { get; set; }

        // private properties
        private GameObject _downedEnemy;
        private GameObject _weaponSelected;
        private GameObject _breakableObjectDetected;


        private InputControls _inputs;
        private PlayerController _pc;

        [HideInInspector] public List<GameObject> downedEnemies = new();

        [HideInInspector] public Transform attkPos;

        private void Awake()
        {
            _pc = GetComponent<PlayerController>();
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
            _inputs.Gameplay.Fusion.Enable();
            _inputs.Gameplay.Fusion.performed += OnFusionPerformed;
        }

        private void OnDisable()
        {
            _inputs.Gameplay.Weapon.Disable();
            _inputs.Gameplay.Weapon.performed -= OnWeaponPerformed;
            _inputs.Gameplay.Attack.Disable();
            _inputs.Gameplay.Attack.performed -= OnAttackPerformed;
            _inputs.Gameplay.Fusion.Disable();
            _inputs.Gameplay.Fusion.performed -= OnFusionPerformed;
        }

        private void Update()
        {
            WeaponDetectionUpdate();
            if (EnemyDetected != null && !EnemyDetected.GetComponent<EnemyBehaviour>().notStunned)
            {
                ViewCone.Instance.DeRegister(EnemyDetected.GetComponent<Collider>());
            }
        }


        #region SceneDetection

        private void EnemyDetectionUpdate()
        {
            Collider[] hitColliders;
            hitColliders = Physics.OverlapSphere(transform.position, fEnemyDetectionRange, lmEnemy);

            if (hitColliders != null && hitColliders.Length != 0)
            {
                GameObject enemy = GetMinimumDistanceCollider(hitColliders).gameObject;
                if (EnemyDetected != enemy && enemy != null)
                {
                    EnemyDetected = enemy;
                }
            }
            else
            {
                if (EnemyDetected != null)
                {
                    EnemyDetected = null;
                }
            }
        }


        #region WeaponDetection

        private Color _cSelectableColor = Color.blue;
        private Color _cFusedColor = Color.green;

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
                    OnCancelDetectedWeapon(_weaponSelected);
                    _weaponSelected = weapon;

                    // surrounding weapon detection
                    bool fusedWeapon = WeaponEquipped != null &&
                                       FusionSystem.Instance.TestFusionWeapon(WeaponEquipped, _weaponSelected);
                    if (fusedWeapon) OnDetectedFusedWeapon(_weaponSelected);
                    else OnDetectSelectableWeapon(_weaponSelected);
                }
            }
            else
            {
                if (_weaponSelected != null)
                {
                    OnCancelDetectedWeapon(_weaponSelected);
                    _weaponSelected = null;
                }
            }
        }

        /// <summary>
        /// weapon that can not be fused with the holding weapon
        /// </summary>
        private void OnDetectSelectableWeapon(GameObject weapon)
        {
            if (weapon == null) return;
            weapon.GetComponent<WeaponBehaviour>().OnSelected(_cSelectableColor);
        }

        /// <summary>
        /// weapon that can be fused with the holding weapon
        /// </summary>
        private void OnDetectedFusedWeapon(GameObject weapon)
        {
            if (weapon == null) return;
            weapon.GetComponent<WeaponBehaviour>().OnSelected(_cFusedColor);
            FuseIndicator.Instance.ShowFuse();
        }

        private void OnCancelDetectedWeapon(GameObject weapon)
        {
            if (weapon == null) return;
            weapon.GetComponent<WeaponBehaviour>().OnNotSelected();
            FuseIndicator.Instance.HideFuse();
        }

        #endregion


        private void BreakableObjectDetectionUpdate()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, fWeaponGrabRange, lmBreakableObj);

            if (hitColliders != null && hitColliders.Length != 0)
            {
                GameObject breakable = GetMinimumDistanceCollider(hitColliders).gameObject;
                _breakableObjectDetected = breakable;
            }
            else
            {
                _breakableObjectDetected = null;
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
            // detection
            WeaponDetectionUpdate();

            // Can not switch weapon during the attack phase
            if (WeaponEquipped != null && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack) return;
            OnSwitchWeapon();
        }

        private void OnFusionPerformed(InputAction.CallbackContext value)
        {
            if (_weaponSelected != null)
            {
                if (WeaponEquipped != null)
                {
                    WeaponDetectionUpdate();
                    bool fused = FusionCheck();
                    if (fused)
                    {
                        AudioControl.Instance.PlayFusion();
                        OnDropWeapon();
                        OnHoldWeapon();
                    }
                }
            }
        }

        private void OnSwitchWeapon()
        {
            if (_weaponSelected != null)
            {
                if (WeaponEquipped != null)
                {
                    OnDropWeapon();
                }

                AudioControl.Instance.PlayPickup();
                OnHoldWeapon();
            }
            else
            {
                if (WeaponEquipped != null)
                {
                    AudioControl.Instance.PlayPickup();
                    OnDropWeapon();
                }
            }
        }

        private bool FusionCheck()
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
                    return true;
                }
            }

            return false;
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

        public (GameObject[], GameObject[]) OnGetLobRangeEnemy()
        {
            return objLobRange.GetComponent<LobRangeWeaponEffect>().GetDetectedEnemies();
        }


        public void DevShowLobRange()
        {
            objLobRange.GetComponent<LobRangeWeaponEffect>().ShowLobRange();
        }

        #endregion


        #region Attack

        private void OnAttackPerformed(InputAction.CallbackContext value)
        {
            BreakableObjectDetectionUpdate();

            if (!StompAttackCheck() && WeaponEquipped != null)
            {
                if (!WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack)
                {
                    WeaponEquipped.GetComponent<WeaponBehaviour>().OnAttack();
                }

                SprintIn();
            }
            else
            {
                if (_breakableObjectDetected != null)
                {
                    _breakableObjectDetected.GetComponent<Breakable>().OnHit();
                    _breakableObjectDetected = null;
                    BreakBehaviour();
                }
            }
        }

        private void SprintIn()
        {
            if (EnemyDetected != null)
            {
                if (WeaponEquipped == null ||
                    WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee)
                {
                    Vector3 playerPos = transform.position;
                    attkPos = EnemyDetected.GetComponent<EnemyBehaviour>().ActiveAttackPoint();
                    Vector3 enemyPos;
                    enemyPos = attkPos.position;
                    if (downedEnemies != null)
                        enemyPos = EnemyDetected.transform.GetChild(0).GetChild(0).position;
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


        #region Stomp

        private GameObject DownedEnemyCheck()
        {
            if (downedEnemies.Count > 0)
            {
                foreach (GameObject enemy in downedEnemies)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) < fSprintDetectionRange)
                        return enemy;
                }
            }

            return null;
        }

        private bool StompAttackCheck()
        {
            _downedEnemy = DownedEnemyCheck();
            // stomp attack specific
            if (_downedEnemy != null && !_bStompAttack && _downedEnemy.GetComponent<EnemyBehaviour>().bExecution)
            {
                Vector3 playerPos = transform.position;
                Vector3 enemyPos = _downedEnemy.transform.position;
                float dist = Vector3.Distance(playerPos, enemyPos);
                if (dist < fSprintDetectionRange)
                {
                    if (WeaponEquipped != null && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack)
                    {
                        return false;
                    }

                    // kill the enemy
                    EnemyDetected = _downedEnemy;
                    downedEnemies.Remove(_downedEnemy);
                    EnemyDetected.GetComponent<EnemyBehaviour>().OnHit(2, false);
                    Score.Instance.Stomp();

                    SprintIn();
                    StompBehaviour();
                    return true;
                }
            }

            return false;
        }


        private void BreakBehaviour()
        {
            _bStompAttack = true;
            CameraEffect.Instance.GenerateMeleeImpulse();
            animator.SetTrigger("Stomp");
            AudioControl.Instance.PlaySlam();
            StartCoroutine(BreakCountDown(fStompTime));
        }

        IEnumerator BreakCountDown(float time)
        {
            yield return new WaitForSeconds(time);
            _bStompAttack = false;
        }

        private void StompBehaviour()
        {
            _bStompAttack = true;
            animator.SetTrigger("Stomp");
            AudioControl.Instance.PlaySlam();
            StartCoroutine(StompCountdown(fStompTime));
        }

        IEnumerator StompCountdown(float time)
        {
            yield return new WaitForSeconds(time);
            _bStompAttack = false;
        }

        #endregion

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, fWeaponGrabRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, fEnemyDetectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, fSprintDetectionRange);
        }


        #region MeleeDetection

        // the trigger enter will be calculate when the hit box is activated
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Enemy") || other.gameObject != EnemyDetected) return;

            bool meleeWeapon = WeaponEquipped
                               && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack
                               && WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee;

            bool stump = objHitBox.GetComponent<Collider>().enabled &&
                         other.gameObject.layer == LayerMask.NameToLayer("Enemy") && _bStompAttack;


            // TODO detect enemy has dead or not
            if (meleeWeapon)
            {
                // check the whether the enemy has been attacked yet
                if (!WeaponEquipped.GetComponent<WeaponBehaviour>().setEnemyAttacked.Contains(other.gameObject))
                {
                    // Todo this bug comes from enemy
                    other.GetComponent<Knockback>().PlayFeedback(_pc.VecDir.normalized);
                    WeaponEquipped.GetComponent<WeaponBehaviour>().setEnemyAttacked.Add(other.gameObject);
                    if (WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Blunt)
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                        EnemyDetected.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                    }
                    else
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, true);
                    }

                    Score.Instance.Attack(WeaponEquipped);
                }

                WeaponEquipped.GetComponent<WeaponBehaviour>().OnUseMeleeWeapon();
            }
            else if (stump)
            {
                if (other.gameObject != null)
                {
                    if (!other.gameObject.GetComponent<EnemyBehaviour>().notStunned)
                    {
                        other.gameObject.GetComponent<EnemyBehaviour>()?.OnHit(2, true);
                    }
                }
            }
        }

        #endregion
    }
}