using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelTutorial
{
public class ButtonCheck : MonoBehaviour
{

    [SerializeField] private buttonTrigger button;
    public GameObject message;
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
            message.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = sprite;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
}
