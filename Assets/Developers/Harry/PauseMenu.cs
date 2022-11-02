using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public Animator animator;

    public bool SettingsIsOpen = false;

    public bool PauseIsOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
              
                animator.SetBool("CloseAll", true);
                Resume();

                if (SettingsIsOpen == true)
                {
                    animator.SetBool("SettingsIsOpen", false);
                    animator.SetBool("CloseAll", true);
                }
                
            }
            else
            {
                
                Pause();

            }
        }
    }

    public void Resume()
    {
 
        Time.timeScale = 1f;
        GameIsPaused = false;
        animator.SetBool("PauseIsOpen", false);

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        animator.SetBool("PauseIsOpen", true);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        animator.SetBool("SettingsIsOpen", true);
        animator.SetBool("PauseIsOpen", false);
        SettingsIsOpen = true;
    }

    public void CloseSettings()
    {
        animator.SetBool("SettingsIsOpen", false);
        animator.SetBool("PauseIsOpen", true);
        animator.SetBool("CloseAll", false);
        SettingsIsOpen = false;

    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

}
