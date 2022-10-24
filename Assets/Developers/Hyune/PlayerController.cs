using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public DefaultControls controls;

    public float maxHP = 100;
    public float currentHP;
    private bool regen = false;
    [SerializeField] private float regenCooldown = 5;
    [SerializeField] private float regenRate = 2f;

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
        currentHP = maxHP;
    }

    private void Update()
    {
        //input = new Vector3(gamepad.leftStick.ReadValue().x, 0, gamepad.leftStick.ReadValue().y);

        currentTurn += input.x * turnScalar;
        currentTurn = Mathf.Clamp(currentTurn, -maxTurn, maxTurn);

        currentSpeed += input.z * speedScalar;
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        moveDir = transform.forward * currentSpeed;

        if (currentHP < maxHP && regen)
        {
            if (currentHP + regenRate < maxHP)
                currentHP += regenRate * Time.deltaTime;
            else
                currentHP = maxHP;
        }
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

    public void Damage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            Debug.Log("G A M E  O V E R");

        StartCoroutine(RegenCooldown());
    }

    private IEnumerator RegenCooldown()
    {
        regen = false;

        yield return new WaitForSeconds(regenCooldown);

        regen = true;

        yield break;
    }
}
