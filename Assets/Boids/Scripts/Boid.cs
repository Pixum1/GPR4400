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
        int amount = 300;

        Vector3[] directions = new Vector3[amount];
        Vector3 bestDir = transform.forward;
        float furthestUnobstructedDst = 0;
        RaycastHit hit;

        for (int i = 0; i < directions.Length; i++)
        {
            float t = (float) i / amount;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = 2 * Mathf.PI * ((1 + Mathf.Sqrt(5)) / 2) * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3(x, y, z);

            Vector3 dir = transform.TransformDirection(directions[i]);
            if(Physics.SphereCast(transform.position, settings.AvoidanceRadius, dir, out hit, settings.AvoidanceRadius, settings.ObstacleLayer))
            {
                if(hit.distance > furthestUnobstructedDst)
                {
                    currentVelocity += bestDir;
                    furthestUnobstructedDst = hit.distance;
                }
                else
                {
                    currentVelocity += dir;
                }
            }
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
