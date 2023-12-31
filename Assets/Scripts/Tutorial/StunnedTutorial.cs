using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedTutorial : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool first = true;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void Update()
    {
        if(this.GetComponent<NewNav>().stunned && first)
        {

            StartDialogue();
            //StartCoroutine(ShortPause(GameObject.Find("[Player]").GetComponent<PlayerController>()));
            first = false;
        }
        
    }

    private IEnumerator ShortPause(PlayerController pc)
    {
        pc.enabled = false;
        yield return new WaitForSeconds(0.5f);
        pauseDone = true;
        pc.enabled = true;
    }
}
