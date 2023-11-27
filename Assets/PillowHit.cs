using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowHit : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool enter = true;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void Update()
    {
        if(this.transform.parent.tag == "Player" && enter)
        {
            StartDialogue();
            enter = false;
        }
    }

}

