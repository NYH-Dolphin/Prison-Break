using Enemy;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody), typeof(Collider))]
    public class WeaponBehaviour : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private Material _mat; // require material "2d Sprite Glow"
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");

        // TODO will be modified later by each weapon's behaviour
        private float _fDropForce = 3f;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody>();
            _mat = _sr.material;
            OnNotSelected();
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public virtual void OnSelected()
        {
            _mat.SetFloat(OutlineWidth, 5);
        }

        public virtual void OnNotSelected()
        {
            _mat.SetFloat(OutlineWidth, 0);
        }

        /// <summary>
        /// Weapon Effect for grabbing
        /// </summary>
        public virtual void OnHold()
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            OnNotSelected();
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        /// <summary>
        /// Weapon Effect for dropping
        /// </summary>
        public virtual void OnDrop()
        {
            gameObject.layer = LayerMask.NameToLayer("Weapon");
            transform.parent = GameObject.Find("[Weapon]").transform;
            _rb.constraints = RigidbodyConstraints.None;
        }


        /// <summary>
        /// Drop with a specific direction
        /// </summary>
        /// <param name="dropDir"></param>
        public virtual void OnDrop(Vector3 dropDir)
        {
            OnDrop();
            _rb.AddForce(dropDir * _fDropForce, ForceMode.Impulse);
        }

        public virtual void OnAttack(Transform startTransform, Transform targetTransform)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                other.gameObject.GetComponent<EnemyBehaviour>().OnHit();
                Destroy(gameObject);
            }
        }
    }
}