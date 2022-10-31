using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum TimeStopCommands
    {
        OnTimeStop = 0,
        OnChangeTarget,
        OnTimePlay
    } 

public class TimeStopData
{
    public TimeStopCommands command;
}
