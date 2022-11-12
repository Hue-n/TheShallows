using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Details")]
    public string QuestName = "Default Quest";
    public string QuestDescription = "Journey to the thing with the stuff.";
    public string QuestRequirements = "Kill 2 fish";
    public string returnText = "return to carl for the reward";
    [Header("Quest Stats")]
    //public int objectiveCur = 0;
    public int objectiveMax;
    [Description("0 = Fetch, 1 = Kill Quest, 2 = Other")]
    public enum QuestType { Fetch, KillQuest, Other };
    public QuestType qType;
    public int FireballReward = 0;
    public int SoulReward = 0;
    [Header("Quest Dialogue")]
    public Dialogue startDialogue;
    public Dialogue midquestDialogue;
    public Dialogue finishDialogue;
    [Description("0 = not Started, 1 = in Progress, 2 = returning, 3 = Complete")]
    public enum State { notStarted, inProgress, returning, complete };
    //public State qState;

}
