using MyCameraEffect;
using UnityEngine;

namespace LevelTutorial
{
    public class BreakDoor : MonoBehaviour
    {
        [SerializeField] private GameObject breakEffectPrefab;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&
                GameObject.Find("[Player]/PlayerSprites/Player Hitbox").GetComponent<Collider>().enabled)
            {
                var effect = Instantiate(breakEffectPrefab);
                effect.transform.position = transform.position;
                CameraEffect.Instance.GenerateBumpImpulse();
                AudioControl.Instance.PlayDoorBreak();
            }
        }
    }
}