using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Enemy_Wave_Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    public GameObject enemyWaveSystem;

    public List<GameObject> enemiesList = new List<GameObject>();

    [System.Serializable]

    public class Wave
    {
        public string name;

        public GameObject enemy;

        public float count;

        public float rate;

        public Wave(GameObject newEnemy, float newCount, float newRate, string Tag = "")
        {
            name = Tag;

            enemy = newEnemy;

            count = newCount;

            rate = newRate;
        }
    }

    public int newRate = 2;
    
    // [System.Serializable]
    
    // public class Wave
    // {
    //     public string name;
    
    //     public List <WaveAction> actions;
    // }
    public GameObject[] enemies;

    private int nextWave = 0;

    public int waveCounter;

    private int newWave;

    public Transform[] spawnPoints = new Transform[3];

    public List<Transform> AllSpawnPoints = new List<Transform>();

    public List <Wave> waves;

    public float timeBetweenWaves;

    private float waveCountdown;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            //Debug.LogError("NO SPAWN POINTS REFERENCED");
        }

        // Find nearest spawn points to the player for the next wave.
        FindNearestSpawnPoints();

        GameManager.Instance.ui.GetComponent<GameUI>().UpdateWaveCounter(1);

        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        //Debug.Log(state);

        if (state == SpawnState.WAITING)
        {
            
            if (!EnemyIsAlive())
            {
                //Debug.Log("wave done");
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
        //Debug.Log ("WAVE COMPLETED");

        state = SpawnState.COUNTING;

        waveCountdown = timeBetweenWaves;

        waveCounter = waveCounter + 1;

        GameManager.Instance.ui.GetComponent<GameUI>().UpdateWaveCounter(waveCounter);
        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.waveStart, 1f);


        // Find nearest spawn points to the player for the next wave.
        FindNearestSpawnPoints();

        if (nextWave + 1 > waves.Count - 1)
        {
            // Wave newWave = new Wave();
            
            // nextWave = 0;

            GameObject newEnemy = enemies [Random.Range (0, enemies.Length - 1)];

            float newCount = Mathf.Round(Mathf.Pow(1.2f, waveCounter) + 10f);

            newRate += 2;

            Wave newWave = new Wave(newEnemy, newCount, newRate);

            waves.Add(newWave);

            // waves.name, waves.enemy, waves.rate, waves.count
        }

        else
        {
            nextWave++;
        }
    }
    
    // Wave()
    // {
    //     Wave newWave = new Wave[waves.name, waves.enemy, waves.rate, waves.count];
    // }

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
            //Debug.Log("spawned");
            yield return new WaitForSeconds(1f/wave.rate);
        }

        // Wave newWave = new Wave[wave.name, wave.enemy, wave.rate, wave.count];

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject enemy)
    {
        //Debug.Log("SPAWNING ENEMY: " + enemy.name);

        Transform sp = spawnPoints[Random.Range (0, spawnPoints.Length) ];

        GameObject recentObj = Instantiate(enemy, sp.position, sp.rotation);

        enemiesList.Add(recentObj);

        //Debug.Log(recentObj.transform.position);
    }

    void FindNearestSpawnPoints()
    {
        List<Transform> temp = new List<Transform>(AllSpawnPoints);

        float least = 1000000;
        int leastAccessor = 0;

        // find the spawnPoints.length's nearest spawn points
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // reset least
            least = 1000000;


            // find shortest distance
            for (int j = 0; j < temp.Count; j++)
            {
                //Debug.Log((GameManager.Instance.playerInstance.transform.position - temp[j].transform.position).magnitude);

                float compareDist = (GameManager.Instance.playerInstance.transform.position - temp[j].transform.position).magnitude;

                if (compareDist< least)
                {
                    least = compareDist;
                    leastAccessor = j;
                }
            }
            // set i's reference
            spawnPoints[i] = temp[leastAccessor];

            // remove list
            temp.RemoveAt(leastAccessor);
        }

        temp.Clear();
    }

    public void StartNextWave()
    {
        waveCountdown = 3;
        
        return;
    }
}