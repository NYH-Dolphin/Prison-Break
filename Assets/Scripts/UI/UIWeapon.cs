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
        [SerializeField] private TextMeshProUGUI uiWeaponSharpness;
        [SerializeField] private List<Image> uiWeaponNums;
        [SerializeField] private Sprite texNull;
        private PlayerWeapon _pw;

        private string weaponName;
        private RectTransform rt;
        private Vector3 endPosition;

        private void Start()
        {
            _pw = GameObject.Find("[Player]").GetComponent<PlayerWeapon>();
            rt = uiWeaponSharpness.gameObject.GetComponent<RectTransform>();
            endPosition = rt.anchoredPosition;
        }

        private void Update()
        {
            UIWeaponInfoUpdate();
        }


        void UIWeaponInfoUpdate()
        {
            if (_pw.WeaponEquipped == null)
            {

                endPosition.y = 440;
                rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, endPosition, 3f * Time.deltaTime);
                if (uiWeaponSharpness.text != "NONE")
                {
                    weaponName = "none";
                    uiWeaponSharpness.text = "NONE";
                    iTween.ScaleFrom(uiWeaponSharpness.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    uiWeaponSharpness.color = Color.white;

                    for (int i = 0; i < 3; i++)
                    {
                        uiWeaponNums[i].sprite = texNull;
                    }
                }
            }
            else
            {
                endPosition.y = 360;
                rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, endPosition, 3f * Time.deltaTime);
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

                if (weaponName != wsr.name)
                {
                
                    // weapon name
                    if (wb.weaponInfo.eSharpness == Sharpness.Sharp)
                    {
                        uiWeaponSharpness.color = Color.red;
                    }
                    else
                    {
                        uiWeaponSharpness.color = Color.blue;
                    }

                    iTween.ScaleFrom(uiWeaponSharpness.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    weaponName = weapon.GetComponent<SpriteRenderer>().sprite.name;
                    if(wb.weaponInfo.eSharpness == Sharpness.Blunt) 
                        uiWeaponSharpness.text = "Blunt";
                    else
                        uiWeaponSharpness.text = "Sharp";
                }
            }
        }
    }
}