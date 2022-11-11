using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetchable : MonoBehaviour
{
    public Quest quest;
    public bool inRange;

    public DefaultControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => PickUp();
    }
    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
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

            FindObjectOfType<GameUI>().UpdateUI();

            if (quest.objectiveCur >= quest.objectiveMax)
            {
                quest.State = 2;
                FindObjectOfType<GameUI>().UpdateUI();
            }

            Destroy(gameObject);
        }
    }
}
