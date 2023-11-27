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
            StartCoroutine(ShortPause(col.gameObject.GetComponent<PlayerController>()));
            collider.enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = sprite;
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