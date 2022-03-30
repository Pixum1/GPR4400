using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boid : MonoBehaviour
{
    private List<Boid> neighbours;

    private Vector3 desiredVelocity, currentVelocity;

    [SerializeField] private BoidSettings settings;
    [SerializeField] private SphereCollider col;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        neighbours = new List<Boid>();

        if (col == null)
            col = GetComponent<SphereCollider>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Alignment();
        Cohesion();
        Separation();
        ObstacleAvoidance();

        if (neighbours.Count != 0)
        {
            currentVelocity += desiredVelocity - currentVelocity;
            desiredVelocity = Vector3.zero;
        }

        currentVelocity += transform.forward;

        currentVelocity = Vector3.ClampMagnitude(currentVelocity, settings.Speed);

        rb.velocity = currentVelocity * Time.deltaTime;

        Debug.DrawLine(transform.position, currentVelocity, Color.blue);

        transform.LookAt(rb.velocity);
    }

    private void ObstacleAvoidance()
    {
        if(Physics.SphereCast(transform.position, settings.AvoidanceRadius, Vector3.up, out RaycastHit hit, float.MaxValue, settings.ObstacleLayer))
        {
            Vector3 newDir = transform.position - hit.point;
            Vector3 vel = newDir * settings.Speed;
            rb.velocity = vel * Time.deltaTime;
        }
    }

    private void Alignment()
    {
        if (neighbours.Count == 0) return;
        Vector3 alignment = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            alignment += neighbours[i].currentVelocity;
        }

        alignment /= neighbours.Count;

        desiredVelocity += alignment.normalized * settings.Speed * settings.Alignment;
    }

    private void Cohesion()
    {
        if (neighbours.Count == 0) return;
        Vector3 center = Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            center += neighbours[i].transform.position;
        }

        center /= neighbours.Count;

        desiredVelocity += (center - transform.position).normalized * settings.Speed * settings.Cohesion;
    }

    private void Separation()
    {
        if (neighbours.Count == 0) return;
        Vector3 direction = Vector3.zero;
        Vector3 distance;

        for (int i = 0; i < neighbours.Count; i++)
        {
            distance = transform.position - neighbours[i].transform.position;
            direction += distance / distance.sqrMagnitude;
        }

        direction /= neighbours.Count;
        desiredVelocity += direction.normalized * settings.Speed * settings.Seperation;
    }

    private void OnTriggerEnter(Collider _other)
    {
        Boid b = _other.GetComponent<Boid>();
        if (b != null)
            neighbours.Add(b);
    }
    private void OnTriggerExit(Collider _other)
    {
        Boid b = _other.GetComponent<Boid>();
        if (b != null)
            neighbours.Remove(b);
    }
}
