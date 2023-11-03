using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponBehaviour : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private Material _mat; // require material "2d Sprite Glow"
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            _mat = _sr.material;
            OnNotSelected();
        }

        /// <summary>
        /// Weapon is selected and ready for grabbing
        /// </summary>
        public void OnSelected()
        {
            _mat.SetFloat(OutlineWidth, 5);
        }

        public void OnNotSelected()
        {
            _mat.SetFloat(OutlineWidth, 0);
        }
    }
}