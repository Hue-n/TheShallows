using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CapLogAnim : MonoBehaviour
{
    public static bool LogOpen = false;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Toggle()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("closed"))
        {
            gameObject.SetActive(true);
            anim.SetTrigger("Open");
            LogOpen = true;
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(transform.GetChild(0).transform.GetChild(0).gameObject);
        }
        else
        {
            anim.SetTrigger("Close");
            LogOpen = false;
            Time.timeScale = 1;
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
