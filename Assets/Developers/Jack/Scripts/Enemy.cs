using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;
    public Transform leftCheck;
    public Transform rightCheck;

    [Header("PREDICTION")]
    [SerializeField] public float _maxDistancePredict = 100;
    [SerializeField] public float _minDistancePredict = 5;
    [SerializeField] public float _maxTimePrediction = 5;
    public Vector3 _standardPrediction;

    [Header("SENSORS")]
    [SerializeField] public float sensorLength = 10f;
    [SerializeField] public Vector3 frontSensorPos = new Vector3(0, 0.2f, 0.5f);
    [SerializeField] public float frontSideSensorPos = 2f;
    [SerializeField] public float frontSensorAngle = 45f;
    public bool avoiding = false;

    public GameObject target;
    public int currentHP;
    public int maxHP;
    public float speed = 3;
    public float rotSpeed = 90;
    public float attackRange = 15;
    public EnemyUIAlert alert;
    public bool isTurning = false;

    // Start is called before the first frame update
    public void Sensors()
    {
        //Move the Player
        rb.velocity = speed * transform.forward;

        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        sensorStartPos += transform.up * frontSensorPos.y;
        float avoidMultiplier = 0;
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
                //Debug.DrawLine(sensorStartPos, hit.point);
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
                //Debug.DrawLine(sensorStartPos, hit.point);
            }
        }
        // Front Left Angle Sensors
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (!hit.collider.CompareTag("Player"))
            {
                avoiding = true;
                avoidMultiplier += 0.75f;
                //Debug.DrawLine(sensorStartPos, hit.point);

            }
        }
        Debug.DrawRay(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, Color.blue);

        sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPos.z;
        // Front Center Sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
            {
                if (!hit.collider.CompareTag("Player"))
                {

                    avoiding = true;
                    //Debug.DrawLine(sensorStartPos, hit.point);
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

    public void Damage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void HuntPlayer()
    {

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

        PredictMovement(leadTimePercentage);

        RotateShip();
    }

    public void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = new Vector3(target.GetComponent<Rigidbody>().position.x, 0, target.GetComponent<Rigidbody>().position.z) + new Vector3(target.GetComponent<Rigidbody>().velocity.x, 0, target.GetComponent<Rigidbody>().velocity.z) * predictionTime;

    }

    public void RotateShip()
    {
        //Debug.Log("Rotate() Called");
        var heading = _standardPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotSpeed * Time.deltaTime));
    }
}
