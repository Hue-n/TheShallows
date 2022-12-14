using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Rigidbody rb;

    public GameObject target;
    public int currentHP;
    public int maxHP;
    //public float speed = 3;
    //public float rotSpeed = 90;
    public float attackRange = 15;
    public EnemyUIAlert alert;
    //public bool isTurning = false;
    public NavMeshAgent agent;

    public AudioClip CannonHit;

    public float aiTick = 0;

    public void Awake()
    {
        
        agent = GetComponent<NavMeshAgent>();
        
    }
    public void StopTime()
    {
        StartCoroutine(TimeStopDuration());

        if (ExtTime.timeScale == 1)
            ExtTime.timeScale = 0f;
        else
            ExtTime.timeScale = 1f;

        Debug.Log(ExtTime.timeScale);
    }

    #region stoopid bullshit
    //// Start is called before the first frame update
    //public void Sensors()
    //{
    //    //Move the Player
    //    rb.velocity = speed * transform.forward * ExtTime.timeScale;

    //    RaycastHit hit;
    //    Vector3 sensorStartPos = transform.position;
    //    sensorStartPos += transform.forward * frontSensorPos.z;
    //    sensorStartPos += transform.up * frontSensorPos.y;
    //    float avoidMultiplier = 0;
    //    avoiding = false;

    //    // Front Right Sensors
    //    sensorStartPos += transform.right * frontSideSensorPos;
    //    Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);
    //    if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //    {
    //        if (!hit.collider.CompareTag("Player"))
    //        {
    //            avoiding = true;
    //            avoidMultiplier -= 1f;
    //            //Debug.DrawLine(sensorStartPos, hit.point);
    //        }

    //    }
    //    // Front Right Angle Sensors
    //    else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
    //    {
    //        if (!hit.collider.CompareTag("Player"))
    //        {
    //            avoiding = true;
    //            avoidMultiplier -= 0.75f;
    //        }

    //    }
    //    Debug.DrawRay(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, Color.blue);

    //    // Front Left Sensors
    //    sensorStartPos -= transform.right * (frontSideSensorPos * 2);
    //    Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);
    //    if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //    {
    //        if (!hit.collider.CompareTag("Player"))
    //        {
    //            avoiding = true;
    //            avoidMultiplier += 1f;
    //            //Debug.DrawLine(sensorStartPos, hit.point);
    //        }
    //    }
    //    // Front Left Angle Sensors
    //    else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
    //    {
    //        if (!hit.collider.CompareTag("Player"))
    //        {
    //            avoiding = true;
    //            avoidMultiplier += 0.75f;
    //            //Debug.DrawLine(sensorStartPos, hit.point);

    //        }
    //    }
    //    Debug.DrawRay(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, Color.blue);

    //    sensorStartPos = transform.position;
    //    sensorStartPos += transform.forward * frontSensorPos.z;
    //    // Front Center Sensor
    //    if (avoidMultiplier == 0)
    //    {
    //        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength))
    //        {
    //            if (!hit.collider.CompareTag("Player"))
    //            {

    //                avoiding = true;
    //                //Debug.DrawLine(sensorStartPos, hit.point);
    //            }
    //            if (hit.normal.y < 0)
    //            {
    //                avoidMultiplier = 3f;

    //            }
    //            else
    //            {
    //                avoidMultiplier = -3f;

    //            }
    //        }
    //    }
    //    Debug.DrawRay(sensorStartPos, transform.forward, Color.blue);

    //    if (avoiding && ExtTime.timeScale == 1)
    //    {

    //        Debug.Log("AvoidMult: " + avoidMultiplier);

    //        //angular velocity
    //        Vector3 angleVel = new Vector3(0, 30 * avoidMultiplier, 0);
    //        Quaternion deltaRotation = Quaternion.Euler(angleVel * ExtTime.timeScale);

    //        rb.MoveRotation(rb.rotation * deltaRotation);
    //    }

    //    if (avoidMultiplier == 0 && !avoiding && ExtTime.timeScale == 1)
    //        HuntPlayer();

    //}


    //public void HuntPlayer()
    //{

    //    var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));

    //    PredictMovement(leadTimePercentage);
    //    RotateShip();
    //}

    //public void PredictMovement(float leadTimePercentage)
    //{
    //    var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

    //    _standardPrediction = new Vector3(target.GetComponent<Rigidbody>().position.x, 0, target.GetComponent<Rigidbody>().position.z) + new Vector3(target.GetComponent<Rigidbody>().velocity.x, 0, target.GetComponent<Rigidbody>().velocity.z) * predictionTime;

    //}
    #endregion

    public void Update()
    {
        aiTick += Time.deltaTime;
        if (aiTick >= 3)
        {
            agent.destination = target.transform.position;
            aiTick = 0;
        }
    }
    public void Damage(int amount)
    {
        currentHP -= amount;
        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, CannonHit, 1f);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, CannonHit, 1f);
        }
    }


    public class ExtTime
    {
        public static float timeScale = 1;
    }

    public IEnumerator TimeStopDuration()
    {
        yield return new WaitForSeconds(4);

        StopTime();

        yield break;
    }

    //private void OnDestroy()
    //{
    //    GameManager.Instance.playerInstance.GetComponent<TimeStop>().AddCharge(15);
    //}

}
