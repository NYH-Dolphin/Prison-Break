using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text Instructions;
    public Image actorImage;
    public TMP_Text actorName;
    public TMP_Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    private PlayerController pc;
    private PlayerWeapon pw;
    Actor actorToDisplay;
    Message messageToDisplay;

    public bool isActive = false;
    private Coroutine displayLineCoroutine;
    private bool canContinue = false;
    private bool fill = false;


    void Awake()
    {
        backgroundBox.transform.localScale = Vector3.zero;
        pc = GameObject.Find("[Player]").gameObject.GetComponent<PlayerController>();
        pw = GameObject.Find("[Player]").gameObject.GetComponent<PlayerWeapon>();
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.25f);
        pc.enabled = false;
        pw.enabled = false;
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            backgroundBox.LeanScale(Vector3.zero, 0.25f);
            isActive = false;
            pc.enabled = true;
            pw.enabled = true;
        }
    }

    void DisplayMessage()
    {
        messageToDisplay = currentMessages[activeMessage];
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        displayLineCoroutine = StartCoroutine(DisplayLine(messageToDisplay.message));

        actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

        switch (actorName.text)
        {
            case "Player":
                messageText.color = new Color(.1f, .7f, .9f);
                messageText.color = Color.white;
                break;
            case "Enemy":
                messageText.color = Color.red;
                actorName.color = Color.red;
                break;
            case "Guide":
                messageText.color = new Color(.15f, .7f, .1f);
                actorName.color = new Color(.15f, .7f, .1f);
                break;
            default:
                messageText.color = Color.white;
                actorName.color = Color.white;
                break;
        }
    }

    void Update()
    {
        if (isActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
                         Input.GetMouseButtonDown(0))) //change to input system
        {
            if (canContinue)
                NextMessage();
            else
                fill = true;
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        messageText.text = "";

        canContinue = false;
        Instructions.enabled = false;

        bool richText = false;

        foreach (char letter in line.ToCharArray())
        {
            if (fill)
            {
                messageText.text = line;
                fill = false;
                break;
            }

            if (letter == '<' || richText)
            {
                richText = true;
                messageText.text += letter;
                if (letter == '>')
                {
                    richText = false;
                }
            }
            else
            {
                messageText.text += letter;
                yield return new WaitForSeconds(0.03f);
            }
        }

        canContinue = true;
        Instructions.enabled = true;
    }
}