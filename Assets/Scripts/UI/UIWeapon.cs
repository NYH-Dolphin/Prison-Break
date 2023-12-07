using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon;
using Range = Weapon.Range;

namespace UI
{
    public class UIWeapon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI uiWeaponName;
        [SerializeField] private List<Image> uiWeaponNums;
        [SerializeField] private Sprite texNull;
        private PlayerWeapon _pw;

        private void Start()
        {
            _pw = GameObject.Find("[Player]").GetComponent<PlayerWeapon>();
        }

        private void Update()
        {
            UIWeaponInfoUpdate();
        }


        void UIWeaponInfoUpdate()
        {
            if (_pw.WeaponEquipped == null)
            {
                if (uiWeaponName.text != "NONE")
                {
                    uiWeaponName.text = "NONE";
                    iTween.ScaleFrom(uiWeaponName.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    uiWeaponName.color = Color.white;

                    for (int i = 0; i < 3; i++)
                    {
                        uiWeaponNums[i].sprite = texNull;
                    }
                }
            }
            else
            {
                GameObject weapon = _pw.WeaponEquipped;
                WeaponBehaviour wb = weapon.GetComponent<WeaponBehaviour>();
                Sprite wsr = weapon.GetComponent<SpriteRenderer>().sprite;

                // weapon durability
                for (int i = 0; i < 3; i++)
                {
                    if (i + 1 <= wb.iDurability)
                    {
                        uiWeaponNums[i].sprite = wsr;
                    }
                    else
                    {
                        uiWeaponNums[i].sprite = texNull;
                    }
                }

                if (uiWeaponName.text != wsr.name)
                {
                    // weapon name
                    if (wb.weaponInfo.eRange == Range.Melee)
                    {
                        uiWeaponName.color = Color.red;
                    }
                    else
                    {
                        uiWeaponName.color = Color.blue;
                    }

                    iTween.ScaleFrom(uiWeaponName.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    uiWeaponName.text = weapon.GetComponent<SpriteRenderer>().sprite.name;
                }
            }
        }
    }
}