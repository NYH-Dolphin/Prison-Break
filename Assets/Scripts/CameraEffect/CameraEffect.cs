using Cinemachine;
using UnityEngine;

namespace MyCameraEffect
{
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource bumpImpulse;
        [SerializeField] private CinemachineImpulseSource meleeImpulse;
        [SerializeField] private CinemachineImpulseSource smallImpulse;
        
        
        public static CameraEffect Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            transform.TryGetComponent(out bumpImpulse);
        }


        public void GenerateBumpImpulse()
        {
            bumpImpulse.GenerateImpulse();
        }

        public void GenerateMeleeImpulse()
        {
            meleeImpulse.GenerateImpulse();
        }

        public void GenerateMeleeImpulseWithVelocity(Vector3 velocity)
        {
            meleeImpulse.GenerateImpulseWithVelocity(velocity);
        }

        public void GenerateSmallImpulse()
        {
            smallImpulse.GenerateImpulse();
        }

    }
}