using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void DefDialogueCaster(Dialogue dialogue, TMP_Text textbox);

public class DialogueTrigger : MonoBehaviour
{
    public static event DefDialogueCaster dialogueCaster;

    public GameObject dialogueBox;
    public Dialogue heldDialogue;
    public TMP_Text textbox;
    public bool wasTriggered = false;

    [SerializeField] bool dialogueEvent = false;

    bool canTalk = false;
    [SerializeField] bool inDialogue = false;

    private void Start()
    {
        DialogueSystem.OnDialogueFinish += OnExitDialogue;
    }

    private void OnDestroy()
    {
        DialogueSystem.OnDialogueFinish -= OnExitDialogue;
    }

    private void OnExitDialogue()
    {
        inDialogue = false;
        dialogueBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogueEvent && !wasTriggered)
            {
                inDialogue = true;
                wasTriggered = true;
                dialogueBox.SetActive(true);
                dialogueCaster?.Invoke(heldDialogue, textbox);
            }
            else if (!dialogueEvent && !wasTriggered)
            { 
                canTalk = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canTalk && !inDialogue)
        {
            inDialogue = true;
            dialogueBox.SetActive(true);
            dialogueCaster?.Invoke(heldDialogue, textbox);
        }
    }
}
