using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class WeaponUIInfo : MonoBehaviour
{
    private WeaponUIManager wUI;
    // Start is called before the first frame update
    void Awake()
    {
        wUI = GameObject.Find("UI/Weapon UI").GetComponent<WeaponUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount > 0)
        {
            var weapin = transform.GetChild(0).GetComponent<WeaponBehaviour>().weaponInfo;
            if(weapin.eRange == Range.Melee)
            {
                wUI.MeleeShow(transform.GetChild(0).GetComponent<SpriteRenderer>().sprite, weapin.iDurability);
                wUI.RangedHide();
            } 
            else
            {
                wUI.RangedShow(transform.GetChild(0).GetComponent<SpriteRenderer>().sprite, weapin.iDurability);
                wUI.MeleeHide();
            }
                
        }
        else
        {
            wUI.RangedHide();
            wUI.MeleeHide();
        }
    }
}
