using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI score;
    [SerializeField] public TextMeshProUGUI questTitle;
    [SerializeField] public TextMeshProUGUI questReq;
    public GameObject captainsLogUI;

    public DefaultControls controls;
    public List<Quest> questList;
    public Quest currentQuest;

    public void Start()
    {
        controls = new DefaultControls();
        controls.Controller.TimeStop.performed += ctx => ToggleCaptainsLog();
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

    public void UpdateWaveCounter(int wave)
    {
        score.text = "Wave: " + wave;
    }
}
