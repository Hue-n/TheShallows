using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class NPC : MonoBehaviour
{

    public DefaultControls controls;

    public Quest quest;
    public string charName;
    public Dialogue[] dialogues;
    public GameObject uiObject;
    public GameObject dialogueObject;
    public bool inRange = false;

    public void Awake()
    {
        controls = new DefaultControls();

        controls.Controller.Attack.performed += ctx => SpeakToNPC();
    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);

    }

    public void SpeakToNPC()
    {
        if (inRange)
        {
            if (!dialogueObject.activeSelf)
            {
                

                switch(quest.State)
                {
                    case 0:
                        {
                            quest.State = 1;
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.startDialogue);
                            break;
                        }
                    case 1:
                        {
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.midquestDialogue);
                            break;
                        }
                    case 2:
                        {
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.finishDialogue);
                            quest.State = 3;
                            quest = null;
                            break;
                        }
                }

                dialogueObject.SetActive(true);
                dialogueObject.GetComponent<KQ_Dialogue>().StartDialogue();
            }  
        }
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            uiObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            uiObject.SetActive(false);
        }
    }
}
