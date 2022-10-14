using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : MonoBehaviour
{
    public RangedEnemy enemyStats;
    public Rigidbody rb;

    private GameObject target;
    private float currentHP;
    private float maxHP;
    private float speed = 5;
    private float rotSpeed = 500;

    [SerializeField] public enum State
    {
        Chase,
        Attack
    }

    private State currentState;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction;

    [Header("SENSORS")]
    [SerializeField] private float sensorLength = 6f;
    [SerializeField] private Vector3 frontSensorPos = new Vector3(0, 0.2f, 0.5f);
    [SerializeField] private float frontSideSensorPos = 2f;
    [SerializeField] private float frontSensorAngle = 45f;
    private bool avoiding = false;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Chase;
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sensors();

        switch(currentState)
        {
            case State.Chase:
                {

                    

                    break;
                }
            case State.Attack:
                {

                    break;
                }
        }

        
    }

    private void Sensors()
    {
        //Move the Player
        rb.velocity = speed * transform.forward;

        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        float avoidMultiplier = 0;

        if (transform.rotation.x != 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
        }
        avoiding = false;
        
        // Front Right Sensors
        sensorStartPos += transform.right * frontSideSensorPos;
        Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                avoiding = true;
                avoidMultiplier -= 1f;
                Debug.DrawLine(sensorStartPos, hit.point);
            }
            
        }
        // Front Right Angle Sensors
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                avoiding = true;
                avoidMultiplier -= 0.75f;
            }
            
        }
        Debug.DrawRay(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, Color.blue);

        // Front Left Sensors
        sensorStartPos -= transform.right * (frontSideSensorPos * 2);
        Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                avoiding = true;
                avoidMultiplier += 1f;
                Debug.DrawLine(sensorStartPos, hit.point);
            }
        }
        // Front Left Angle Sensors
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                avoiding = true;
                avoidMultiplier += 0.75f;
                Debug.DrawLine(sensorStartPos, hit.point);
                Debug.Log("FrontLeft");

            }
        }
        Debug.DrawRay(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, Color.blue);

        sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        // Front Center Sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength/2))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                   
                    avoiding = true;
                    Debug.DrawLine(sensorStartPos, hit.point);
                }
                if (hit.normal.y < 0)
                {
                    avoidMultiplier = 3f;

                }
                else
                {
                    avoidMultiplier = -3f;

                }
            }
        }
        Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);

        if (avoiding)
        {

            Debug.Log("AvoidMult: " + avoidMultiplier);

           //angular velocity
            Vector3 angleVel = new Vector3(0, 30 * avoidMultiplier, 0);
            Quaternion deltaRotation = Quaternion.Euler(angleVel * Time.deltaTime);

            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        
        if (avoidMultiplier == 0 && !avoiding)
            HuntPlayer();

    }

    private void HuntPlayer()
    {
        Debug.Log("Hunt");

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

        PredictMovement(leadTimePercentage);

        RotateShip();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = target.GetComponent<Rigidbody>().position + target.GetComponent<Rigidbody>().velocity * predictionTime;
    }

    private void RotateShip()
    {
        var heading = _standardPrediction - transform.position;
        
        var rotation = Quaternion.LookRotation(heading);

        //JITTERY?
        Debug.DrawRay(transform.position, heading, Color.red);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotSpeed * Time.deltaTime));
    }
}
