using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tailwind : MonoBehaviour
{
    public Vector3 force;

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
