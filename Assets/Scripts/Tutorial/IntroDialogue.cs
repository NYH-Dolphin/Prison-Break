using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroDialogue : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    [SerializeField] private GameObject fade;

    void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(messages,actors);
    }

    void Update()
    {
        if(FindObjectOfType<DialogueManager>().isActive == false)
            fade.GetComponent<NextScene>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
