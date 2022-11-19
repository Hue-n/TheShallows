using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameUI UIcontroller;

    //Variables
    [Tooltip("Deactivated Enemies to activate")]
    public GameObject[] EnemyList;
    private int enemyNum;
    [Tooltip("Assign NPC who gave quest")]
    public NPC questGiver;
    private bool spawned = false;

    private void Awake()
    {
        UIcontroller = GameUI.Instance;
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
                UIcontroller.objList[questGiver.questID] += 1;

                UIcontroller.stateList[questGiver.questID] = Quest.State.returning;
                FindObjectOfType<GameUI>().UpdateUI();


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
