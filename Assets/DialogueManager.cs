using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public Image actorImage;
    public TMP_Text actorName;
    public TMP_Text messageText;
    public TMP_Text instructions;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;

    public static bool isActive = false;

    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one,0.25f);
    }

    public void NextMessage()
    {
        activeMessage++;
        if(activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            backgroundBox.LeanScale(Vector3.zero, 0.25f);
            isActive = false;
        }
            
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        switch (actorName.text)
        {
        case "Player":
            messageText.color = new Color(20, 172, 229);
            messageText.color = new Color(20, 172, 229);
            break;
        case "Enemy":
            messageText.color = Color.red;
            actorName.color = Color.red;
            break;
        case "Guide":
            messageText.color = Color.green;
            actorName.color = Color.green;
            break;
        default:
            messageText.color = Color.black;
            actorName.color = Color.black;
            break;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isActive)
            NextMessage();
    }
}
