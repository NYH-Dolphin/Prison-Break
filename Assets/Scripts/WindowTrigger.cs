using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool first = true;
    [SerializeField] Collider collider;
    [SerializeField] Sprite sprite;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && first)
        {
            StartDialogue();
            collider.enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = sprite;
            first = false;
        }

    }
}