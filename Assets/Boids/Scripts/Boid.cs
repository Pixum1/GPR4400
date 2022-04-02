using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boid : MonoBehaviour
{
    [SerializeField]
    private BoidSettings m_Settings;

    private List<Boid> neighbours;
    private SphereCollider col;
    private Rigidbody rb;

    private Vector3[] rayDirections;

    private void Start()
    {
        neighbours = new List<Boid>();

        if (col == null)
            col = GetComponent<SphereCollider>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Random.insideUnitSphere * Time.deltaTime * m_Settings.Speed;
    }

    private void Update()
    {
        transform.localScale = Vector3.one * m_Settings.Size;
        col.radius = m_Settings.Radius;

        transform.LookAt(transform.position + rb.velocity); //<- face rigidbody move direction

        for (int i = 0; i < neighbours.Count; i++)
        {
            Debug.DrawLine(transform.position, neighbours[i].transform.position, Color.blue);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity += ApplyObstacleAvoidance(m_Settings.AvoidanceIntensity * (m_Settings.Speed * Time.deltaTime)); //<- apply obstacle avoidance
        rb.velocity += ApplyAlignment(m_Settings.Alignment); //<- apply alignment
        rb.velocity += ApplyCohesion(m_Settings.Cohesion); //<- apply cohesion
        rb.velocity += ApplySeperation(m_Settings.Seperation); //<- apply seperation

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, m_Settings.Speed * Time.fixedDeltaTime); //<- limit rigidbody velocity
    }

    #region Boid behaviour

    private Vector3 ApplyAlignment(float _intensity)
    {
        Vector3 alignment = Vector3.zero;

        if (neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            alignment += neighbours[i].rb.velocity;
        }

        alignment /= neighbours.Count;

        return alignment * _intensity;
    }

    private Vector3 ApplyCohesion(float _intensity)
    {
        Vector3 cohesion = Vector3.zero;

        if (neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < neighbours.Count; i++)
        {
            cohesion += neighbours[i].transform.position - transform.position;
        }

        cohesion /= neighbours.Count;

        return cohesion * _intensity;
    }

    private Vector3 ApplySeperation(float _intensity)
    {
        Vector3 seperation = Vector3.zero;

        if (neighbours.Count == 0)
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
    private Vector3[] RaySphere(int _rayDensity, float _length)
    {
        int rayAmount = (int)Mathf.Pow(_rayDensity * 2, 3);
        rayDirections = new Vector3[rayAmount]; //<- Create array with the amount of rays generated

        int count = 0;

        for (int y = -_rayDensity; y < _rayDensity; y++)
        {
            for (int z = -_rayDensity; z < _rayDensity; z++)
            {
                for (int x = -_rayDensity; x < _rayDensity; x++)
                {
                    Vector3 offset = new Vector3(x + .5f, y + .5f, z + .5f); //<- offset of each ray in local space
                    Vector3 localPos = transform.position + offset; //<- position of individual ray in world space
                    Vector3 dir = (transform.position - localPos).normalized * _length; //<- direction of individual ray

                    rayDirections[count] = dir;
                    count++;
                }
            }
        }

        return rayDirections;
    }

    private Vector3 ApplyObstacleAvoidance(float _intensity)
    {
        Vector3[] directions = RaySphere(m_Settings.RayDensity, m_Settings.AvoidanceRadius); //<- array with all directions
        Vector3 bestDir = Vector3.zero;
        float bestDirCount = 0; //<- count of all directions with no obstacle

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 dir = directions[i];
            Ray r = new Ray(transform.position, dir);

            //- All directions with no obstacle get added up
            if (!Physics.Raycast(r, m_Settings.AvoidanceRadius, m_Settings.ObstacleLayer))
            {
                bestDir += dir;
                bestDirCount++;
            }
            else
                Debug.DrawRay(transform.position, dir, Color.red);
        }

        bestDir /= bestDirCount; //<- get average of all directions

        return bestDir * _intensity;
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
