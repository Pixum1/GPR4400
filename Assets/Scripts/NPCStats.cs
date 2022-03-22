using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_NPC_Stats")]
public class NPCStats : ScriptableObject
{
    public float moveSpeed;
    public float moveRadius;
    public float sightRadius;
    public float attackDamage;
    public float attackRange;
    public LayerMask enemyLayer;
}
