using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : Enemy
{
    public CreatureStats enemyStats;
    private bool Attacking = false;
    private bool Rotate = false;
    private float chargeForce = 100;
    private float attackCooldown = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        
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

        
        //frontSensorPos = enemyStats.sensorPos;
        //sensorLength = enemyStats.sensorLength;
        
        //set navmesh stats

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > attackRange)
        {
            //Sensors();
        }
        else
        {
            if (!Attacking)
            {
                StartCoroutine(RammingSpeed());
            }
        }

        if (Rotate)
        {
            //HuntPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage(10);
        }
    }

    private IEnumerator RammingSpeed()
    {
        Attacking = true;
        Rotate = true;

        alert.Alerter(3);
        yield return new WaitForSeconds(3);

        rb.AddForce(transform.forward * chargeForce, ForceMode.Impulse);
        Rotate = false;

        GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DisableBox());

        StartCoroutine(AttackCooldown());
        yield break;
    }

    private IEnumerator DisableBox()
    {
        yield return new WaitForSeconds(2);

        GetComponent<BoxCollider>().enabled = false;

        yield break;
    }

    private IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(attackCooldown);

        Attacking = false;
        yield break;
    }
}
