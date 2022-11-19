using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;

public class QuestLog : MonoBehaviour
{
    public GameUI UIcontroller = GameUI.Instance;

    //UI Text
    private TextMeshProUGUI TitleText;
    private TextMeshProUGUI DescriptionText;
    private TextMeshProUGUI RequirementText;

    //Variables
    private int objMax;
    private string reqText;
    private int logID;

    void Awake()
    {
        UIcontroller = GameUI.Instance;

        //Set the variables
        TitleText = GetComponent<TextMeshProUGUI>();
        DescriptionText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RequirementText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetQuest(Quest data, int ID)
    {
        objMax = data.objectiveMax;
        reqText = data.QuestRequirements;
        TitleText.text = data.QuestName;
        DescriptionText.text = data.QuestDescription;
        RequirementText.text = reqText + " (0/"+ objMax +")";
        logID = ID;
    }

    public void SelectQuest()
    {
        UIcontroller.SetActiveQuest(logID);
    }

    public void UpdateLog(int updatedReq)
    {
        
        RequirementText.text = reqText + " (" + updatedReq + "/" + objMax + ")";
        
        if (updatedReq >= objMax)
        {
            //TitleText.color = Color.green;
            //DescriptionText.color = Color.green;
            //RequirementText.color = Color.green;
        }
    }

    public void Selected(bool value)
    {
        if (value)
        {
            TitleText.color = Color.red;
            DescriptionText.color = Color.red;
            RequirementText.color = Color.red;

        }  
        else
        {
            TitleText.color = Color.white;
            DescriptionText.color = Color.white;
            RequirementText.color = Color.white;
        }
        
    }
}
