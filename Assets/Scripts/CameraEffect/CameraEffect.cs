using System;
using Cinemachine;
using UnityEngine;

namespace MyCameraEffect
{
    public class CameraEffect : MonoBehaviour
    {
        private CinemachineImpulseSource _impulse;

        public static CameraEffect Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _impulse = GameObject.Find("[Player]").gameObject.GetComponent<CinemachineImpulseSource>();
        }


        public void GenerateImpulse()
        {
            _impulse.GenerateImpulse();
        }

        public void GenerateImpulse(Vector3 velocity)
        {
            _impulse.GenerateImpulseWithVelocity(velocity);
        }
    }
}