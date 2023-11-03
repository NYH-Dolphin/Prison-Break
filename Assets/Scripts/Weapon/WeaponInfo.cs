using System;
using UnityEngine;

namespace Weapon
{
    public enum WeaponAttackType
    {
        Throwable,
        Lob,
        Swing,
        Shooting,
    }

    [Serializable]
    public class WeaponInfo : MonoBehaviour
    {
        public string sName;
        public int iAvailableTimes;
        public float fWeight;
        public float fAtk;
        public WeaponAttackType eWeaponAttackType;
    }
}