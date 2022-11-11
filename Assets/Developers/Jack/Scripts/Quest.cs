using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Details")]
    public string QuestName = "Default Quest";
    public string QuestDescription = "Journey to the thing with the stuff.";
    [Header("Quest Stats")]
    public GameObject EnemyTrigger;
    public GameObject[] Fetchables;
    [SerializeField] public enum QuestType { Fetch, Kill, Other }
    public int FireballReward = 0;
    public int SoulReward = 0;
    [Header("Quest Dialogue")]
    public Dialogue startDialogue;
    public Dialogue midquestDialogue;
    public Dialogue finishDialogue;
    public enum state{notStarted, inProgress, returning, complete }

}
