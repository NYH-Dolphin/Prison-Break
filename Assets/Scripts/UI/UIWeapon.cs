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

        private PlayerWeapon _pw;

        // record to check whether update the weapon information or not
        private string _sPrevWeaponName;

        // color for different range type
        private Color _cSharp = Color.red;
        private Color _cBlunt = new Color(0,.7f,1);
        
        // for weapon info animation use
        private RectTransform _rt;
        private Vector3 _vRectUpPos;
        private Vector3 _vRectDownPos;
        [HideInInspector] public string spriteName;

        private void Awake()
        {
            _pw = GameObject.Find("[Player]").GetComponent<PlayerWeapon>();
            _rt = uiWeaponSharpness.gameObject.GetComponent<RectTransform>();
            Vector3 rtPos = _rt.anchoredPosition;
            _vRectUpPos = new Vector3(rtPos.x, -35f, rtPos.z);
            _vRectDownPos = new Vector3(rtPos.x, -100f, rtPos.z);
        }

        private void Update()
        {
            UIWeaponInfoUpdate();
        }


        #region WeaponInfo

        private void UIWeaponInfoUpdate()
        {
            if (_pw.WeaponEquipped == null)
            {
                _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectUpPos, 3f * Time.deltaTime);
                EmptyWeaponInfoUpdate();
            }
            else
            {
                _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectDownPos, 6f * Time.deltaTime);
                HoldingWeaponInfoUpdate();
            }
        }

        private void EmptyWeaponInfoUpdate()
        {
            if (uiWeaponSharpness.text != "NONE")
            {
                uiWeaponSharpness.text = "NONE";
                _sPrevWeaponName = string.Empty;
                uiWeaponType.text = string.Empty;
                iTween.ScaleFrom(uiWeaponSharpness.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                iTween.ScaleFrom(uiWeaponType.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
                uiWeaponSharpness.color = Color.white;
                for (int i = 0; i < 3; i++)
                {
                    uiWeaponNums[i].sprite = texNull;
                }
            }
        }

        private void HoldingWeaponInfoUpdate()
        {
            GameObject weapon = _pw.WeaponEquipped;
            WeaponBehaviour wb = weapon.GetComponent<WeaponBehaviour>();
            Sprite wsr = weapon.GetComponent<SpriteRenderer>().sprite;
            spriteName = wsr.name;

            // weapon durability update
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

            // weapon information update
            if (_sPrevWeaponName != wsr.name)
            {
                _sPrevWeaponName = wsr.name;

                // weapon type
                uiWeaponType.text = wb.weaponInfo.eAttackType.ToString();
                iTween.ScaleFrom(uiWeaponType.gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.2f);


                // weapon sharpness
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
            }
        }

        #endregion
    }
}