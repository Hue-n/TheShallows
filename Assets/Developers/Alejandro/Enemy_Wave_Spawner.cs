using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enemy_Wave_Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    public GameObject enemyWaveSystem;
    
    [System.Serializable]

    public class Wave
    {
        public string name;

        public GameObject enemy;

        public int count;

        public float rate;
    }
    
    // [System.Serializable]
    
    // public class Wave
    // {
    //     public string name;
    
    //     public List <WaveAction> actions;
    // }

    private int nextWave = 0;

    private int newWave;

    public Transform[] spawnPoints;

    public List <Wave> waves;

    public float timeBetweenWaves;

    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("NO SPAWN POINTS REFERENCED");
        }

        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }

            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine( SpawnWave ( waves[nextWave] ) );
            }
        }

        else
        {
            waveCountdown -= Time.deltaTime;
        }

        //transform.Rotate (new Vector3 (0,15,0) * Time.deltaTime);
    }

    void WaveCompleted()
    {
        Debug.Log ("WAVE COMPLETED");

        state = SpawnState.COUNTING;

        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Count - 1)
        {
            // Wave newWave = new Wave[waves.enemy, waves.rate, waves.count];
            
            // nextWave = 0;

            NewWave();
        }

        else
        {
            nextWave++;
        }
    }
    void NewWave()
    {
        Wave newWave = new Wave{waves.name, waves.enemy, waves.rate, waves.count};
    }

    // void FreezeGame()
    // {
    //     Time.timeScale = 0;
    // }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);

            yield return new WaitForSeconds(1f/wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Debug.Log ("SPAWNING ENEMY: " + enemy.name);

        Transform sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];

        Instantiate (enemy, sp.position, sp.rotation);
    }

    public void StartNextWave()
    {
        waveCountdown = 3;
        
        return;
    }
}