using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_NPC_Stats")]
public class NPCStats : ScriptableObject
{
    public float moveSpeed;
    public float sightRadius;
    public float hearingRadius;
    public float moveRadius;
    public float attackDamage;
    public float attackRange;
}
