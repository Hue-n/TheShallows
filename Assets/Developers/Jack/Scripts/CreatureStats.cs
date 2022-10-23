using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature Enemy", menuName = "CreatureStats")]
public class CreatureStats : ScriptableObject
{
    public string creatureName;
    public int points;
    public int maxHP;
    public float spd;
    public float rotSpd;
    public float damage;
    public float attackRange;
    public Vector3 sensorPos;
    public float sensorLength;
}
