using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private Image meleeImage;
    [SerializeField] private TMP_Text meleeDurability;
    [SerializeField] private Image rangedImage;
    [SerializeField] private TMP_Text rangedDurability;

    public void MeleeShow(Sprite sprite, int durability)
    {
        meleeImage.enabled = true;
        meleeDurability.enabled = true;
        meleeImage.sprite = sprite;
        meleeDurability.text = durability.ToString();
    }

    public void MeleeHide()
    {
        meleeImage.enabled = false;
        meleeDurability.enabled = false;
    }

    public void RangedShow(Sprite sprite, int durability)
    {
        rangedImage.enabled = true;
        rangedDurability.enabled = true;
        rangedImage.sprite = sprite;
        rangedDurability.text = durability.ToString();
    }

    public void RangedHide()
    {
        rangedImage.enabled = false;
        rangedDurability.enabled = false;
    }
}
