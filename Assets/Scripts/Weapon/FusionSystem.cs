using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapon
{
    [Serializable]
    public class FusionPair
    {
        [SerializeField] public GameObject rangeWeapon;
        [SerializeField] public GameObject meleeWeapon;
        [SerializeField] public GameObject fusionWeapon;
    }

    [Serializable]
    public class BaseWeapon
    {
        [SerializeField] public AttackType type;
        [SerializeField] public GameObject baseWeaponPrefab;
    }


    public class FusionSystem : MonoBehaviour
    {
        [SerializeField] private TextAsset csvFusionWeaponList;
        [SerializeField] private TextAsset csvFusionPairList;
        [SerializeField] private List<BaseWeapon> baseWeaponTypePrefabs;

        private Dictionary<string, WeaponInfo> _dicFusionWeaponInfos;
        private Dictionary<AttackType, GameObject> _dicBaseWeaponPrefabs;
        private Dictionary<string, string> _dicFusionPair;
        public static FusionSystem Instance;


        [SerializeField] private List<FusionPair> pairs;
        private Dictionary<string, GameObject> _dicFusion;


        private void Awake()
        {
            LoadBaseWeapon();
            ReadFusionWeaponCSV();
            ReadFusionPairCSV();

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


        private void LoadBaseWeapon()
        {
            _dicBaseWeaponPrefabs = new Dictionary<AttackType, GameObject>();
            foreach (BaseWeapon baseWeapon in baseWeaponTypePrefabs)
            {
                _dicBaseWeaponPrefabs[baseWeapon.type] = baseWeapon.baseWeaponPrefab;
            }
        }

        private void ReadFusionPairCSV()
        {
            if (csvFusionPairList == null)
            {
                Debug.LogError("Fusion pair CSV file is required to provide");
            }

            _dicFusionPair = new Dictionary<string, string>();
            string[] data = csvFusionPairList.text.Replace("\r", "").Split('\n');
            data = data.Skip(1).ToArray(); // skip the first row (for name)
            foreach (string line in data)
            {
                string[] fields = line.Split(',');
                if (fields.Length != 3) // remove redundant line
                {
                    continue;
                }

                string weapon1 = fields[0];
                string weapon2 = fields[1];
                string fusionWeapon = fields[2];
                _dicFusionPair[weapon1 + "-" + weapon2] = fusionWeapon;
            }
        }

        private void ReadFusionWeaponCSV()
        {
            if (csvFusionWeaponList == null)
            {
                Debug.LogError("Fusion weapon CSV file is required to provide");
            }

            _dicFusionWeaponInfos = new Dictionary<string, WeaponInfo>();
            string[] data = csvFusionWeaponList.text.Split('\n');
            data = data.Skip(1).ToArray();
            foreach (string line in data)
            {
                string[] fields = line.Split(',');
                if (fields.Length == 1) // remove redundant line
                {
                    continue;
                }

                string weaponName = fields[0];
                string range = fields[1];
                string attackType = fields[2];
                string sharpness = fields[3];

                // Set up the weaponInfo
                WeaponInfo weaponInfo = ScriptableObject.CreateInstance<WeaponInfo>();
                weaponInfo.name = weaponName;
                weaponInfo.bFused = true;
                weaponInfo.iDurability = 3;
                switch (range)
                {
                    case "melee":
                        weaponInfo.eRange = Range.Melee;
                        break;
                    case "ranged":
                        weaponInfo.eRange = Range.Ranged;
                        break;
                }

                switch (attackType)
                {
                    case "slam":
                        weaponInfo.eAttackType = AttackType.Slam;
                        break;
                    case "swing":
                        weaponInfo.eAttackType = AttackType.Swing;
                        break;
                    case "thrust":
                        weaponInfo.eAttackType = AttackType.Thrust;
                        break;
                    case "throw":
                        weaponInfo.eAttackType = AttackType.Throwable;
                        break;
                    case "boomerang":
                        weaponInfo.eAttackType = AttackType.Boomerang;
                        break;
                    case "lob":
                        weaponInfo.eAttackType = AttackType.Lob;
                        break;
                }

                switch (sharpness)
                {
                    case "sharp":
                        weaponInfo.eSharpness = Sharpness.Sharp;
                        break;
                    case "blunt":
                        weaponInfo.eSharpness = Sharpness.Blunt;
                        break;
                }

                _dicFusionWeaponInfos[weaponName] = weaponInfo;
            }
        }

        public GameObject GetFusionWeapon(GameObject w1, GameObject w2)
        {
            if (w1.GetComponent<WeaponBehaviour>().weaponInfo.eRange ==
                w2.GetComponent<WeaponBehaviour>().weaponInfo.eRange) return null;
            string key = w1.GetComponent<WeaponBehaviour>().weaponInfo.eRange == Range.Ranged
                ? $"{w1.GetComponent<SpriteRenderer>().sprite.name}-{w2.GetComponent<SpriteRenderer>().sprite.name}"
                : $"{w2.GetComponent<SpriteRenderer>().sprite.name}-{w1.GetComponent<SpriteRenderer>().sprite.name}";
            if (_dicFusionPair.TryGetValue(key, out var weaponName))
            {
                WeaponInfo weaponInfo = _dicFusionWeaponInfos[weaponName];
                GameObject weaponPrefab = _dicBaseWeaponPrefabs[weaponInfo.eAttackType];
                weaponPrefab.GetComponent<WeaponBehaviour>().weaponInfo = weaponInfo;
                string spritePath = $"FusionWeapon/{weaponName}";
                Sprite newSprite = Resources.Load<Sprite>(spritePath);
                weaponPrefab.GetComponent<SpriteRenderer>().sprite = newSprite;
                return Instantiate(weaponPrefab);
            }

            return null;
        }
    }
}