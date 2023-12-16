using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;

    private bool first = true;


    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void Update()
    {
        if(this.transform.parent != null)
        {
            
            if(this.transform.parent.tag == "holder" && first)
            {
                StartDialogue();
                first = false;
            }
        }
        
    }
}
