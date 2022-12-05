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

    public GameObject bullet;
    public GameObject shootEffect;

    public Transform bulletSpawn;
    public float bulletTime = 0.5f;

    [SerializeField]
    public enum State
    {
        Chase,
        Attack
    }

    [Range(0f, 1f)]
    public float dropRate;
    public List<GameObject> drops;

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
        yield return new WaitForSeconds(1.5f);

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
        alert.Alerter(2);
        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.cannonFire, 1f);
        Instantiate(shootEffect, bulletSpawn.position, bulletSpawn.rotation);

        if (Random.Range(0, 10) <= 2)
        {
            yield return Miss();
        }
        else
        {
            //Put Screenshake Here
            yield return Hit();
            target.GetComponent<PlayerCon_KrakenQuest>().Damage(25);
            yield return new WaitForSeconds(1f);

        }
        currentState = State.Chase;
        firing = false;
        yield break;
    }

    private IEnumerator Miss()
    {
        //Lerp a bullet
        GameObject inst = Instantiate(bullet, bulletSpawn.position, transform.rotation);

        float lerpLength = bulletTime;
        float lerpPosition = 0;

        while (lerpPosition < lerpLength)
        {
            inst.transform.position = Vector3.Lerp(bulletSpawn.position, KQGameManager.Instance.playerInstance.transform.position, lerpPosition / lerpLength);
            lerpPosition += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.cannonMiss, 1f);
        Destroy(inst);

        Instantiate(KQGameManager.Instance.missEffect, KQGameManager.Instance.playerInstance.transform.position, Quaternion.identity);
    }

    private IEnumerator Hit()
    {
        //Lerp a bullet
        GameObject inst = Instantiate(bullet, bulletSpawn.position, transform.rotation);

        float lerpLength = bulletTime;
        float lerpPosition = 0;

        while (lerpPosition < lerpLength)
        {
            inst.transform.position = Vector3.Lerp(bulletSpawn.position, KQGameManager.Instance.playerInstance.transform.position, lerpPosition / lerpLength);
            lerpPosition += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.cannonHit, 1f);
        Destroy(inst);

        Instantiate(KQGameManager.Instance.hitEffect, KQGameManager.Instance.playerInstance.transform.position, Quaternion.identity);
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
    }
}