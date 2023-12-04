using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool first = true;

    void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    
}
