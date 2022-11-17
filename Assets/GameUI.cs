using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questReq;
    public GameObject captainsLogUI;

    public DefaultControls controls;

    public List<Quest> questList;
    public List<Quest.State> stateList;
    public List<int> objList;
    public List<GameObject> logList;
    public int currentQuest = 0;

    
    public void Awake()
    {
        controls = new DefaultControls();
        controls.Controller.Log.performed += ctx => ToggleCaptainsLog();

    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    public void ToggleCaptainsLog()
    {
        Debug.Log("CLog Toggle");
        if (captainsLogUI.activeSelf)
        {
            captainsLogUI.GetComponent<CapLogAnim>().Toggle();
            Time.timeScale = 1f;
        }
        else
        {
            captainsLogUI.GetComponent<CapLogAnim>().Toggle();
            Time.timeScale = 0f;
        }
    }

    public void UpdateUI()
    {
        //Update Current UI
        questTitle.text = questList[currentQuest].QuestName;

        if (stateList[currentQuest] == Quest.State.returning)
        {
            questReq.text = questList[currentQuest].returnText;
        }
        else
        questReq.text = questList[currentQuest].QuestRequirements + " (" + objList[currentQuest] + "/" + questList[currentQuest].objectiveMax + ")";

        if (stateList[currentQuest] == Quest.State.complete)
        {
            currentQuest += 1;
            UpdateUI();
            //Debug.Log("Quest List " + currentQuest);
        }

        int count = 0;
        //Update Captains Log
        foreach (GameObject obj in logList)
        {
            obj.GetComponent<QuestLog>().UpdateLog(objList[count]);
        }

    }

    public void AddQuest(GameObject questLog, Quest quest)
    {
        questList.Add(quest);
        logList.Add(questLog);
        objList.Add(0);
        stateList.Add(Quest.State.notStarted);
        UpdateUI();
        questLog.GetComponent<QuestLog>().SetQuest(quest);
    }

    public void UpdateWaveCounter(int wave)
    {
        score.text = "Wave: " + wave;
    }

    void OnGUI()
    {
        //GUILayout.Label(stateList[currentQuest].ToString());
    }
}
