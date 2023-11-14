using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    [Serializable]
    public class FusionPair
    {
        [SerializeField] public WeaponInfo rangeWeapon;
        [SerializeField] public WeaponInfo meleeWeapon;
        [SerializeField] public GameObject fusionWeapon;
    }

    public class FusionSystem : MonoBehaviour
    {
        [SerializeField] private List<FusionPair> pairs;

        private Dictionary<string, GameObject> _dicFusion;
        public static FusionSystem Instance;

        private void Awake()
        {
            _dicFusion = new();
            for (int i = 0; i < pairs.Count; i++)
            {
                string key = $"{pairs[i].rangeWeapon.name}-{pairs[i].meleeWeapon.name}";
                _dicFusion[key] = pairs[i].fusionWeapon;
            }

            Instance = this;
        }

        public GameObject GetFusionWeapon(WeaponInfo w1, WeaponInfo w2)
        {
            if (w1.eRange == w2.eRange) return null;
            string key = w1.eRange == Range.Ranged ? $"{w1.name}-{w2.name}" : $"{w2.name}-{w1.name}";
            if (_dicFusion.TryGetValue(key, out var weapon))
            {
                return weapon;
            }
            return null;
        }
    }
}