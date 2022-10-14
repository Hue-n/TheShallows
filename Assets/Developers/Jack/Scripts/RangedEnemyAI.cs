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
    private float rotSpeed = 1050;

    [SerializeField] public enum State
    {
        Chase,
        Avoid,
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
        currentState = State.Avoid;
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
                    rb.velocity = speed * transform.forward;

                    var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

                    PredictMovement(leadTimePercentage);

                    RotateShip();

                    break;
                }
            case State.Avoid:
                {
                    rb.velocity = speed * transform.forward;


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
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        float avoidMultiplier = 0;
        avoiding = false;
        
        // Front Right Sensors
        sensorStartPos += transform.right * frontSideSensorPos;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier -= 1f;
            Debug.DrawLine(sensorStartPos, hit.point);
        }
        
        // Front Right Angle Sensors
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier -= 0.5f;
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        // Front Left Sensors
        sensorStartPos -= transform.right * (frontSideSensorPos * 2);
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier += 1f;
            Debug.DrawLine(sensorStartPos, hit.point);
        }
        
        // Front Left Angle Sensors
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            avoiding = true;
            avoidMultiplier += 0.5f;
            Debug.DrawLine(sensorStartPos, hit.point);
        }

        sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        // Front Center Sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                avoiding = true;
                Debug.DrawLine(sensorStartPos, hit.point);
                if (hit.normal.x < 0)
                {
                    avoidMultiplier = -1f;
                }
                else
                {
                    avoidMultiplier = 1f;
                }
            }
        }

        if (avoiding)
        {
            Debug.Log("Avoiding:" + avoidMultiplier);
            rb.MoveRotation(Quaternion.Euler(transform.rotation.x, transform.rotation.y + (5 * avoidMultiplier), transform.rotation.z));
        }

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
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotSpeed * Time.deltaTime));
    }
}
