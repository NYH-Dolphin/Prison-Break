using MyCameraEffect;
using UnityEngine;

namespace LevelTutorial
{
    public class BreakDoor : MonoBehaviour
    {

        [SerializeField] private GameObject breakEffectPrefab;
        
        private void OnDestroy()
        {
            var effect = Instantiate(breakEffectPrefab);
            effect.transform.position = transform.position;
            CameraEffect.Instance.GenerateImpulse();
            AudioControl.Instance.PlayDoorBreak();
        }
    }
}