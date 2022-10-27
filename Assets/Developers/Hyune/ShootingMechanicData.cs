using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingMechanicCommands
{ 
    StateChange = 0,
    UpdateMinigameValue,
    UpdateTarget,
    Hit,
    Miss
}

public class ShootingMechanicData
{
    // Must ALWAYS be set
    public ShootingMechanicCommands package = 0;

    public ShootStates currentState = 0;
    public Transform targetReference = null;
    public float currentMinigameValue = 0f;
    public bool hit = false;
    public bool missed = false;
}
