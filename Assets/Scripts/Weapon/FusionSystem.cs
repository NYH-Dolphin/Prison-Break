using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    [Serializable]
    public class FusionPair
    {
        [SerializeField] public GameObject rangeWeapon;
        [SerializeField] public GameObject meleeWeapon;
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
                string key =
                    $"{pairs[i].rangeWeapon.GetComponent<SpriteRenderer>().sprite.name}" +
                    $"-" +
                    $"{pairs[i].meleeWeapon.GetComponent<SpriteRenderer>().sprite.name}";
                _dicFusion[key] = pairs[i].fusionWeapon;
            }
            Instance = this;
        }

        public GameObject GetFusionWeapon(GameObject w1, GameObject w2)
        {
            if (w1.GetComponent<WeaponBehaviour>().weaponInfo.eRange ==
                w2.GetComponent<WeaponBehaviour>().weaponInfo.eRange) return null;
            string key = w1.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Ranged
                ? $"{w1.GetComponent<SpriteRenderer>().sprite.name}-{w2.GetComponent<SpriteRenderer>().sprite.name}"
                : $"{w2.GetComponent<SpriteRenderer>().sprite.name}-{w1.GetComponent<SpriteRenderer>().sprite.name}";
            if (_dicFusion.TryGetValue(key, out var weapon))
            {
                return Instantiate(weapon);
            }
            return null;
        }
    }
}