using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocalPoint : MonoBehaviour
{
    [HideInInspector] public GameObject focus;
    [HideInInspector] public CameraBehavior behavior;

    private void Start()
    {
        focus = GameManager.Instance.playerInstance;
        behavior = GameManager.Instance.mainCameraInstance.GetComponent<CameraBehavior>();
    }

    // Snaps to target
    // Update is called once per frame
    void Update()
    {
        transform.position = focus.transform.position;
    }

    // Setter
    public void SetFocalPoint(GameObject arg)
    {
        focus = arg;
    }
}
