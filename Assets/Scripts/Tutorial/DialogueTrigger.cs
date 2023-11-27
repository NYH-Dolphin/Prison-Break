using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    private bool pauseDone = false;
    private bool first = true;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" && first)
        {
            StartDialogue();
            StartCoroutine(ShortPause(col.gameObject.GetComponent<PlayerController>()));
            first = false;
        }

    }

    void Update()
    {
        if(this.tag == "Tutorial" && pauseDone) Destroy(gameObject);
    }

    private IEnumerator ShortPause(PlayerController pc)
    {
        pc.enabled = false;
        yield return new WaitForSeconds(1.5f);
        pauseDone = true;
        pc.enabled = true;
    }
}

[System.Serializable]
public class Message{
    public int actorID;
    public string message;
}

[System.Serializable]
public class Actor{
    public string name;
    public Sprite sprite;
}
