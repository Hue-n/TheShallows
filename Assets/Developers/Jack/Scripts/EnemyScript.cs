using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public bool rangedEnemy = true;
    public float maxSpeed = 3;
    public int maxHP = 3;

    private NavMeshAgent navAgent;

    [SerializeField]
    private enum state
    {
        trackPlayer,
        shootingAtPlayer,
        rammingIntoPlayer
    }

    public void SetStats()
    {
        // Use with scriptable object to set up different ship variants
    }

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
