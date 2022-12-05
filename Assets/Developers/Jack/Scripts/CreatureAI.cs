using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CreatureAI : Enemy
{
    public CreatureStats enemyStats;
    private bool Attacking = false;
    private float chargeForce = 125;
    private float attackCooldown = 3;

    public BoxCollider collidey;
    public BoxCollider trigger;

    [Range(0f, 1f)]
    public float dropRate;
    public List<GameObject> drops;

    public bool isKrak;

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

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > attackRange)
        {

        }
        else
        {
            if (!Attacking)
            {
                StartCoroutine(RammingSpeed());
            }
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

        alert.Alerter(3);
        yield return new WaitForSeconds(1);

        rb.AddForce(transform.forward * chargeForce, ForceMode.Impulse);

        trigger.enabled = true;
        collidey.enabled = false;
        StartCoroutine(DisableBox());

        StartCoroutine(AttackCooldown());
        yield break;
    }

    private IEnumerator DisableBox()
    {
        yield return new WaitForSeconds(2);

        trigger.enabled = false;
        collidey.enabled = true;

        yield break;
    }

    private IEnumerator AttackCooldown()
    {

        yield return new WaitForSeconds(attackCooldown);

        Attacking = false;
        yield break;
    }
    
    void Drop() {
        float roll;
        roll = Random.Range(0f,1f);
        Debug.Log(roll);
        if (roll < dropRate) {
            //Instantiate(drops[Random.Range(0,drops.Count-1)], transform.position, transform.rotation);
            Instantiate(drops[Random.Range(0,drops.Count-1)], rb.position + Vector3.up * 0.5f, Quaternion.identity);
        }
    }

    public void OnDestroy()
    {
        FindObjectOfType<ScoreKeeper>().AddScore(enemyStats.points);
        Drop();

        if (isKrak)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
