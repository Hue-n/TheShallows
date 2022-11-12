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
    public int currentQuest = 0;

    
    public void Awake()
    {
        controls = new DefaultControls();
        controls.Controller.Log.performed += ctx => ToggleCaptainsLog();

        UpdateUI();
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
        if (captainsLogUI.activeSelf)
        {
            captainsLogUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            captainsLogUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void UpdateUI()
    {
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
            Debug.Log("Quest List " + currentQuest);
        }
    }

    public void UpdateWaveCounter(int wave)
    {
        score.text = "Wave: " + wave;
    }
}
