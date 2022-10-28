using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    public GameObject playerInstance;
    public GameObject mainCameraInstance;
    public GameObject focalPointInstance;

    public float startingMusicVol = 1f;
    public float startingSFXVol = 1f;
    public float startingVoiceVol = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize Volume
        AudioManager.SetChannelVolume(0, startingMusicVol);
        AudioManager.SetChannelVolume(1, startingSFXVol);
        AudioManager.SetChannelVolume(2, startingVoiceVol);
    }
}
