using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelTutorial
{
public class ButtonCheck : MonoBehaviour
{

    [SerializeField] private buttonTrigger button;

    [SerializeField] private Sprite sprite;

    // Update is called once per frame
    void Update()
    {
        CheckButton();
    }

    void CheckButton()
    {
        if(!button.hint.activeSelf)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
}
