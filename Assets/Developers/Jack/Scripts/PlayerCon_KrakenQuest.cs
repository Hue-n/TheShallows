using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PlayerCon_KrakenQuest : MonoBehaviour
{
    public DefaultControls controls;

    public float maxHP = 100;
    public float currentHP;

    public float maxSpeed;
    public float maxTurn;

    public float turnScalar = 0.05f;
    public float speedScalar = 0.05f;

    public bool canTakeColDMG = true;

    //public bool isMoving;

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

        controls.Controller.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
    }

    private void OnDestroy()
    {
        controls.Controller.Movement.performed -= ctx => OnMovement(ctx.ReadValue<Vector2>());
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

        moveDir = transform.forward * currentSpeed;

    }

    public void OnMovement(Vector3 input)
    {
        // Only calculate boat movement if the player's move state is idle.

        if (GameManager.Instance.playerInstance.GetComponent<ShootingMechanic>().GetCurrentState() == ShootStates.idle)
        {

            Vector3 inputVal = new Vector3(input.x, 0, input.y);

            currentTurn += inputVal.x * turnScalar;
            currentTurn = Mathf.Clamp(currentTurn, -maxTurn, maxTurn);

            currentSpeed += inputVal.z * speedScalar;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
    }

    public void DebugLog()
    {
        //Debug.Log(input);
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

        GetComponent<TimeStop>().AddCharge(15);

        if (currentHP <= 0)
        {
            // Check the spawner
            GameManager.Instance.CheckHighScore(GameManager.Instance.spawner.waveCounter);

            Debug.Log("G A M E  O V E R");
            SceneManager.LoadScene("LoseScreen");
        }

        Debug.Log("Player took "+amount+" damage");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canTakeColDMG)
        {
            Damage(5);
            canTakeColDMG = false;
            StartCoroutine(ColDamageCooldown());
        }

    }

    private IEnumerator ColDamageCooldown()
    {
        yield return new WaitForSeconds(3);

        canTakeColDMG = true;

        yield break;
    }


}
