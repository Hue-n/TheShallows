using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangedEnemy", menuName = "EnemyStats")]
public class ShipStats : ScriptableObject
{
    public string shipName;
    public int points;
    public int maxHP;
    public float spd;
    public float rotSpd;
    public float damage;
    public float attackRange;

}
