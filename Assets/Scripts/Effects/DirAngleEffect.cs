using UnityEngine;
using Player;

namespace Effects
{
    
    public class DirAngleEffect : MonoBehaviour
    {
        [SerializeField] private ViewCone viewCone;
        
        // TODO set enemy hint
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                viewCone._objectsInTrigger.Add(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                viewCone._objectsInTrigger.Remove(other);
                Debug.Log(other.gameObject.name + "   " + other.transform.GetChild(2).name);
                other.transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(50,50,50);
                other.transform.GetChild(2).transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                GetComponentInParent<PlayerWeapon>()._enemyDetected = null;
            }
        }
    }
}