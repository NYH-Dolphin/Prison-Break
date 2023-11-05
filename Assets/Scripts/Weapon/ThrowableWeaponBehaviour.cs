using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class ThrowableWeaponBehaviour : WeaponBehaviour
    {
        private CinemachineSmoothPath _path;
        private CinemachineDollyCart _cart;

        private void Start()
        {
            _path = GameObject.Find("Dolly Track").GetComponent<CinemachineSmoothPath>();
        }
        
        private void Update()
        {
            if (_cart != null && Math.Abs(_cart.m_Position - 1.0f) < 0.01f)
            {
                Destroy(gameObject);
            }
        }

        public override void OnAttack(Transform startTransform, Transform targetTransform)
        {
            if (_cart == null)
            {
                CinemachineSmoothPath.Waypoint[] points = _path.m_Waypoints;
                Vector3 startPosition = startTransform.position;
                Vector3 targetPosition = targetTransform.position;
                Vector3 midPosition = (startPosition + targetPosition) / 2.0f;
                midPosition.y += 1f;
                points[0].position = startPosition;
                points[1].position = midPosition;
                points[2].position = targetPosition;
                _cart = transform.AddComponent<CinemachineDollyCart>();
                _cart.m_PositionUnits = CinemachinePathBase.PositionUnits.Normalized;
                _cart.m_Path = _path;
                _cart.m_Speed = 2f;
            }
        }
    }
}