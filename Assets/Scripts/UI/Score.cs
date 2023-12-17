using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using UnityEngine.UI;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    public static Score Instance;
    private string currentWeaponName = "";
    private string lastWeapon = "";
    private string currentWeaponBehaviour = "";
    private string lastWeaponBehaviour = "";

    private float weaponMultiplier = 1;
    private float behaviourMultiplier = 1;
    private int attack;
    private float addition;
    private int score;
    private GameObject weapon;
    private Image outside;
    private Image inside;
    private TMP_Text display;
    private RectTransform displayTransform;
    private Vector3 endTransform;
    private Vector3 beginTransform;
    private bool active = false;
    public float levelScoreLimit;
    public string grade = "D";

    private bool fused;

    [SerializeField] private Sprite[] grades;
    [SerializeField] private Sprite[] filledGrades;
    

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        outside = GameObject.Find("/UI/Grade").GetComponent<Image>();
        inside = GameObject.Find("/UI/GradeFilled").GetComponent<Image>();
        display = GameObject.Find("/UI/MultiplierDisplay").GetComponent<TMP_Text>();
        displayTransform = display.transform.GetComponent<RectTransform>();
        endTransform = displayTransform.anchoredPosition;
        endTransform.y = -100f;
        beginTransform = displayTransform.anchoredPosition;
        beginTransform.y = -35f;
    }

    public void Attack(GameObject weaponEquipped)
    {
        weapon = weaponEquipped;
        currentWeaponName = weapon.GetComponent<SpriteRenderer>().sprite.name;
        switch(weapon.GetComponent<WeaponBehaviour>().weaponInfo.eAttackType)
        {
            case AttackType.Swing:
                currentWeaponBehaviour = "Swing";
                break;
            case AttackType.Throw:
                currentWeaponBehaviour = "Throw";
                break;
            case AttackType.Lob:
                currentWeaponBehaviour = "Lob";
                break;
            case AttackType.Slam:
                currentWeaponBehaviour = "Slam";
                break;
            case AttackType.Thrust:
                currentWeaponBehaviour = "Thrust";
                break;
            case AttackType.Boomerang:
                currentWeaponBehaviour = "Boomerang";
                break;
        }
        Multiplier();
        Calculator();
        lastWeapon = currentWeaponName;
        lastWeaponBehaviour = currentWeaponBehaviour;
        weapon = null;
    }

    private void Multiplier()
    {
        if(currentWeaponName != lastWeapon)
        {
            weaponMultiplier *= 1.1f;
            if(currentWeaponBehaviour != lastWeaponBehaviour)
            {
                behaviourMultiplier *= 1.1f;
            }
            else
                behaviourMultiplier = 1;
        }
        else
        {
            behaviourMultiplier = 1;
            weaponMultiplier = 1;
        }
            
    }

    private void Calculator()
    {
        if(weapon.GetComponent<WeaponBehaviour>().weaponInfo.bFused)
        {
            if(weapon.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Sharp)
                attack = 80;
            else
                attack = 70;
            fused = true;
        }
        else
        {
            if(weapon.GetComponent<WeaponBehaviour>().weaponInfo.eSharpness == Sharpness.Sharp)
                attack = 60;
            else
                attack = 50;
            fused = false;
        }
        addition = attack*weaponMultiplier*behaviourMultiplier;
        score += (int) addition;
        active = true;
        displayTransform.anchoredPosition = beginTransform;
    }

    public void Stomp()
    {
        attack = 30;
        score += attack;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(score < levelScoreLimit)
        {
            outside.sprite = grades[3];
            inside.sprite = filledGrades[3];
            inside.fillAmount = score/levelScoreLimit;
            grade = "D";
            display.color = new Color(.8f,.25f,.2f);
        }
        else if(score < 2 * levelScoreLimit)
        {
            outside.sprite = grades[2];
            inside.sprite = filledGrades[2];
            inside.fillAmount = (score-levelScoreLimit)/levelScoreLimit;
            grade = "C";
            display.color = new Color(.6f,.4f,.1f);
        }
        else if(score < 3 * levelScoreLimit)
        {
            outside.sprite = grades[1];
            inside.sprite = filledGrades[1];
            inside.fillAmount = (score- 2*levelScoreLimit)/levelScoreLimit;
            grade = "B";
            display.color = new Color(.9f,.85f,.1f);
        }
        else
        {
            outside.sprite = grades[0];
            inside.sprite = filledGrades[0];
            inside.fillAmount = (score- 3*levelScoreLimit)/levelScoreLimit;
            grade = "A";
            display.color = new Color(.25f,.7f,0f);
        }

        if(active)
        {
            displayTransform.anchoredPosition = Vector3.Lerp(displayTransform.anchoredPosition, endTransform, 3f * Time.deltaTime);
            if(fused)
                display.text = "New Weapon Multiplier: " + weaponMultiplier.ToString("F2") + Environment.NewLine + "New Behavior Multiplier: " + behaviourMultiplier.ToString("F2") + Environment.NewLine + "Fused";
            else
                display.text = "New Weapon Multiplier: " + weaponMultiplier.ToString("F2") + Environment.NewLine + "New Behavior Multiplier: " + behaviourMultiplier.ToString("F2");
        }
        if(displayTransform.anchoredPosition.y <= endTransform.y + 2)
        {
            active = false;
            display.text = "";
        }
        
    }
}
