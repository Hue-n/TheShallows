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

    [SerializeField]
    public enum State
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
        agent.stoppingDistance = attackRange - (attackRange / 3);

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < attackRange && currentState != State.Attack)
        {
            Debug.Log("in range");
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
                    Debug.Log("state is attack");
                    if (!firing)
                    {
                        StartCoroutine(Fire());
                        Debug.Log("fire coroutine");
                    }
                }

                break;
        }
    }

    private IEnumerator AttackMode()
    {
        yield return new WaitForSeconds(3f);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < attackRange)
        {
            Debug.Log("set state to attack");
            currentState = State.Attack;
        }

        yield break;
    }

    private IEnumerator Fire()
    {
        Debug.Log("fire");
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
            target.GetComponent<PlayerController>().Damage(500000);
            yield return new WaitForSeconds(1f);

        }
        currentState = State.Chase;
        firing = false;
        yield break;
    }



}