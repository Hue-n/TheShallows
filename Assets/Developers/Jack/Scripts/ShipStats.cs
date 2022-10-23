using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShipEnemy", menuName = "ShipStats")]
public class ShipStats : ScriptableObject
{
    public string shipName;
    public int points;
    public int maxHP;
    public float spd;
    public float rotSpd;
    public float damage;
    public float attackRange;
    public Vector3 sensorPos;
    public float sensorLength;

}
