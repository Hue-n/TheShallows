using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCut : MonoBehaviour
{


    // Update is called once per frame
    void OnBecameInvisible()
    {
        GetComponent<Renderer>().enabled = false;
        Debug.Log($"'{name}' is Invisible to '{Camera.current.name}'");

    }

    private void OnBecameVisible()
    {
        GetComponent<Renderer>().enabled = true;
        Debug.Log($"'{name}' is Visible to '{Camera.current.name}'");

    }
}
