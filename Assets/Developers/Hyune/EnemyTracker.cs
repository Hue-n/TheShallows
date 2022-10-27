using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private ShootingMechanic player;

    public bool isTargetable = false;

    public float targetableRange = 100f;

    public bool alive = true;

    private void Start()
    {
        player = GameManager.Instance.playerInstance.GetComponent<ShootingMechanic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            // if the distance between the specific instance of the enemy and the instance of the player is less than the targetable range, add itself to the player instance's enemylist.
            if (((player.transform.position - transform.position).magnitude <= targetableRange) && !isTargetable)
            {
                player.AddToEList(this);
                isTargetable = true;
            }
            else if (((player.transform.position - transform.position).magnitude > targetableRange) && isTargetable)
            {
                player.RemoveFromList(this);
                isTargetable = false;
            }
            else
            {
                // enemy is either out of range & isn't targetable OR in range and is targetable. Either outcome doesn't require an action.
            }
        }
    }

    public void Die()
    {
        // if told to die, die.
        player.RemoveFromList(this);
        isTargetable = false;
        alive = false;

        StartCoroutine(Death());
    }

    // BLOW UP, BOOM
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // on gameobject death, make sure to check if you're still targetable. If you are, remove yourself from the list.
        if(isTargetable)
        {
            player.RemoveFromList(this);
            isTargetable = false;
        }
    }
}
