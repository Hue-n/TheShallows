using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //Variables
    [Tooltip("Deactivated Enemies to activate")]
    public GameObject[] EnemyList;
    private int enemyNum;
    [Tooltip("Assign NPC who gave quest")]
    public NPC questGiver;
    private bool spawned = false;

    private void Awake()
    {
        enemyNum = EnemyList.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateEnemy();
        }
    }

    public void Update()
    {
        if (spawned)
        {
            int count = 0;
            foreach (GameObject enemy in EnemyList)
            {
                if (enemy == null)
                {
                    count++;
                }

            }

            if (count == enemyNum)
            {
                //ENemies killed
                Debug.Log("Enemies Killed");
                GameUI.Instance.objList[questGiver.questID] += 1;

                GameUI.Instance.stateList[questGiver.questID] = Quest.State.returning;
                GameUI.Instance.UpdateUI();


                Destroy(gameObject);

            }
        }
        
    }

    void ActivateEnemy()
    {
        foreach(GameObject enemy in EnemyList)
        {
            enemy.SetActive(true);
            spawned = true;

        }
    }
}
