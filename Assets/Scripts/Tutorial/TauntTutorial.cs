using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntTutorial : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool first = true;
    public GameObject wall;
    [SerializeField] private Sprite sprite;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && first)
        {
            StartDialogue();
            wall.GetComponent<SpriteRenderer>().sprite = sprite;
            wall.GetComponent<Breakable>().enabled = true;
            first = false;
        }

    }

    void Update()
    {
        if(this.tag == "Tutorial" && pauseDone) Destroy(gameObject);
    }
}
