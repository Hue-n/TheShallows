using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapLogAnim : MonoBehaviour
{
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
        }
        else
        {
            anim.SetTrigger("Close");
        }
    }   
}
