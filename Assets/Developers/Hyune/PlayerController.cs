using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public DefaultControls controls;

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

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    private void Awake()
    {
        controls = new DefaultControls();

        controls.Controller.Movement.performed += ctx => DebugLog();
        controls.Controller.Movement.performed += ctx => input = new Vector3(ctx.ReadValue<Vector2>().x, 0,
            ctx.ReadValue<Vector2>().y);
    }

    private void OnDestroy()
    {
        controls.Controller.Movement.performed -= ctx => DebugLog();
        controls.Controller.Movement.performed -= ctx => input = new Vector3(ctx.ReadValue<Vector2>().x, 0,
            ctx.ReadValue<Vector2>().y);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = transform.forward;
    }

    private void Update()
    {
        //input = new Vector3(gamepad.leftStick.ReadValue().x, 0, gamepad.leftStick.ReadValue().y);

        currentTurn += input.x * turnScalar;
        currentTurn = Mathf.Clamp(currentTurn, -maxTurn, maxTurn);

        currentSpeed += input.z * speedScalar;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        moveDir = transform.forward * currentSpeed;
    }

    public void DebugLog()
    {
        Debug.Log(input);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveDir;
        rb.angularVelocity = new Vector3(0, currentTurn, 0);
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
