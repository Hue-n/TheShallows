using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenTrigger : MonoBehaviour
{

    public GameObject Kraken;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Kraken.SetActive(true);
        }
    }

}
