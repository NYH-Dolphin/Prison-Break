using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowHit : MonoBehaviour
{
    public DialogueTrigger trigger;
    private bool enter = true;

    void Update()
    {
        if(this.transform.parent.tag == "Player" && enter)
        {
            trigger.StartDialogue();
            enter = false;
        }
    }

    // void OnTriggerEnter(Collider col)
    // {
    //     if(col.tag == "Breakable");
    //         Destroy(gameObject);
    // }
}
