using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnShootingMechanic(ShootingMechanicData data);

public enum ShootStates
{ 
    idle = 0,
    targeting,
    skillchecking,
    deathgague,
}

public class ShootingMechanic : MonoBehaviour
{
    public bool canShoot = true;

    public static event OnShootingMechanic OnShootingMechanic;

    public float correctTime = 0.5f;
    public float errorTime = 0.15f;

    public float bulletTime = 0.5f;

    public FocalPoint focalPoint;

    public List<EnemyTracker> enemyList = new List<EnemyTracker>();
    public Transform currentTargetTrans;

    private ShootStates currentState = ShootStates.idle;

    private DefaultControls controls;

    public int currentTarget = 0;

    private bool userInput = false;

    private bool inputCooldown = false;

    public Coroutine shootingMinigame = null;

    public List<Transform> cannonPoints = new List<Transform>();
    public GameObject smoke;

    public AudioClip CannonFire;

    public AudioClip CannonMiss;

    public bool deathGague;
    public float currentDeathGague;
    public float maxDeathGague = 100;

    public GameObject defaultPP;
    public GameObject deathPP;

    //Fireball Variables
    public bool fireballMode = false;


    private void Awake()
    {
        controls = new DefaultControls();

        // add control delegate subscriptions here
        controls.Controller.Targeting.performed += ctx => OnScanButton();
        controls.Controller.ChooseTarget.performed += ctx => OnTargetChange(ctx.ReadValue<Vector2>());
        controls.Controller.Attack.performed += ctx => OnAttackButton();

        controls.Controller.DeathGague.performed += ctx => OnDeathGague();
    }

    public void OnDeathGague()
    {
        if (!deathGague && currentDeathGague != 0 && currentState == ShootStates.idle)
        {
            deathGague = true;
            SetCurrentState(ShootStates.deathgague);
            deathPP.SetActive(true);
            defaultPP.SetActive(false);
        }
        else
        {
            deathGague = false;
            SetCurrentState(ShootStates.idle);
            deathPP.SetActive(false);
            defaultPP.SetActive(true);
        }
    }

    private void Start()
    {
        focalPoint = FindObjectOfType<FocalPoint>();
    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }
    #region Hyune's Regular Shooting Code
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

            case ShootStates.deathgague:
                if (CheckInDeathGague())
                {
                    // Deiterate Death Gague
                    currentDeathGague -= Time.deltaTime * 4;

                    if (CheckForEnemy())
                    {
                        ChangeTarget();

                        // Always keep the accessor clamped to evade a OOB
                        int targetTracker = currentTarget;
                        currentTarget %= enemyList.Count;
                        if (currentTarget != targetTracker)
                        {
                            ChangeTarget();
                        }
                    }
                    else
                    {
                        // put the focus back on the player
                        focalPoint.SetFocalPoint(gameObject);
                    }
                }
                else
                {
                    // put the focus back on the player
                    focalPoint.SetFocalPoint(gameObject);
                    SetCurrentState(ShootStates.idle);
                    // Reset Accessor
                    currentTarget = 0;
                    deathGague = false;
                    deathPP.SetActive(false);
                    defaultPP.SetActive(true);
                }
                break;
        }
    }

    private bool CheckInDeathGague()
    {
        if (currentDeathGague <= 0)
        {
            return false;
        }
        else
        {
            return true;
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
        currentTargetTrans = enemyList[Mathf.Abs(currentTarget) % enemyList.Count].gameObject.transform;
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
            case ShootStates.deathgague:
                if (CheckForEnemy())
                {
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
                }
                break;
        }
    }

    void OnScanButton()
    {
        if(canShoot)
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
            case ShootStates.deathgague:
                // Nothing.
                break;
        }
    }

    // Check if the list is empty
    public bool CheckForEnemy()
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
            case ShootStates.deathgague:
                Vector3 targetCannon = CalculateCannonPos();

                // Death & VFX Logic
                enemyList[Mathf.Abs(currentTarget) % enemyList.Count].Die(targetCannon);
                Instantiate(smoke, targetCannon, Quaternion.identity);

                //Sound effects
                AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, CannonFire, 1f);
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
                //Case: Player Wins!
                focalPoint.SetFocalPoint(gameObject);
                SetCurrentState(ShootStates.idle);

                // Death Gague
                currentDeathGague += 5;

                Vector3 targetCannon = CalculateCannonPos();

                // Death & VFX Logic
                enemyList[Mathf.Abs(currentTarget) % enemyList.Count].Die(targetCannon);
                Instantiate(smoke, targetCannon, Quaternion.identity);

                //Sound effects
                AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, CannonFire, 1f);

            }
            else
            {
                // Case: Player Loses!
                focalPoint.SetFocalPoint(gameObject);
                SetCurrentState(ShootStates.idle);
                Debug.Log("YOU MISSED!");

                Vector3 targetCannon = CalculateCannonPos();

                // VFX Logic
                enemyList[Mathf.Abs(currentTarget) % enemyList.Count].Miss(targetCannon);
                Instantiate(smoke, targetCannon, Quaternion.identity);

                //Sound effects
                AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, CannonFire, 1f);
            }
        }
    }

    public Vector3 CalculateCannonPos()
    {
        // Do a distance check from each cannon point to the target and the closest one is the one you shoot from.
        int leastAccessor = 0;
        float least = 100;

        for (int i = 0; i < 4; i++)
        {
            float compareDistance = (enemyList[Mathf.Abs(currentTarget) % enemyList.Count].transform.position - cannonPoints[i].transform.position).magnitude;

            if (compareDistance < least)
            {
                least = compareDistance;
                leastAccessor = i;
            }
        }

        return cannonPoints[leastAccessor].transform.position;
    }
    #endregion

    #region Jack's Fireball Code
    public void ToggleFireball()
    {
        fireballMode = !fireballMode;
        if (fireballMode)
        {
            canShoot = false;
            SetCurrentState(ShootStates.idle);
        } else
        {
            canShoot = true;
        }

    }



    #endregion
}