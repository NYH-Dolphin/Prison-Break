using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapon;

namespace UI
{
    public class UIWeapon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI uiWeaponSharpness;
        [SerializeField] private TextMeshProUGUI uiWeaponType;
        [SerializeField] private List<Image> uiWeaponNums;
        [SerializeField] private Sprite texNull;
        
        // color for different range type
        private Color _cSharp = Color.red;
        private Color _cBlunt = new(142, 194, 226);
        
        private string _sPrevWeaponName;
        private Vector3 _vRectInitPos;
        
        

        private PlayerWeapon _pw;
        private RectTransform _rt;


        private void Start()
        {
            _pw = GameObject.Find("[Player]").GetComponent<PlayerWeapon>();
            _rt = uiWeaponSharpness.gameObject.GetComponent<RectTransform>();
            _vRectInitPos = _rt.anchoredPosition;
        }

        private void Update()
        {
            UIWeaponInfoUpdate();
        }


        void UIWeaponInfoUpdate()
        {
            if (_pw.WeaponEquipped == null)
            {
                _vRectInitPos.y = -45f;
                _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectInitPos, 3f * Time.deltaTime);
                if (uiWeaponSharpness.text != "NONE")
                {
                    _sPrevWeaponName = "none";
                    uiWeaponSharpness.text = "NONE";
                    uiWeaponType.text = "";
                    iTween.ScaleFrom(uiWeaponSharpness.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    iTween.ScaleFrom(uiWeaponType.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                    uiWeaponSharpness.color = Color.white;

                    for (int i = 0; i < 3; i++)
                    {
                        uiWeaponNums[i].sprite = texNull;
                    }
                }
            }
            else
            {
                _vRectInitPos.y = -95f;
                _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectInitPos, 6f * Time.deltaTime);
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
                
                // weapon information
                if (_sPrevWeaponName != wsr.name)
                {
                    _sPrevWeaponName = wsr.name;
                    
                    //weapon sharpness
                    if (wb.weaponInfo.eSharpness == Sharpness.Sharp)
                    {
                        uiWeaponSharpness.color = _cSharp;
                        uiWeaponSharpness.text = "Sharp";
                    }
                    else
                    {
                        uiWeaponSharpness.color = _cBlunt;
                        uiWeaponSharpness.text = "Blunt";
                    }
                    iTween.ScaleFrom(uiWeaponSharpness.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);


                    uiWeaponType.text = wb.weaponInfo.eAttackType.ToString();
                    iTween.ScaleFrom(uiWeaponType.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                }
            }
        }
    }
}