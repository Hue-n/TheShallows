using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class NPC : MonoBehaviour
{
    
    public DefaultControls controls;

    //Variables
    private Quest quest;
    [Header("Fill These Out:")]
    [Tooltip("Put Quest Scr Obj here")]
    public Quest InitQuest;
    private Dialogue[] dialogues;
    private GameObject uiObject;
    [Tooltip("Put Dialogue Scr Obj here")]
    public GameObject dialogueObject;
    

    [Tooltip("Fetchables & Enemy Spawn Objects")]
    public GameObject[] QuestObject;

    private bool inRange = false;
    private bool questAdded = false;

    [Tooltip("Assign quest container object")]
    [SerializeField] private GameObject questContainer;
    [Tooltip("Assign QuestUI prefab")]
    [SerializeField] private GameObject questPrefab;
    [Header("Auto-Filled:")]
    public int questID;

    public void Awake()
    {

        controls = new DefaultControls();

        controls.Controller.Attack.performed += ctx => SpeakToNPC();
       
        quest = ScriptableObject.CreateInstance<Quest>();
        quest = InitQuest;

        uiObject = transform.GetChild(0).transform.GetChild(0).gameObject;
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
            if (!dialogueObject.activeSelf && !GameUI.Instance.isLogActive)
            {
                if (!questAdded)
                {
                    //Activate Fetchables & Enemything
                    foreach(GameObject obj in QuestObject)
                    {
                        obj.SetActive(true);
                    }
                    
                    // add to quest log
                    GameObject[] logs = GameObject.FindGameObjectsWithTag("Quest");
                    Vector2 newLoc = new Vector2(-70, 380 + (-200 * logs.Length));
                    GameObject newQuest = Instantiate(questPrefab, questContainer.transform);
                    newQuest.GetComponent<RectTransform>().anchoredPosition = newLoc;
                    GameUI.Instance.AddQuest(newQuest, quest);
                    questID = GameUI.Instance.GetComponent<GameUI>().questList.Count-1;
                    questAdded = true;
                }
                //add quest & switch to this one
                switch(GameUI.Instance.stateList[questID])
                {
                    case Quest.State.notStarted:
                        {
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.startDialogue);

                            //UnityEngine.Debug.Log("Intro Dialogue");
                            GameUI.Instance.stateList[questID] = Quest.State.inProgress;
                            break;
                        }
                    case Quest.State.inProgress:
                        {
                            //UnityEngine.Debug.Log("In Progress Talk");
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.midquestDialogue);
                            break;
                        }
                    case Quest.State.returning:
                        {
                            //UnityEngine.Debug.Log("Finish Talk");
                            dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(quest.finishDialogue);
                            GameUI.Instance.stateList[questID] = Quest.State.complete;
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCon_KrakenQuest>().fbAmmo += quest.FireballReward;
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCon_KrakenQuest>().souls += quest.SoulReward;
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCon_KrakenQuest>().SetFBAmmoText();
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCon_KrakenQuest>().SetSoulsText();

                            GameUI.Instance.UpdateUI();
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
