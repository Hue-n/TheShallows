using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] SpawnableEnemies;
    [SerializeField] private Transform[] SpawnLocations;
    [SerializeField] private float spawnFrequency = 5;

    private bool active = false;
    private bool timer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active && !timer)
        {
            StartCoroutine(SpawnTimer());
            timer = true;
        }
    }

    public IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnFrequency);

        SpawnEnemy();
        timer = false;

        yield break;
    }

    void SpawnEnemy()
    {
        //Randomely Select an enemy
        GameObject enemy = SpawnableEnemies[Random.Range(0, SpawnableEnemies.Length)];

        //Spawn it at a random location
        Instantiate(enemy, SpawnLocations[Random.Range(0, SpawnLocations.Length)]);
    }
}
