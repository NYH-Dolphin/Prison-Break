using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPipeTutorial : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool first = true;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void Start()
    {
        StartDialogue();

    }
}
