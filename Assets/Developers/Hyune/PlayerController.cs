using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float maxTurn;

    public float turnScalar = 0.05f;
    public float speedScalar = 0.05f;

    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentTurn;

    private Rigidbody rb;

    // the ship's actual movement
    private Vector3 moveDir;
    private Vector3 input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = transform.forward;
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        currentTurn += input.x * turnScalar;
        currentTurn = Mathf.Clamp(currentTurn, -maxTurn, maxTurn);

        currentSpeed += input.z * speedScalar;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        moveDir = transform.forward * currentSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDir;
        rb.angularVelocity = new Vector3(0, currentTurn, 0);
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
