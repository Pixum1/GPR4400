using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class StateMachine : MonoBehaviour
{
    public NPCStats stats;

    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public float moveRadius;
    [HideInInspector]
    public float sightRadius;
    [HideInInspector]
    public float attackDamage;
    [HideInInspector]
    public float attackRange;
    [HideInInspector]
    public float attackSpeed;
    [HideInInspector]
    public float health;
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public LayerMask enemyLayer;

    [SerializeField]
    public GameObject bulletPrefab;

    protected BaseState currentState;

    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public NavMeshAgent agent;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetValues();
        startPosition = transform.position;
    }
    public virtual void Start()
    {
        currentState = GetInitialState();

        if (currentState != null)
            currentState.Enter();
    }

    public virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();

        CheckDeath();
    }

    public void ChangeState(BaseState _newState)
    {
        currentState.Exit();

        currentState = _newState;
        currentState.Enter();
    }

    public virtual BaseState GetInitialState()
    {
        return null;
    }

    private void GetValues()
    {
        moveSpeed = stats.MoveSpeed;
        moveRadius = stats.MoveRadius;
        sightRadius = stats.SightRadius;
        attackDamage = stats.AttackDamage;
        attackRange = stats.AttackRange;
        enemyLayer = stats.EnemyLayer;
        attackSpeed = stats.AttackSpeed;
        health = stats.Health;
    }
    public virtual void CheckDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
