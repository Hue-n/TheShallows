using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{

    public DefaultControls controls;
    public Quest quest;
    public string charName;
    public Dialogue[] dialogues;
    public GameObject uiObject;
    public GameObject dialogueObject;
    private bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        uiObject = GetComponentInChildren<Image>().gameObject;
        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => SpeakToNPC();
    }

    public void SpeakToNPC()
    {
        if (inRange)
        {
            if (!dialogueObject.activeSelf)
            {
                dialogueObject.SetActive(true);
                dialogueObject.GetComponent<KQ_Dialogue>().AssignDialogue(dialogues[0]);
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
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
