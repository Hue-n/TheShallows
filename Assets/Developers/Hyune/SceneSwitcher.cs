using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    public DefaultControls controls;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            controls = new DefaultControls();

            controls.Controller.SceneForward.performed += ctx => SwitchSceneForward();
            controls.Controller.SceneBackward.performed += ctx => SwitchSceneBackwards();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    public void SwitchSceneForward()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        sceneIndex = Mathf.Clamp(sceneIndex, 0, SceneManager.sceneCountInBuildSettings - 1);
        if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("OOB");
        }
    }

    public void SwitchSceneBackwards()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        sceneIndex = Mathf.Clamp(sceneIndex, 0, SceneManager.sceneCountInBuildSettings - 1);
        if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("OOB");
        }
    }
}
