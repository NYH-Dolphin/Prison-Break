using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;
using Weapon;

public class SharpnessIndicator : MonoBehaviour
{
    private PlayerWeapon pw;
    [SerializeField] private TMP_Text sharpness;


    void Start()
    {
        pw = GameObject.Find("[Player]").GetComponent<PlayerWeapon>();
    }
    void Update()
    {

        if(pw.WeaponEquipped != null)
        {
            if(pw.WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Blunt)
                sharpness.text = "Blunt";
            else
                sharpness.text = "Sharp";
        }
        else
            sharpness.text = "None";
    }
}
