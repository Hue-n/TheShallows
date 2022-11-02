using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Experimental.AI;
using static Cinemachine.CinemachineOrbitalTransposer;

public class ShipAI : Enemy
{
    private bool firing = false;
    public ShipStats enemyStats;

    [SerializeField] public enum State
    {
        Chase,
        Attack
    }

    private State currentState;
    // Start is called before the first frame update
    void Start()
    {
        
        currentState = State.Chase;
        target = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(target.transform.position);

        rb = GetComponent<Rigidbody>();
        alert = GetComponentInChildren<EnemyUIAlert>();
        if (enemyStats != null)
        SetStats();
    }

    private void SetStats()
    {
        currentHP = maxHP;
        maxHP = enemyStats.maxHP;
        agent.speed = enemyStats.spd;
        agent.angularSpeed = enemyStats.rotSpd;
        attackRange = enemyStats.attackRange;
        frontSensorPos = enemyStats.sensorPos;
        sensorLength = enemyStats.sensorLength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < attackRange && currentState != State.Attack)
        {
            StartCoroutine(AttackMode());
        }

        switch (currentState)
        {
            case State.Chase:
                {
                    //Sensors();
                    

                    break;
                }
            case State.Attack:
                {
                    
                    if (isTurning)
                    {
                        
                        float lDist = Vector3.Distance(leftCheck.position, target.transform.position);
                        float rDist = Vector3.Distance(rightCheck.position, target.transform.position);

                        if (lDist < rDist)
                        {
                            

                            if (!firing)
                            {
                                StartCoroutine(Fire());
                            }
                        }
                        else
                        {
                            

                            if (!firing)
                            {
                                StartCoroutine(Fire());
                            }
                        }
                    }

                    break;
                }
        }

        
    }
    private IEnumerator AttackMode()
    {
        yield return new WaitForSeconds(3f);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < attackRange)
        {
            currentState = State.Attack;
            isTurning = true;
        }
        yield break;
    }

    private IEnumerator Fire()
    {
        firing = true;
        alert.Alerter(3);
        yield return new WaitForSeconds(3f);
               
        if (Random.Range(0, 10) <= 2)
        {
            Debug.Log("Miss");
            
        }
        else
        {
            //Put Screenshake Here
            //Damage the Player
            target.GetComponent<PlayerController>().Damage(5);
            yield return new WaitForSeconds(1f);
            
        }
        currentState = State.Chase;
        firing = false;
        yield break;
    }

}
