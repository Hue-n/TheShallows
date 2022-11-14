using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBAmmo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate (new Vector3 (30,15,30) * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
