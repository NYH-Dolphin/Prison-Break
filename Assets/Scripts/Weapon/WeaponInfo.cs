using System;
using UnityEngine;

namespace Weapon
{
    public enum Sharpness
    {
        Sharp,
        Blunt
    }

    public enum Range
    {
        Ranged,
        Melee
    }


    public enum AttackType
    {
        Throwable,
        Lob,
        Swing,
        Thrust,
        Slam
    }

    [CreateAssetMenu(fileName = "WeaponInfo", menuName = "ScriptableObjects/WeaponInformation", order = 1)]
    public class WeaponInfo : ScriptableObject
    {
        public int iDurability;
        public Sharpness eSharpness;
        public Range eRange;
        public AttackType eAttackType;
        public bool bFused;
    }
}