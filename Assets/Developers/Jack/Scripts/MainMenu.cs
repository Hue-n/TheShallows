using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public GameObject credits;
    public AudioClip TitleMusic;
    
    public void Start()
    {
        AudioManager.Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        settings.SetActive(false);
        credits.SetActive(false);
        menu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
