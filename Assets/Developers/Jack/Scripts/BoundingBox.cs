using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class BoundingBox : MonoBehaviour
{
    private bool OutofBounds = false;
    public GameObject UIobj;
    public TextMeshProUGUI counter;
    private float timer = 5;

    // Start is called before the first frame update
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OutofBounds = true;
            timer = 5;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OutofBounds = false;
        }
    }


    private void Update()
    {
        if (OutofBounds)
        {
            UIobj.SetActive(true);
            timer -= 1 * Time.deltaTime;
            timer = Mathf.Round(timer);

            counter.text = timer.ToString();
        }
    }

}
