using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public Sniper sniper;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dialogue());
    }

    private IEnumerator Dialogue()
    {
        sniper.active = false;
        yield return new WaitForSeconds(2f);
        StartDialogue();
        yield return new WaitForSeconds(2f);
        NextDialogue();
        yield return new WaitForSeconds(2f);
        NextDialogue();
        yield return new WaitForSeconds(2f);
        sniper.active = true;
    }
    public void StartDialogue()
    {
        FindObjectOfType<SniperDialogueManager>().OpenDialogue(messages,actors);
    }

    public void NextDialogue()
    {
        FindObjectOfType<SniperDialogueManager>().NextMessage();
    }
}
