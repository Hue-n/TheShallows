using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUILocation : MonoBehaviour
{
    [SerializeField] private Transform UILoc;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(UILoc.position);
    }
}
