using System;
using Cinemachine;
using UnityEngine;

namespace MyCameraEffect
{
    public class CameraEffect : MonoBehaviour
    {
        private CinemachineCollisionImpulseSource _impulse;

        public static CameraEffect Instance;

        private void Awake()
        {
            Instance = this;
        }


        public void GenerateImpulse()
        {
            _impulse.GenerateImpulse();
        }

        private void Start()
        {
            _impulse = GameObject.Find("[Player]").gameObject.GetComponent<CinemachineCollisionImpulseSource>();
        }
    }
}