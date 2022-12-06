using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class PlayerCon_KrakenQuest : MonoBehaviour
{
    public DefaultControls controls;

    public GameObject questContainer;
    public GameObject defaultQuest;
    public Quest quest;
    public int questID;

    public float maxHP = 100;
    public float currentHP;

    public int fbAmmo = 0;
    public TMP_Text fbAmmoText;

    public int souls = 0;
    public TMP_Text soulsText;

    public float maxSpeed;
    public float maxTurn;

    public float turnScalar = 0.05f;
    public float speedScalar = 0.05f;

    public bool canTakeColDMG = true;

    public bool fireballUnlocked;
    public bool lanternUnlocked;

    public bool isDamned;
    public float timeDamned = 5f;
    public float damnedTimer;

    public AudioClip Lantern;

    public AudioClip ItemPickup;

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
        controls.Controller.Lantern.performed += ctx => ActivateLantern();
    }

    private void OnDestroy()
    {
        controls.Controller.Movement.performed -= ctx => OnMovement(ctx.ReadValue<Vector2>());
        controls.Controller.Lantern.performed += ctx => ActivateLantern();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDir = transform.forward;
        currentHP = maxHP;
        Cursor.visible = false;

        SetFBAmmoText();
        SetSoulsText();

        GameObject[] logs = GameObject.FindGameObjectsWithTag("Quest");
        Vector2 newLoc = new Vector2(-70, 280 + (-100 * logs.Length));
        GameObject newQuest = Instantiate(defaultQuest, questContainer.transform);
        newQuest.GetComponent<RectTransform>().anchoredPosition = newLoc;
        GameUI.Instance.AddQuest(newQuest, quest);
        questID = GameUI.Instance.GetComponent<GameUI>().questList.Count - 1;
    }

    private void Update()
    {
        moveDir = transform.forward * currentSpeed;

        

        if (isDamned)
        {
            damnedTimer -= Time.deltaTime;
            canTakeColDMG = false;
            // GetComponent<Collider>(false);
            maxSpeed = maxSpeed * 2;
            maxTurn = maxTurn * 2;
            if (damnedTimer < 0)
                isDamned = false;
                canTakeColDMG = true;
                maxSpeed = maxSpeed / 2;
                maxTurn = maxTurn / 2;
        }
    }

    public void ActivateLantern()
    {
        if (souls >= 1)
        {
                isDamned = true;
                damnedTimer = timeDamned;
                souls = souls - 1;
                SetSoulsText();
                AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, Lantern, 1f);
        }
    }

    public void OnMovement(Vector3 input)
    {
        // Only calculate boat movement if the player's move state is idle.

        if (KQGameManager.Instance.playerInstance.GetComponent<ShootingMechanic>().GetCurrentState() == ShootStates.idle)
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "FBAmmo")
        {
            fbAmmo = fbAmmo + 1;
            SetFBAmmoText();

            Destroy(collider.gameObject);
            AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, ItemPickup, 1f);
        }

        if (collider.tag == "Health")
        {
            if (currentHP < maxHP)
            {
                currentHP = currentHP + 25f;

                Destroy(collider.gameObject);
                AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, ItemPickup, 1f);
            }
        }

        if (collider.tag == "Souls")
        {
            souls = souls + 1;
            SetSoulsText();

            Destroy(collider.gameObject);
            AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, ItemPickup, 1f);
        }
    }

    public void SetFBAmmoText()
    {
        fbAmmoText.text = "Fireball Rounds: " + fbAmmo.ToString();
    }

    public void SetSoulsText()
    {
        soulsText.text = "Souls: " + souls.ToString();
    }

}
