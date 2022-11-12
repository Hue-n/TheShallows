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
    public Quest InitQuest;
    public string charName;
    public Dialogue[] dialogues;
    public GameObject uiObject;
    public GameObject dialogueObject;
    public bool inRange = false;

    public GameUI UIcontroller;
    public void Awake()
    {
        controls = new DefaultControls();

        controls.Controller.Attack.performed += ctx => SpeakToNPC();
       
        quest = ScriptableObject.CreateInstance<Quest>();
        quest = InitQuest;

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

                switch(UIcontroller.stateList[UIcontroller.currentQuest])
                {
                    case Quest.State.notStarted:
                        {
                            UIcontroller.stateList[UIcontroller.currentQuest] = Quest.State.inProgress;
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.startDialogue);
                            break;
                        }
                    case Quest.State.inProgress:
                        {
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.midquestDialogue);
                            break;
                        }
                    case Quest.State.returning:
                        {
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.finishDialogue);
                            UIcontroller.stateList[UIcontroller.currentQuest] = Quest.State.complete;
                            UIcontroller.UpdateUI();
                            quest = null;
                            break;
                        }
                }

                dialogueObject.SetActive(true);
                dialogueObject.GetComponent<KQ_Dialogue>().StartDialogue();
                FindObjectOfType<FocalPoint>().SetFocalPoint(gameObject);
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
