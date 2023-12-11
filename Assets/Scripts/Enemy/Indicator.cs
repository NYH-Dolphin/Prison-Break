using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Weapon;

public class Indicator : MonoBehaviour
{
    private GameObject player;

    public Sprite fist;
    public Sprite xmark;
    public Sprite empty;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<PlayerWeapon>().WeaponEquipped != null)
        {
            if(player.GetComponent<PlayerWeapon>().WeaponEquipped.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Blunt)
                GetComponent<SpriteRenderer>().sprite = fist;
            else
                GetComponent<SpriteRenderer>().sprite = xmark;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = empty;
        }
        
    }
}
