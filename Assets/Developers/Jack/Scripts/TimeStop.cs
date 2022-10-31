using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public delegate void OnTimeStop(TimeStopData data);

public class TimeStop : MonoBehaviour
{
    public static event OnTimeStop OnTimeStop;

    public DefaultControls controls;

    public ShootingMechanic mechanic;


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
        // Check if the time stop is ready
        foreach(Enemy boi in FindObjectsOfType<Enemy>())
        {
            boi.StopTime();
        }
        
        Debug.Log("Time Stopped");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
