using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{ 
    none = 0,
    raider,
    shark,
    serpent,
    crab
}

public class EnemyTracker : MonoBehaviour
{
    private ShootingMechanic player;

    public Transform transformReference;

    public EnemyTypes type = EnemyTypes.none;

    public bool isTargetable = false;

    public float targetableRange = 100f;

    public bool alive = true;

    public GameObject bullet;
    public GameObject deathAnim;

    private GameObject bulletInst;

    public float bulletTime = 1f;

    private void Start()
    {
        player = KQGameManager.Instance.playerInstance.GetComponent<ShootingMechanic>();
        bulletTime = player.bulletTime;
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

    public void Die(Vector3 bulletSpawn)
    {
        // if told to die, die.
        player.RemoveFromList(this);
        isTargetable = false;
        alive = false;

        StartCoroutine(Death(bulletSpawn));
    }

    // BLOW UP, BOOM
    private IEnumerator Death(Vector3 bulletSpawn)
    {
        //Lerp a bullet
        GameObject inst = Instantiate(bullet, bulletSpawn, Quaternion.LookRotation(transform.position - bulletSpawn));

        inst.transform.rotation = Quaternion.LookRotation(transform.position - bulletSpawn);

        float lerpLength = bulletTime;
        float lerpPosition = 0;
        
        while (lerpPosition < lerpLength)
        {
            inst.transform.position = Vector3.Lerp(bulletSpawn, transform.position, lerpPosition / lerpLength);
            lerpPosition += Time.deltaTime;
            yield return null;
        }

        Destroy(inst);

        if (type == EnemyTypes.raider)
        {
            Instantiate(deathAnim, transform.position, transformReference.rotation);
        }
        else
        { 
            Instantiate(deathAnim, transform.position, transform.rotation);
        }

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.cannonHit, 1f);
        Destroy(gameObject);
    }

    public void Miss(Vector3 bulletSpawn)
    {
        StartCoroutine(MissEvent(bulletSpawn));
    }

    private IEnumerator MissEvent(Vector3 bulletSpawn)
    {
        //Lerp a bullet
        GameObject inst = Instantiate(bullet, bulletSpawn, Quaternion.LookRotation(transform.position - bulletSpawn));

        float lerpLength = bulletTime;
        float lerpPosition = 0;

        while (lerpPosition < lerpLength)
        {
            inst.transform.position = Vector3.Lerp(bulletSpawn, transform.position, lerpPosition / lerpLength);
            lerpPosition += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, AudioManager.Instance.cannonMiss, 1f);
        Destroy(inst);

        Instantiate(GameManager.Instance.missEffect, transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        // on gameobject death, make sure to check if you're still targetable. If you are, remove yourself from the list.
        if(isTargetable)
        {
            player.RemoveFromList(this);
            isTargetable = false;
        }

        GameManager.Instance.spawner.GetComponent<Enemy_Wave_Spawner>().enemiesList.Remove(gameObject);
    }
}
