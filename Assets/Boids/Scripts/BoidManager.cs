using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public List<Boid> Boids = new List<Boid>();


    int rayDenseTemp;

    private Octtree octtree;
    private List<Boid> returnedObjects;

    private void Start()
    {
        octtree = new Octtree(0, new Box(500, 500, 500, transform.position));   
    }

    private void Update()
    {
        for (int i = 0; i < Boids.Count; i++)
        {
            
            Boids[i].transform.localScale = Vector3.one * Boids[i].m_Settings.Size;
            Boids[i].col.radius = Boids[i].m_Settings.BoidRadius;

            Boids[i].transform.LookAt(Boids[i].transform.position + Boids[i].rb.velocity); //<- face rigidbody move direction

            for (int j = 0; j < Boids[i].Neighbours.Count; j++)
            {
                Debug.DrawLine(Boids[i].transform.position, Boids[i].Neighbours[j].transform.position, Color.blue);
            }

            if (rayDenseTemp != Boids[i].m_Settings.RayDensity)
                Boids[i].rayDirections = Boids[i].RaySphere(Boids[i].m_Settings.RayDensity);

            rayDenseTemp = Boids[i].m_Settings.RayDensity;


            #region Boid Behaviour and Obstacle Avoidance
            if (IsHeadingForObstacle(Boids[i]))
            {
                Boids[i].rb.velocity += ApplyObstacleAvoidance(Boids[i].m_Settings.AvoidanceIntensity * (Boids[i].m_Settings.Speed * Time.deltaTime), Boids[i].m_Settings.AvoidanceRadius, Boids[i]); //<- apply obstacle avoidance
            }

            Boids[i].rb.velocity += ApplyAlignment(Boids[i].m_Settings.Alignment, Boids[i]); //<- apply alignment
            Boids[i].rb.velocity += ApplyCohesion(Boids[i].m_Settings.Cohesion, Boids[i]); //<- apply cohesion
            Boids[i].rb.velocity += ApplySeperation(Boids[i].m_Settings.Seperation, Boids[i]); //<- apply seperation

            Boids[i].rb.velocity = Vector3.ClampMagnitude(Boids[i].rb.velocity, Boids[i].m_Settings.Speed * Time.deltaTime); //<- limit rigidbody velocity
            #endregion
        }
    }


    #region Boid behaviour

    private Vector3 ApplyAlignment(float _intensity, Boid _boid)
    {
        Vector3 alignment = Vector3.zero;

        if (_boid.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _boid.Neighbours.Count; i++)
        {
            alignment += _boid.Neighbours[i].rb.velocity;
        }

        alignment /= _boid.Neighbours.Count;

        return alignment * _intensity;
    }

    private Vector3 ApplyCohesion(float _intensity, Boid _boid)
    {
        Vector3 cohesion = Vector3.zero;

        if (_boid.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _boid.Neighbours.Count; i++)
        {
            cohesion += _boid.Neighbours[i].transform.position - _boid.transform.position;
        }

        cohesion /= _boid.Neighbours.Count;

        return cohesion * _intensity;
    }

    private Vector3 ApplySeperation(float _intensity, Boid _boid)
    {
        Vector3 seperation = Vector3.zero;

        if (_boid.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _boid.Neighbours.Count; i++)
        {
            seperation -= _boid.Neighbours[i].transform.position - _boid.transform.position;
        }

        seperation /= _boid.Neighbours.Count;

        return seperation.normalized * _intensity;
    }

    #endregion

    #region ObstacleAvoidance

    private bool IsHeadingForObstacle(Boid _boid)
    {
        if (Physics.OverlapSphere(_boid.transform.position, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer).Length > 0)
            return true;
        else
            return false;
    }

    private Vector3 ApplyObstacleAvoidance(float _intensity, float _rayLength, Boid _boid)
    {
        Vector3 bestDir = Vector3.zero;
        float bestDirCount = 0; //<- count of all directions with no obstacle

        for (int i = 0; i < _boid.rayDirections.Length; i++)
        {
            Vector3 dir = _boid.rayDirections[i] * _rayLength;
            Ray r = new Ray(_boid.transform.position, dir);

            //- All directions with no obstacle get added up
            if (!Physics.Raycast(r, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
            {
                bestDir += dir;
                bestDirCount++;
            }
            //else
            //Debug.DrawRay(_boid.transform.position, dir, Color.red);
        }

        if (bestDirCount > 0)
            bestDir /= bestDirCount; //<- get average of all directions

        return bestDir * _intensity;
    }
    #endregion
}