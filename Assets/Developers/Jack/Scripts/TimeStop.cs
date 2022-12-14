using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTimeStop(TimeStopData data);

public class TimeStop : MonoBehaviour
{
    public static event OnTimeStop OnTimeStop;

    public DefaultControls controls;

    public ShootingMechanic mechanic;

    public int charge;

    private void Awake()
    {
        controls = new DefaultControls();

        // add control delegate subscriptions here
        controls.Controller.TimeStop.performed += ctx => OnTimeStopInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        mechanic = GetComponent<ShootingMechanic>();
    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    public void OnTimeStopInput()
    {
        if (charge == 100)
        {
            // Check if the time stop is ready
            foreach (Enemy boi in FindObjectsOfType<Enemy>())
            {
                boi.StopTime();
            }
        }
        
        Debug.Log("Time Stopped");
    }

    public void AddCharge(int amount)
    {
        if (amount + charge < 100)
            charge += amount;
        else
            charge = 100;
    }
}
