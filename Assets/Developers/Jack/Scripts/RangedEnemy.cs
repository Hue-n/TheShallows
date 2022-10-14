using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangedEnemy", menuName = "EnemyStats")]
public class RangedEnemy : ScriptableObject
{
    public string shipName;
    public Mesh mesh;
    public MeshCollider collider;
    public int maxHP;
    public int maxSpeed;
    public int turnSpeed;
    public int damage;

}
