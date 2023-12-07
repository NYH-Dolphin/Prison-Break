using UnityEngine;

namespace Weapon.Effects
{
    public class DirAngleEffect : MonoBehaviour
    {
        
        // TODO set enemy hint
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Debug.Log("detect: " + other.gameObject.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Debug.Log("exit: " + other.gameObject.name);
            }
        }
    }
}