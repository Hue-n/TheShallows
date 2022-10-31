using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCut : MonoBehaviour
{


    // Update is called once per frame
    void OnBecameInvisible()
    {
        GetComponent<WaterManger>().enabled = false;
        //GetComponent<WaveManager>().enabled = false;


    }

    private void OnBecameVisible()
    {
        GetComponent<WaterManger>().enabled = true;
        //GetComponent<WaveManager>().enabled = true;


    }
}
