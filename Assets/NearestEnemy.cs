using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestEnemy : MonoBehaviour
{
    public Enemy_Wave_Spawner spawner;

    public GameObject arrow;

    private void Start()
    {
        spawner = GameManager.Instance.spawner.GetComponent<Enemy_Wave_Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawner.enemiesList.Count > 0)
        {
            arrow.SetActive(true);
            Vector3 focus = spawner.enemiesList[0].transform.position - GameManager.Instance.playerInstance.transform.position;
            transform.rotation = (Quaternion.LookRotation(focus));
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
