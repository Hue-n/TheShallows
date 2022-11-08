using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KQGameManager : MonoBehaviour
{
    // Singleton
    public static KQGameManager Instance;

    public GameObject playerInstance;
    public GameObject mainCameraInstance;
    public GameObject focalPointInstance;
    public GameUI ui;

    public GameObject hitEffect;
    public GameObject missEffect;

    public float startingMusicVol = 1f;
    public float startingSFXVol = 1f;
    public float startingVoiceVol = 1f;
    public float startingAmbienceVol = 1f;

    public int highScore;

    public void CheckHighScore(int newScore)
    {
        if (newScore > highScore)
        {
            highScore = newScore;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // The duplicate passes the original all of its references before dying.
            Instance.playerInstance = playerInstance;
            Instance.mainCameraInstance = mainCameraInstance;
            Instance.focalPointInstance = focalPointInstance;
            Instance.ui = ui;

            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize Volume
        AudioManager.SetChannelVolume(0, startingMusicVol);
        AudioManager.SetChannelVolume(1, startingSFXVol);
        AudioManager.SetChannelVolume(2, startingVoiceVol);
        AudioManager.SetChannelVolume(3, startingAmbienceVol);
    }
}
