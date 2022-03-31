using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boid : MonoBehaviour
{
    private List<Boid> neighbours;

    [SerializeField]
    private BoidSettings settings;
    [SerializeField]
    private SphereCollider col;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField, Range(1, 5)]
    private int mRayIterations;

    private void Start()
    {
        neighbours = new List<Boid>();

        if (col == null)
            col = GetComponent<SphereCollider>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Random.insideUnitSphere * Time.deltaTime * settings.Speed;
    }

    private void Update()
    {
        col.radius = settings.Radius;

        transform.LookAt(transform.position + rb.velocity); //<- face rigidbody move direction

        for (int i = 0; i < neighbours.Count; i++)
        {
            Debug.DrawLine(transform.position, neighbours[i].transform.position, Color.blue);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity += ObstacleAvoidance(mRayIterations, settings.AvoidanceRadius); //<- apply obstacle avoidance
        rb.velocity += Alignment(settings.Alignment); //<- apply alignment
        rb.velocity += Cohesion(settings.Cohesion); //<- apply cohesion
        rb.velocity += Seperation(settings.Seperation); //<- apply seperation

        //rb.velocity += transform.forward; //<- initial speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, settings.Speed * Time.fixedDeltaTime); //<- limit rigidbody velocity
    }

    #region Boid behaviour

    private Vector3 Alignment(float _intensity)
    {
        Vector3 alignment = Vector3.zero;

        if (neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            alignment += neighbours[i].rb.velocity;
        }

        alignment /= neighbours.Count;

        return alignment.normalized * _intensity;
    } 

    private Vector3 Cohesion(float _intensity)
    {
        Vector3 cohesion = Vector3.zero;

        if (neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            cohesion += neighbours[i].transform.position - transform.position;
        }

        cohesion /= neighbours.Count;

        return cohesion.normalized * _intensity;
    }

    private Vector3 Seperation(float _intensity)
    {
        Vector3 seperation = Vector3.zero;

        if(neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            seperation -= neighbours[i].transform.position - transform.position;
        }

        seperation /= neighbours.Count;

        return seperation.normalized * _intensity;
    }

    #endregion

    #region ObstacleAvoidance
    private Vector3 ObstacleAvoidance(int _rayIterations, float _length)
    {
        Vector3 desiredVelocity = Vector3.zero;
        List<RaycastHit> hits = new List<RaycastHit>(); //<- list of hit information of all rays that hit something

        for (int y = -_rayIterations; y < _rayIterations; y++)
        {
            for (int z = -_rayIterations; z < _rayIterations; z++)
            {
                for (int x = -_rayIterations; x < _rayIterations; x++)
                {
                    //-- Create a sphere of rays
                    Vector3 offset = new Vector3(x + .5f, y + .5f, z + .5f); //<- offset of each ray in local space
                    Vector3 localPos = transform.position + offset; //<- position of individual ray in world space
                    Vector3 dir = localPos - transform.position; //<- direction of individual ray

                    dir.Normalize();

                    desiredVelocity += CheckForCollision(dir * _length, hits);
                }
            }
        }

        desiredVelocity /= hits.Count; //<- get average of all ray directions that hit

        Debug.DrawRay(transform.position, desiredVelocity.normalized, Color.green);

        return desiredVelocity.normalized * settings.AvoidanceSensitivity;
    }

    private Vector3 CheckForCollision(Vector3 _dir, List<RaycastHit> _hitList)
    {
        Vector3 newDir = Vector3.zero;
        if(Physics.Raycast(transform.position, _dir, out RaycastHit hit, _dir.magnitude, settings.ObstacleLayer))
        {
            newDir += transform.position - hit.point; //<- get oposite direction
            _hitList.Add(hit); //<- add to list if hit

            Debug.DrawRay(transform.position, _dir, Color.red);
        }

        return newDir;
    }
    #endregion

    #region Trigger stuff
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
    #endregion
}
