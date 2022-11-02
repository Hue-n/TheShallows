using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI score;

    public void UpdateWaveCounter(int wave)
    {
        score.text = "Wave: " + wave;
    }
}
