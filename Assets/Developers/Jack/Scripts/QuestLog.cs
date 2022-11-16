using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    private TextMeshProUGUI TitleText;
    private TextMeshProUGUI DescriptionText;
    private TextMeshProUGUI RequirementText;
    private Quest questInfo;

    // Start is called before the first frame update
    void Start()
    {
        //Set the variables
        TitleText = GetComponent<TextMeshProUGUI>();
        DescriptionText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        RequirementText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetQuest(Quest data, int objectives)
    {
        if (objectives == questInfo.objectiveMax)
        {
            TitleText.color = Color.green;
            DescriptionText.color = Color.green;
            RequirementText.color = Color.green;

        }

        questInfo = data;
        TitleText.text = questInfo.QuestName;
        DescriptionText.text = questInfo.QuestDescription;
        RequirementText.text = questInfo.QuestRequirements + " (" + objectives + "/" + questInfo.objectiveMax + ")";
    }
}
