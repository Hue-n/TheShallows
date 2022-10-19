using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangedEnemy", menuName = "EnemyStats")]
public class CreatureStats : ScriptableObject
{
    public string creatureName;
    public int points;
    public int maxHP;
    public float spd;
    public float rotSpd;
    public float damage;
    public float attackRange;

}
