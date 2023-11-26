using Player;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody), typeof(Collider))]
    public class WeaponBehaviour : MonoBehaviour
    {
        public WeaponInfo weaponInfo;
        protected SpriteRenderer Sr;
        protected Rigidbody Rb;
        protected Material Mat; // require material "2d Sprite Glow"
        [HideInInspector] public bool bAttack;
        protected PlayerWeapon Pw;
        protected PlayerController Pc;
        protected Collider Coll;
        protected int IDurability;


        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");

        // TODO will be modified later by each weapon's behaviour
        private float _fDropForce = 3f;


        private void Awake()
        {
            if (weaponInfo == null)
            {
                Debug.LogError("Weapon Info is not set! Make Sure you specify a correct weapon info");
            }

            // weaponInfo is a serializable object, we need to use runtime variable 
            IDurability = weaponInfo.iDurability;
            Sr = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody>();
            Coll = GetComponent<Collider>();
            Mat = Sr.material;
            OnNotSelected();
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public virtual void OnSelected()
        {
            Mat.SetFloat(OutlineWidth, 5);
        }

        public virtual void OnNotSelected()
        {
            Mat.SetFloat(OutlineWidth, 0);
        }

        /// <summary>
        /// Weapon Effect for grabbing
        /// </summary>
        public virtual void OnHold(PlayerWeapon pw)
        {
            Pw = pw;
            Pc = pw.gameObject.GetComponent<PlayerController>();
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
            gameObject.layer = LayerMask.NameToLayer("Weapon");
            transform.parent = GameObject.Find("[Weapon]").transform;
            Rb.constraints = RigidbodyConstraints.None;
        }


        /// <summary>
        /// Drop with a specific direction
        /// </summary>
        /// <param name="dropDir"></param>
        public virtual void OnDrop(Vector3 dropDir)
        {
            OnDrop();
            Rb.AddForce(dropDir * _fDropForce, ForceMode.Impulse);
        }


        public virtual void OnAttack()
        {
        }


        public void OnUseMeleeWeapon()
        {
            IDurability -= 1;

            if (IDurability == 0)
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