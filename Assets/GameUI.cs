using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;
    public DefaultControls controls;

    //UI Objects
    public TextMeshProUGUI score;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questReq;
    public GameObject captainsLogUI;
    
    //Variables
    public List<Quest> questList;
    public List<Quest.State> stateList;
    public List<int> objList;
    public List<GameObject> logList;
    public int currentQuest = 0;
    public bool isLogActive = false;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        controls = new DefaultControls();
        controls.Controller.Log.performed += ctx => ToggleCaptainsLog();
        //controls.Controller.QuestSelect.performed += ctx => SelectQuest(ctx.ReadValue<Vector2>());
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
        if (!isLogActive)
        {
            captainsLogUI.SetActive(true);
        }
        isLogActive = !isLogActive;
        captainsLogUI.GetComponent<CapLogAnim>().Toggle();
        
    }
    
    public void SetActiveQuest(int questToSelect)
    {
        currentQuest = questToSelect - 1;
        UpdateUI();
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
            logList[currentQuest].GetComponent<QuestLog>().MarkCompleted();
            UpdateUI();
            for (int i = 0; i < stateList.Count; i++)
            {
                if (stateList[i] != Quest.State.complete)
                {
                    currentQuest = i;
                }
            }
            //Debug.Log("Quest List " + currentQuest);
        }

        int count = 0;
        //Update Captains Log
        foreach (GameObject obj in logList)
        {
            Debug.Log(obj);
            obj.GetComponent<QuestLog>().UpdateLog(objList[count]);
            count++;
        }

    }

    public void AddQuest(GameObject questLog, Quest quest)
    {
        questList.Add(quest);
        logList.Add(questLog);
        objList.Add(0);
        stateList.Add(Quest.State.notStarted);
        UpdateUI();
        questLog.GetComponent<QuestLog>().SetQuest(quest, logList.Count);
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
