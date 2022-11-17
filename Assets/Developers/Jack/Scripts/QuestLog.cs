using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.Controls;

public class QuestLog : MonoBehaviour
{
    private TextMeshProUGUI TitleText;
    private TextMeshProUGUI DescriptionText;
    private TextMeshProUGUI RequirementText;

    private int objMax;
    private string reqText;
    // Start is called before the first frame update
    void Awake()
    {
        //Set the variables
        TitleText = GetComponent<TextMeshProUGUI>();
        DescriptionText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RequirementText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetQuest(Quest data)
    {
        objMax = data.objectiveMax;
        reqText = data.QuestRequirements;
        TitleText.text = data.QuestName;
        DescriptionText.text = data.QuestDescription;
        RequirementText.text = reqText + " (0/"+ objMax +")";
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
}
