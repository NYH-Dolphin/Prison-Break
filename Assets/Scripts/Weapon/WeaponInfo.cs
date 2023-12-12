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
        Throw,
        Lob,
        Swing,
        Thrust,
        Slam,
        Boomerang
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