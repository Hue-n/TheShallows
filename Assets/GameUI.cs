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
    public int currentQuest = 0;

    
    public void Start()
    {
        controls = new DefaultControls();
        controls.Controller.Log.performed += ctx => ToggleCaptainsLog();

        UpdateUI();
    }

    private void OnEnable()
    {
        //controls.Controller.Enable();
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

        if (questList[currentQuest].State == 2)
        {
            questReq.text = questList[currentQuest].returnText;
        }
        else
        questReq.text = questList[currentQuest].QuestRequirements + " (" + questList[currentQuest].objectiveCur + "/" + questList[currentQuest].objectiveMax + ")";

        if (questList[currentQuest].State == 3)
        {
            currentQuest += 1;
        }
    }

    public void UpdateWaveCounter(int wave)
    {
        score.text = "Wave: " + wave;
    }
}
