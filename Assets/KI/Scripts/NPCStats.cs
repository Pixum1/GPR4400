using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KI/NPCStats", fileName = "new_NPC_Stats")]
public class NPCStats : ScriptableObject
{
    private float moveSpeed;
    private float moveRadius;
    private float sightRadius;
    private float attackDamage;
    private float attackRange;
    private float attackSpeed;
    private float health;
    private LayerMask enemyLayer;
    public float MoveSpeed => moveSpeed;
    public float MoveRadius => moveRadius;
    public float SightRadius => sightRadius;
    public float AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public float Health => health;
    public LayerMask EnemyLayer => enemyLayer;

}
