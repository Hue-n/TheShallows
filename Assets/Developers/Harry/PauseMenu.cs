using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public Animator animator;

    public bool SettingsIsOpen = false;

    public bool PauseIsOpen = false;

    public DefaultControls controls;

    public GameObject pauseFirstButton, optionsFirstButton, optionsClosedButton; 


    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    private void Awake()
    {
        controls = new DefaultControls();

        controls.Controller.Pause.performed += ctx => OnPause();
    }

    private void OnDestroy()
    {
        controls.Controller.Movement.performed -= ctx => OnPause();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        animator.SetBool("PauseIsOpen", false);
        pauseMenuUI.SetActive(false);
        //clear selected object 
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        animator.SetBool("PauseIsOpen", true);

        //clear selected object 
        EventSystem.current.SetSelectedGameObject(null);
        //set new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        animator.SetBool("SettingsIsOpen", true);
        animator.SetBool("PauseIsOpen", false);
        SettingsIsOpen = true;

        //clear selected object 
        EventSystem.current.SetSelectedGameObject(null);
        //set new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseSettings()
    {
        animator.SetBool("SettingsIsOpen", false);
        animator.SetBool("PauseIsOpen", true);
        animator.SetBool("CloseAll", false);
        SettingsIsOpen = false;

        //clear selected object 
        EventSystem.current.SetSelectedGameObject(null);
        //set new selected object
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void OnPause()
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


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

}
