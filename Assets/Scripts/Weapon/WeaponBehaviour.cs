using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody), typeof(Collider))]
    public class WeaponBehaviour : MonoBehaviour
    {
        public WeaponInfo weaponInfo;
        [SerializeField] protected LayerMask lmGround;

        // basic components
        protected SpriteRenderer Sr;
        protected Rigidbody Rb;
        protected Material Mat;
        protected PlayerWeaponEffect Effect;
        protected PlayerWeapon Pw;
        protected PlayerController Pc;
        protected Collider Coll;

        [HideInInspector] public int iDurability;
        [HideInInspector] public bool bAttack;
        [HideInInspector] public HashSet<GameObject> setEnemyAttacked; // enemy detected in one attack section

        private void Awake()
        {
            if (weaponInfo == null)
            {
                Debug.LogError("Weapon Info is not set! Make Sure you specify a correct weapon info");
            }


            // weaponInfo is a serializable object, we need to use runtime variable 
            iDurability = weaponInfo.iDurability;
            Sr = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody>();
            Coll = GetComponent<Collider>();
            Mat = Sr.material;
            setEnemyAttacked = new();
            OnNotSelected();
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public virtual void OnSelected(Color color)
        {
            Mat.SetColor("_GlowColor", color);
            Mat.SetFloat("_GlowAmount", 1f);
        }

        public virtual void OnNotSelected()
        {
            Mat.SetFloat("_GlowAmount", 0f);
        }

        /// <summary>
        /// Weapon Effect for grabbing
        /// </summary>
        public virtual void OnHold(PlayerWeapon pw)
        {
            Pw = pw;
            Pc = pw.gameObject.GetComponent<PlayerController>();
            Effect = pw.gameObject.GetComponent<PlayerWeaponEffect>();
            Coll.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Player");
            OnNotSelected();
            Rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        /// <summary>
        /// Weapon Effect for dropping
        /// </summary>
        public virtual void OnDrop()
        {
            Pw = null;
            Pc = null;
            Effect.OnCancelAllEffect();
            Effect = null;
            // ignore player collision when drop
            Coll.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Weapon");
            transform.parent = GameObject.Find("[Weapon]").transform;
            Rb.constraints = RigidbodyConstraints.FreezeRotation;
            Rb.drag = 1f;
            Rb.angularDrag = 0.05f;
        }


        /// <summary>
        /// Drop with a specific direction
        /// </summary>
        /// <param name="dropDir"></param>
        public virtual void OnDrop(Vector3 dropDir)
        {
            OnDrop();
            Rb.AddForce(dropDir * 3f, ForceMode.Impulse);
        }


        public virtual void OnAttack()
        {
            setEnemyAttacked = new();
            Pc.OnAttackPerformed(weaponInfo.eAttackType);
            if (weaponInfo.eRange == Range.Ranged)
            {
                Pc.SetPlayerAttackPosition();
            }
        }


        public void OnUseMeleeWeapon()
        {
            iDurability -= 1;

            if (iDurability == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                bAttack = false;
            }
        }
    }
}