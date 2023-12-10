using System;
using Enemy;
using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Weapon.Effects;
using Range = Weapon.Range;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerWeapon : MonoBehaviour
    {
        [Header("Basic Component")] [SerializeField]
        private Animator animator;

        [Header("Weapon and Enemy Detect")] [SerializeField]
        private LayerMask lmWeapon;

        [SerializeField] private LayerMask lmBreakableObj;
        [SerializeField] private LayerMask lmEnemy;
        [SerializeField] private float fWeaponGrabRange;
        [SerializeField] private float fEnemyDetectionRange;
        [SerializeField] private float fEnemyDetectionAngle;
        [SerializeField] private GameObject objHitBox;

        [Header("Holding Weapon")] [SerializeField]
        public Transform tHoldWeaponTransform;

        [SerializeField] [Range(0, 1)] private float fHoldWeaponScale;

        [FormerlySerializedAs("fShivTime")] [Header("Stomp Attack")] [SerializeField]
        private float fStompTime;

        private bool _bStompAttack;

        [Header("Sprint")] [SerializeField] private float fSprintDetectionRange;
        [SerializeField] private float fSprintDistance;
        [SerializeField] [Range(0.01f, 0.5f)] private float fSprintTime;

        [Header("Weapon Attack Effects")] [SerializeField]
        private GameObject objLobRange;

        // private properties
        private GameObject _enemyDetected;
        private GameObject _weaponSelected;
        private GameObject _breakableObjectDetected;
        public GameObject WeaponEquipped { get; private set; }
        private Vector3 direction;

        private InputControls _inputs;
        private PlayerController _pc;


        private void Awake()
        {
            _pc = GetComponent<PlayerController>();
        }


        private void Update()
        {
            DevShowLobRange();
            EnemyDetectionUpdate();
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


        #region SceneDetection

        // Current weapon behaviour no longer need enemy detection
        private void EnemyDetectionUpdate()
        {
            Collider[] hitColliders;
            hitColliders = Physics.OverlapSphere(transform.position, fEnemyDetectionRange, lmEnemy);

            if (hitColliders != null && hitColliders.Length != 0)
            {
                GameObject enemy = GetMinimumDistanceCollider(hitColliders).gameObject;
                if(enemy != null)
                {
                    Vector3 directionToTarget = (enemy.transform.position - transform.position).normalized;
                    float angle = Vector3.Angle(direction, directionToTarget);
                    if (angle < fEnemyDetectionAngle / 2)
                    {
                        if(_enemyDetected != enemy)
                        {
                            _enemyDetected = enemy;
                            _enemyDetected.transform.GetChild(3).gameObject.SetActive(true);
                        } 
                        
                    }
                    else if (_enemyDetected != null)
                    {
                        _enemyDetected.transform.GetChild(3).gameObject.SetActive(false);
                        _enemyDetected = null;
                    }
                }
                
            }
            else
            {
                if (_enemyDetected != null)
                {
                    _enemyDetected.transform.GetChild(3).gameObject.SetActive(false);
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

        public void DirectionCheck(float hor, float vert)
        {
            if(hor > 0)
            {
                if(vert > 0)
                {
                    direction = new Vector3(1,0,1).normalized;
                }
                else if(vert < 0)
                {
                    direction = new Vector3(1,0,-1).normalized;
                }
                else
                    direction = new Vector3(1,0,0).normalized;
            }
            else if(hor < 0)
            {
                if(vert > 0)
                {
                    direction = new Vector3(-1,0,1).normalized;
                }
                else if(vert < 0)
                {
                    direction = new Vector3(-1,0,-1).normalized;
                }
                else
                    direction = new Vector3(-1,0,0).normalized;
            }
            else
            {
                if(vert > 0)
                {
                    direction = new Vector3(0,0,1).normalized;
                }
                else if(vert < 0)
                {
                    direction = new Vector3(0,0,-1).normalized;
                }
            }

            
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

        private bool FusionCheck()
        {
            WeaponInfo weaponEquippedInfo = WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo;
            // use a basic weapon
            if (!weaponEquippedInfo.bFused)
            {
                GameObject fusedWeapon = FusionSystem.Instance.GetFusionWeapon(WeaponEquipped, _weaponSelected);
                if (fusedWeapon != null)
                {
                    Debug.Log("TRIGGER");
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
            EnemyDetectionUpdate();
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
                    StompBehaviour();
                }
            }
        }

        private void SprintIn()
        {
            if (_enemyDetected != null)
            {
                if (WeaponEquipped == null ||
                    WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Melee)
                {
                    Vector3 playerPos = transform.position;
                    Transform attkPos = _enemyDetected.GetComponent<EnemyBehaviour>().ActiveAttackPoint();
                    Debug.Log(attkPos.name);
                    Vector3 enemyPos = attkPos.position;
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

        private bool StompAttackCheck()
        {
            // stomp attack specific
            if (_enemyDetected != null && _enemyDetected.GetComponent<EnemyBehaviour>().bExecution && !_bStompAttack)
            {
                Vector3 playerPos = transform.position;
                Vector3 enemyPos = _enemyDetected.transform.position;
                float dist = Vector3.Distance(playerPos, enemyPos);
                if (dist < fSprintDetectionRange)
                {
                    if (WeaponEquipped != null && WeaponEquipped.GetComponent<WeaponBehaviour>().bAttack)
                    {
                        return false;
                    }

                    // directly kill it
                    if (_enemyDetected != null)
                    {
                        _enemyDetected.GetComponent<EnemyBehaviour>().OnHit(2, false);
                    }

                    StompBehaviour();
                    SprintIn();
                    return true;
                }
            }

            return false;
        }


        private void StompBehaviour()
        {
            _bStompAttack = true;
            animator.SetTrigger("Stomp");
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
        }


        #region MeleeDetection

        // the trigger enter will be calculate when the hit box is activated
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Enemy") || other.gameObject != _enemyDetected) return;

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
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHitBlunt();
                    else
                        other.gameObject.GetComponent<EnemyBehaviour>().OnHit(2, true);
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