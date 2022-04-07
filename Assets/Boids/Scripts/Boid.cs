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
    private FibonacciSphere sphere;
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

        //rayDirections = RaySphere(m_Settings.RayDensity);
        rayDirections = new FibonacciSphere(m_Settings.RayCount, m_Settings.AvoidanceRadius).points;
    }

    //public Vector3[] RaySphere(int _rayDensity)
    //{
    //    int rayAmount = (int)Mathf.Pow(_rayDensity * 2, 3);
    //    rayDirections = new Vector3[rayAmount]; //<- Create array with the amount of rays generated

    //    int count = 0;

    //    for (int y = -_rayDensity; y < _rayDensity; y++)
    //    {
    //        for (int z = -_rayDensity; z < _rayDensity; z++)
    //        {
    //            for (int x = -_rayDensity; x < _rayDensity; x++)
    //            {
    //                Vector3 offset = new Vector3(x + .5f, y + .5f, z + .5f); //<- offset of each ray in local space
    //                Vector3 localPos = transform.position + offset; //<- position of individual ray in world space
    //                Vector3 dir = (transform.position - localPos).normalized; //<- direction of individual ray

    //                rayDirections[count] = dir;
    //                count++;
    //            }
    //        }
    //    }

    //    return rayDirections;
    //}
}
