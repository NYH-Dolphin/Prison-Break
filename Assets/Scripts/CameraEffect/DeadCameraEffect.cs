using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace MyCameraEffect
{
    public class DeadCameraEffect : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        private bool _bTrigger;
        private float _fLerpTime;
        private ColorAdjustments _col;
        public static DeadCameraEffect Instance;

        private void Awake()
        {
            Instance = this;
        }


        private void Update()
        {
            if (volume == null) return;
            if (_bTrigger)
            {
                _fLerpTime += Time.deltaTime * 0.5f;
                _fLerpTime = Math.Min(1, _fLerpTime);

                if (volume.profile.TryGet(out _col))
                {
                    _col.saturation.value = -100 * _fLerpTime;
                }
            }
        }


        public void OnTriggerDeadEffect()
        {
            _bTrigger = true;
        }
    }
}