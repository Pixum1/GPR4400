using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boid : MonoBehaviour
{
    private RaycastHit[] neighbours;
    private Vector3 desiredVelocity;
    [SerializeField]
    private BoidSettings settings;
    private NavMeshAgent agent;

    [SerializeField]
    private EntitySM sm;
    private float speed;
    private Vector3 currentVelocity;


    private void Start()
    {
        sm = GetComponent<EntitySM>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        speed = sm.moveSpeed;
        neighbours = Physics.SphereCastAll(transform.position, settings.radius, Vector3.up, settings.boidGroup);

        Alignment();
        Cohesion();
        Seperation();

        if(neighbours.Length != 0)
        {
            currentVelocity += desiredVelocity - currentVelocity;
            desiredVelocity = Vector3.zero;
        }
        if(sm.target != null)
            currentVelocity -= transform.position - sm.target.transform.position;

        agent.Move(currentVelocity * Time.deltaTime);
    }

    private void Alignment()
    {
        if (neighbours.Length == 0) return;
        Vector3 allignment = Vector3.zero;

        for (int i = 0; i < neighbours.Length; i++)
        {
            allignment += neighbours[i].transform.position;
        }

        allignment /= neighbours.Length;

        desiredVelocity += allignment.normalized * speed * settings.alignment;
    }
    private void Cohesion()
    {
        if (neighbours.Length == 0) return;
        Vector3 center = Vector3.zero;

        for (int i = 0; i < neighbours.Length; i++)
        {
            center += neighbours[i].transform.position;
        }

        center /= neighbours.Length;

        desiredVelocity += (center - transform.position).normalized * speed * settings.cohesion;
    }
    private void Seperation()
    {
        if (neighbours.Length == 0) return;
        Vector3 direction = Vector3.zero;
        Vector3 distance;

        for (int i = 0; i < neighbours.Length; i++)
        {
            distance = transform.position - neighbours[i].transform.position;
            direction += distance / distance.sqrMagnitude;
        }

        direction /= neighbours.Length;

        desiredVelocity += direction.normalized * speed * settings.seperation;
    }
}
