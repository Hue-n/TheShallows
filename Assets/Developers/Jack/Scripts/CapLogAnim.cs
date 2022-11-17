using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            anim.SetTrigger("Open");
            LogOpen = true;
            Time.timeScale = 0;
        }
        else
        {
            anim.SetTrigger("Close");
            LogOpen = false;
            Time.timeScale = 1;
        }
    }   
}
