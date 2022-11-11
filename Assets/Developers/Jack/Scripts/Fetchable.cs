using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetchable : MonoBehaviour
{
    public Quest quest;
    public bool inRange;

    public DefaultControls controls;

    public void AssignQuest(Quest assignment)
    {
        quest = assignment;
    }

    // Start is called before the first frame update
    void Start()
    {
        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => PickUp();
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
    
    public void PickUp()
    {
        if (inRange)
        {
            quest.objectiveCur += 1;

            if (quest.objectiveCur >= quest.objectiveMax)
            {
                quest.State = 2;
            }

            Destroy(gameObject);
        }
    }
}
