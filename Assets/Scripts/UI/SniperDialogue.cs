using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public Sniper sniper;
    [SerializeField] private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dialogue());
    }

    private IEnumerator Dialogue()
    {
        pc.enabled = false;
        sniper.active = false;
        yield return new WaitForSeconds(2f);
        StartDialogue();
        yield return new WaitForSeconds(2f);
        NextDialogue();
        yield return new WaitForSeconds(2f);
        NextDialogue();
        yield return new WaitForSeconds(2f);
        sniper.active = true;
        pc.enabled = true;
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
