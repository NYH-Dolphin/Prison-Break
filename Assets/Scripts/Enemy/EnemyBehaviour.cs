using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        private Material _mat; // require material "2d Sprite Glow"
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");

        private void Awake()
        {
            _mat = sr.material;
            OnNotSelected();
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public void OnSelected()
        {
            _mat.SetColor(GlowColor, Color.red);
            _mat.SetFloat(OutlineWidth, 0.03f);
        }

        public void OnNotSelected()
        {
            _mat.SetFloat(OutlineWidth, 0);
        }
        
        
        // TODO Current directly make enemy dead after being hitted
        public void OnHit()
        {
            Destroy(gameObject);
        }
    }
}