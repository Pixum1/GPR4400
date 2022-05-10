using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boid : MonoBehaviour
{
    private BoidManager boidManager;

    [HideInInspector] public List<Boid> Neighbours;
    [HideInInspector] public SphereCollider col;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3[] rayDirections;
    [SerializeField] public BoidSettings m_Settings;

    private void Awake()
    {
        boidManager = FindObjectOfType<BoidManager>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        boidManager.Boids.Add(this);

    }

    private void OnDisable()
    {
        boidManager.Boids.Remove(this);
    }

    private void Start()
    {
        rb.velocity = Random.insideUnitSphere * Time.deltaTime * m_Settings.Speed;

        rayDirections = new FibonacciSphere(m_Settings.RayCount, m_Settings.AvoidanceRadius).points;
    }
}
