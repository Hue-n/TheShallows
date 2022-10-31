using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseScreenScript : MonoBehaviour
{
  public void RestartButton()
  {
    SceneManager.LoadScene("WaveSurvival");
  }

  public void MainMenuButton()
  {
    SceneManager.LoadScene("MainMenu");
  }

  public void QuitButton ()
  {
    Application.Quit();
  }
}
