using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "KI/NPCStats", fileName = "new_NPC_Stats")]
public class NPCStats : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveRadius;
    [SerializeField] private float sightRadius;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float health;
    [SerializeField] private LayerMask enemyLayer;
    public float MoveSpeed => moveSpeed;
    public float MoveRadius => moveRadius;
    public float SightRadius => sightRadius;
    public float AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public float Health => health;
    public LayerMask EnemyLayer => enemyLayer;

}
