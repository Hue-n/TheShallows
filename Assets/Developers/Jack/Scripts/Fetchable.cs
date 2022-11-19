using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Fetchable : MonoBehaviour
{
    private GameUI UIcontroller;
    public DefaultControls controls;

    public Quest quest;
    public NPC questGiver;
    private bool inRange;

    // Start is called before the first frame update
    void Awake()
    {
        UIcontroller = GameUI.Instance;

        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => PickUp();

        UIcontroller = FindObjectOfType<GameUI>();
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
            UIcontroller.objList[questGiver.questID] += 1;

            FindObjectOfType<GameUI>().UpdateUI();

            if (UIcontroller.objList[questGiver.questID] >= quest.objectiveMax)
            {
                UIcontroller.stateList[questGiver.questID] = Quest.State.returning;
                FindObjectOfType<GameUI>().UpdateUI();
            }

            Destroy(gameObject);
        }
    }
}
