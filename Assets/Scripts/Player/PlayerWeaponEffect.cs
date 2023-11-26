using System;
using UnityEngine;

namespace Player
{
    
    [RequireComponent(typeof(PlayerWeapon))]
    public class PlayerWeaponEffect : MonoBehaviour
    {
        private PlayerWeapon _pw;

        private void Awake()
        {
            _pw = GetComponent<PlayerWeapon>();
        }
    }
}