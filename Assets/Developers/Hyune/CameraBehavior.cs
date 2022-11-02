using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public FocalPoint focalPoint;
    public Transform playerTransform;

    public float smoothTime = 0.15f;
    
    private Vector3 currentVel;

    private Vector3 offset;

    private void Start()
    {
        focalPoint = GameManager.Instance.focalPointInstance.GetComponent<FocalPoint>();
        playerTransform = GameManager.Instance.playerInstance.transform;
        offset = transform.position - playerTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
        transform.position = smoothedPosition;

        // desired rotation = lookRotation to the focalPoint
        Quaternion desiredRotation = Quaternion.LookRotation(focalPoint.transform.position - transform.position, Vector3.up);
        Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothTime);
        transform.rotation = smoothedRotation;
    }
}
