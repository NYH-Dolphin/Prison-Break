using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapon
{
    [Serializable]
    public class FusionData
    {
        [SerializeField] public GameObject fusionWeapon;
        [SerializeField] public string fusionSpritePath;
        [SerializeField] public WeaponInfo weaponInfo;
    }

    public class FusionSystem : MonoBehaviour
    {
        [SerializeField] private TextAsset fusionCsv;
        private Dictionary<string, FusionData> _fusionDict;
        public static FusionSystem Instance;
        
        private void Awake()
        {
            _fusionDict = new();
            ReadFusionCsv();

            Instance = this;
        }

        private void ReadFusionCsv()
        {
            if (fusionCsv == null)
            {
                Debug.LogError("Fusion weapon CSV file is required");
            }

            string[] data = fusionCsv.text.Split('\n');
            data = data.Skip(1).ToArray();
            foreach (string line in data)
            {
                string[] fields = line.Split(',');
                if (fields.Length == 1) // remove redundant line
                {
                    continue;
                }

                string weaponName = fields[0];
                string meleeWeaponName = fields[1];
                string rangedWeaponName = fields[2];
                string spritePath = $"FusionWeaponSprites/{meleeWeaponName}-{rangedWeaponName}";
                string attackBehavior = fields[5];
                string sharpness = fields[6];

                // Set up the weaponInfo
                WeaponInfo weaponInfo = ScriptableObject.CreateInstance<WeaponInfo>();
                weaponInfo.bFused = true;
                weaponInfo.iDurability = 3;
                string weaponPrefabPath = string.Empty;

                // Determine the range, attack type, and prefab path from the attack type column
                switch (attackBehavior)
                {
                    case "slam":
                        weaponInfo.eAttackType = AttackType.Slam;
                        weaponInfo.eRange = Range.Melee;
                        weaponPrefabPath = "FusionWeaponPrefabs/SlamWeapon";
                        break;
                    case "swing":
                        weaponInfo.eAttackType = AttackType.Swing;
                        weaponInfo.eRange = Range.Melee;
                        weaponPrefabPath = "FusionWeaponPrefabs/SwingWeapon";
                        break;
                    case "thrust":
                        weaponInfo.eAttackType = AttackType.Thrust;
                        weaponInfo.eRange = Range.Melee;
                        weaponPrefabPath = "FusionWeaponPrefabs/ThrustWeapon";
                        break;
                    case "throw":
                        weaponInfo.eAttackType = AttackType.Throwable;
                        weaponInfo.eRange = Range.Ranged;
                        weaponPrefabPath = "FusionWeaponPrefabs/ThrowWeapon";
                        break;
                    case "boomerang":
                        weaponInfo.eAttackType = AttackType.Boomerang;
                        weaponInfo.eRange = Range.Ranged;
                        weaponPrefabPath = "FusionWeaponPrefabs/BoomerangWeapon";
                        break;
                    case "lob":
                        weaponInfo.eAttackType = AttackType.Lob;
                        weaponInfo.eRange = Range.Ranged;
                        weaponPrefabPath = "FusionWeaponPrefabs/LobWeapon";
                        break;
                }

                // Determine the weapon sharpness from the sharpness column
                switch (sharpness)
                {
                    case "sharp":
                        weaponInfo.eSharpness = Sharpness.Sharp;
                        break;
                    case "blunt":
                        weaponInfo.eSharpness = Sharpness.Blunt;
                        break;
                }

                // Create two keys - melee weapon first or ranged weapon first
                string key1 = $"{meleeWeaponName}-{rangedWeaponName}";
                string key2 = $"{rangedWeaponName}-{meleeWeaponName}";

                // Create the weapon prefab based on the attack type
                GameObject weaponPrefab = Resources.Load<GameObject>(weaponPrefabPath);

                // create fusion data and assign the weapon prefab and weapon info
                FusionData fd = new FusionData();
                fd.fusionWeapon = weaponPrefab;
                fd.weaponInfo = weaponInfo;
                fd.fusionSpritePath = spritePath;

                // add fusion data to dict under both keys
                _fusionDict[key1] = fd;
                _fusionDict[key2] = fd;
            }
        }

        public GameObject GetFusionWeapon(GameObject w1, GameObject w2)
        { 
            // Cant fuse ranged and melee weapons
            if (w1.GetComponent<WeaponBehaviour>().weaponInfo.eRange == w2.GetComponent<WeaponBehaviour>().weaponInfo.eRange){
                return null;
            }
            string key = $"{w1.GetComponent<SpriteRenderer>().sprite.name}-{w2.GetComponent<SpriteRenderer>().sprite.name}";

            if (_fusionDict.TryGetValue(key, out FusionData fusionData))
            {
                // Create instance of the weapon prefab and the weapon info
                WeaponInfo weaponInfo = Instantiate(fusionData.weaponInfo);
                GameObject weaponPrefabInstance = Instantiate(fusionData.fusionWeapon);

                // set the prefabs weapon info to the fusions weapon info
                weaponPrefabInstance.GetComponent<WeaponBehaviour>().weaponInfo = weaponInfo;
                weaponPrefabInstance.GetComponent<WeaponBehaviour>().iDurability = weaponInfo.iDurability;
                
                
                // Set the sprite of the prefab to the fusion sprite
                Sprite newSprite = Resources.Load<Sprite>(fusionData.fusionSpritePath);
                weaponPrefabInstance.GetComponent<SpriteRenderer>().sprite = newSprite;
                weaponPrefabInstance.transform.localScale *= 1.5f;

                return weaponPrefabInstance;
            }
            Debug.Log($"Fusion not found for key: {key}");
            return null;
        }
    }
}