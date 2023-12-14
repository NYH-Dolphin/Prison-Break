using UnityEngine;
using Player;

namespace Effects
{
    
    public class DirAngleEffect : MonoBehaviour
    {
        // TODO set enemy hint
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(!ViewCone.Instance._objectsInTrigger.Contains(other)) ViewCone.Instance.Register(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ViewCone.Instance.DeRegister(other);
            }
        }
    }
}