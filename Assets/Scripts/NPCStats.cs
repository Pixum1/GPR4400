using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_NPC_Stats")]
public class NPCStats : ScriptableObject
{
    [HideInInspector]
    public Vector3 startPosition;
    public float moveSpeed;
    public float sightRadius;
    public float attackDamage;
    public float attackRange;
    public LayerMask enemies;
}
