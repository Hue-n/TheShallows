using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnShootingMechanic(ShootingMechanicData data);

public enum ShootStates
{ 
    idle = 0,
    targeting,
    skillchecking
}

public class ShootingMechanic : MonoBehaviour
{
    public static event OnShootingMechanic OnShootingMechanic;

    public float correctTime = 0.5f;
    public float errorTime = 0.15f;

    public FocalPoint focalPoint;

    public List<EnemyTracker> enemyList = new List<EnemyTracker>();

    private ShootStates currentState = ShootStates.idle;

    private DefaultControls controls;

    public int currentTarget = 0;

    private bool userInput = false;

    private bool inputCooldown = false;

    public Coroutine shootingMinigame = null;

    private void Awake()
    {
        controls = new DefaultControls();

        // add control delegate subscriptions here
        controls.Controller.Targeting.performed += ctx => OnScanButton();
        controls.Controller.ChooseTarget.performed += ctx => OnTargetChange(ctx.ReadValue<Vector2>());
        controls.Controller.Attack.performed += ctx => OnAttackButton();
    }

    private void Start()
    {
        focalPoint = GameManager.Instance.focalPointInstance.GetComponent<FocalPoint>();
    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ShootStates.idle:
                if (Input.GetKeyDown(KeyCode.X))
                {
                    OnScanButton();
                }
                break;
            case ShootStates.targeting:
                // constantly check if there are enemies in range.
                if (CheckForEnemy())
                {
                    // Always keep the accessor clamped to evade a OOB
                    int targetTracker = currentTarget;
                    currentTarget %= enemyList.Count;
                    if (currentTarget != targetTracker)
                    {
                        ChangeTarget();
                    }
                    
                    #region Keyboard Controls
                    // Loop through list, rotate camera to target.
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        currentTarget--;
                        currentTarget = Mathf.Abs(currentTarget) % enemyList.Count;
                        focalPoint.SetFocalPoint(enemyList[currentTarget].gameObject);
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        currentTarget++;
                        currentTarget = Mathf.Abs(currentTarget) % enemyList.Count;
                        focalPoint.SetFocalPoint(enemyList[currentTarget].gameObject);
                    }

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        // put the focus back on the player
                        focalPoint.SetFocalPoint(gameObject);
                        currentState = ShootStates.idle;
                        currentTarget = 0;
                    }
                    #endregion Keyboard Controls
                }

                else
                {
                    // put the focus back on the player
                    focalPoint.SetFocalPoint(gameObject);
                    currentState = ShootStates.idle;
                    // Reset Accessor
                    currentTarget = 0;
                }

                break;
            case ShootStates.skillchecking:
                break;
        }
    }

    public ShootStates GetCurrentState()
    {
        return currentState;
    }

    public void SetCurrentState(ShootStates newState)
    {
        currentState = newState;

        ShootingMechanicData data = new ShootingMechanicData();
        data.currentState = newState;

        OnShootingMechanic?.Invoke(data);
    }
    
    // Auxillary Function to help change the target.
    public void ChangeTarget()
    {
        focalPoint.SetFocalPoint(enemyList[Mathf.Abs(currentTarget) % enemyList.Count].gameObject);
        ShootingMechanicData data = new ShootingMechanicData();

        data.package = ShootingMechanicCommands.UpdateTarget;
        data.targetReference = enemyList[Mathf.Abs(currentTarget) % enemyList.Count].gameObject.transform;

        OnShootingMechanic?.Invoke(data);
    }

    void OnTargetChange(Vector2 input)
    {
        switch (currentState)
        {
            case ShootStates.idle:
                break;
            case ShootStates.targeting:
                if (!inputCooldown)
                {
                    if (input.x > 0)
                    {
                        currentTarget++;
                        ChangeTarget();
                    }
                    else if (input.x < 0)
                    {
                        currentTarget--;
                        ChangeTarget();
                    }
                    else
                    {
                        // Nothing
                    }

                    StartCoroutine(InputCooldown());

                }
                break;
            case ShootStates.skillchecking:
                break;
        }
    }

    void OnScanButton()
    {
        switch (currentState)
        {
            case ShootStates.idle:
                if (CheckForEnemy())
                {
                    SetCurrentState(ShootStates.targeting);
                    ChangeTarget();
                }
                break;
            case ShootStates.targeting:

                // Reset Accessors
                focalPoint.SetFocalPoint(gameObject);
                currentTarget = 0;
                SetCurrentState(ShootStates.idle);

                break;
            case ShootStates.skillchecking:
                // Nothing.
                break;
        }
    }

    // Check if the list is empty
    bool CheckForEnemy()
    {
        if (enemyList.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddToEList(EnemyTracker inst)
    {
        enemyList.Add(inst);
    }

    public void RemoveFromList(EnemyTracker inst)
    {
        enemyList.Remove(inst);
    }

    public void OnAttackButton()
    {
        switch (currentState)
        {
            case ShootStates.idle:
                // Nothing
                break;
            case ShootStates.targeting:
                // Start Shooting Minigame
                shootingMinigame = StartCoroutine(ShootingMinigame());
                SetCurrentState(ShootStates.skillchecking);
                break;
            case ShootStates.skillchecking:
                userInput = true;
                // End Minigame
                break;
        }
    }

    public IEnumerator InputCooldown()
    {
        inputCooldown = true;

        float cooldownTime = 0.25f;
        float currentTime = 0;

        while (currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        inputCooldown = false;
    }

    private void SendMinigameData(float currentValue)
    {
        ShootingMechanicData data = new ShootingMechanicData();
        data.package = ShootingMechanicCommands.UpdateMinigameValue;
        data.currentMinigameValue = currentValue;

        OnShootingMechanic?.Invoke(data);
    }

    private IEnumerator ShootingMinigame()
    {
        // win condition: hit correctly
        // lose condition: miss
        // break condition: timeout

        // summary: a timer that the player can stop with the 'A' button. A value is also randomly chosen from between 0 & 100 to iterate as well.
        // When the player presses 'A', the timer will stop and the value will be run through a cosine. If that value is within the error range of 0.5, the player
        // hits the target. If it doesn't, they miss. If the player times out, the game exits and notifies the UI of this as well.

        // call delegate that the shooting minigame started

        userInput = false;

        float maxTime = 2f;
        float timer = 0f;

        float currentValue = Random.Range(0, 100);

        while (timer < maxTime && !userInput)
        {
            timer += Time.deltaTime;
            currentValue += Time.deltaTime;
            SendMinigameData(currentValue);

            yield return null;
        }

        // Case: Player Timed Out
        if (timer > maxTime)
        {
            focalPoint.SetFocalPoint(gameObject);
            SetCurrentState(ShootStates.idle);
        }

        else
        {
            // check the player's value and see if they hit it correctly.
            if ((Mathf.Abs(Mathf.Cos(currentValue)) >= correctTime - errorTime) && (Mathf.Abs(Mathf.Cos(currentValue)) <= correctTime + errorTime))
            {
                // Case: Player Wins!
                focalPoint.SetFocalPoint(gameObject);
                SetCurrentState(ShootStates.idle);
                enemyList[Mathf.Abs(currentTarget) % enemyList.Count].Die();
            }
            else
            {
                // Case: Player Loses!
                focalPoint.SetFocalPoint(gameObject);
                SetCurrentState(ShootStates.idle);
                Debug.Log("YOU MISSED!");
            }
        }
    }

}