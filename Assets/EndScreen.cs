using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI waveNumber;
    public TextMeshProUGUI highScoreText;

    public AudioClip sound;
    private void Start()
    {
        waveNumber.text = "You made it to wave: " + GameManager.Instance.waveNumber;
        highScoreText.text = "Your highscore is wave: " + GameManager.Instance.highScore;

        GameManager.Instance.waveNumber = 0;

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, sound);
    }
}
