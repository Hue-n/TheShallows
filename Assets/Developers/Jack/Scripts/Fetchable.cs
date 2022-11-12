using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Fetchable : MonoBehaviour
{
    public Quest quest;
    public bool inRange;
    public GameUI UIcontroller;

    public DefaultControls controls;

    // Start is called before the first frame update
    void Awake()
    {
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
            UIcontroller.objList[UIcontroller.currentQuest] += 1;

            FindObjectOfType<GameUI>().UpdateUI();

            if (UIcontroller.objList[UIcontroller.currentQuest] >= quest.objectiveMax)
            {
                UIcontroller.stateList[UIcontroller.currentQuest] = Quest.State.returning;
                FindObjectOfType<GameUI>().UpdateUI();
            }

            Destroy(gameObject);
        }
    }
}
