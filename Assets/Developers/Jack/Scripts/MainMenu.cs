using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject credits;
    public GameObject controls;
    public AudioClip TitleMusic;
    
    public void Start()
    {
        AudioManager.Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("WaveSurvival");
    }

    public void controlsMenu()
    {
        settings.SetActive(false);
        credits.SetActive(false);
        controls.SetActive(true);
        menu.SetActive(false);
    }
    
    public void creditsMenu()
    {
        settings.SetActive(false);
        credits.SetActive(true);
        controls.SetActive(false);
        menu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        settings.SetActive(false);
        credits.SetActive(false);
        menu.SetActive(true);
        controls.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
