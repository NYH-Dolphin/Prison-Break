using Enemy;
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
        protected bool BAttack;
        
        
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        
        // TODO will be modified later by each weapon's behaviour
        private float _fDropForce = 3f;


        private void Awake()
        {
            if (weaponInfo == null)
            {
                Debug.LogError("Weapon Info is not set! Make Sure you specify a correct weapon info");
            }
            
            Sr = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody>();
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
        public virtual void OnHold()
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            OnNotSelected();
            Rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        /// <summary>
        /// Weapon Effect for dropping
        /// </summary>
        public virtual void OnDrop()
        {
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


        /// <summary>
        /// Weapon Attack Behaviour
        /// </summary>
        /// <param name="startTransform"></param> Weapon Staring Point
        /// <param name="targetTransform"></param>Target Enemy
        /// <param name="facingDir"></param>Player Facing Point
        public virtual void OnAttack(Transform startTransform, Transform targetTransform, Vector3 facingDir)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (BAttack && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.gameObject.GetComponent<EnemyBehaviour>().OnHit();
                Destroy(gameObject);
            }
        }
    }
}