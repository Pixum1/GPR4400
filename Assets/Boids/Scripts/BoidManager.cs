using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField]
    private VectorField vField;
    public List<Boid> Boids = new List<Boid>();

    private Octtree octtree;
    private List<Boid> returnedObjects = new List<Boid>();

    [SerializeField]
    private float boxSize;

    private void Start()
    {
        vField = FindObjectOfType<VectorField>();
        octtree = new Octtree(0, new Box(boxSize, boxSize, boxSize, transform.position));
    }

    private void Update()
    {
        octtree.Clear();

        for (int i = 0; i < Boids.Count; i++)
        {
            #region Octtree
            Boids[i].Neighbours.Clear();
            returnedObjects.Clear();

            octtree.Insert(Boids[i]);

            octtree.Retrieve(returnedObjects, Boids[i]);

            for (int f = 0; f < returnedObjects.Count; f++)
            {
                Boid boidToCheck = returnedObjects[f];
                if ((boidToCheck.transform.position - Boids[i].transform.position).sqrMagnitude <= Boids[i].m_Settings.BoidRadius * Boids[i].m_Settings.BoidRadius)
                    if (boidToCheck.m_Settings.BoidLayer == Boids[i].m_Settings.BoidLayer)
                        Boids[i].Neighbours.Add(returnedObjects[f]);
            }
            #endregion

            #region Boid Settings
            Boids[i].transform.localScale = Vector3.one * Boids[i].m_Settings.Size;

            Boids[i].transform.LookAt(Boids[i].transform.position + Boids[i].rb.velocity); //<- face rigidbody move direction
            #endregion

            #region Boid Behaviour and Obstacle Avoidance
            if (IsHeadingForObstacle(Boids[i]))
            {
                //Boids[i].rb.velocity += ApplyObstacleAvoidance(Boids[i].m_Settings.AvoidanceIntensity * (Boids[i].m_Settings.Speed * Time.deltaTime), Boids[i].m_Settings.AvoidanceRadius, Boids[i]); //<- apply obstacle avoidance
                Boids[i].rb.velocity += ApplyObstacleAvoidance(Boids[i].m_Settings.AvoidanceIntensity * (Boids[i].m_Settings.Speed * Time.deltaTime), Boids[i]); //<- apply obstacle avoidance
            }

            if (vField != null)
            {
                Vector3 vectorForce = vField.GetForceDirection(transform.position);
                Boids[i].rb.velocity += vectorForce.normalized * Boids[i].m_Settings.CurrentIntensity;
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
            alignment += _boid.Neighbours[i].rb.velocity.normalized;
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

    private Vector3 ApplyObstacleAvoidance(float _intensity, Boid _boid)
    {
        for (int i = 0; i < _boid.rayDirections.Length; i++)
        {
            Vector3 dir = _boid.transform.TransformDirection(_boid.rayDirections[i]);
            Ray r = new Ray(_boid.transform.position, dir);
            if (!Physics.SphereCast(r, .2f, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
            {
                //Debug.DrawRay(_boid.transform.position, dir, Color.green);
                return dir * _intensity;
            }
            //else
            //    Debug.DrawRay(_boid.transform.position, dir, Color.red);
        }

        return _boid.transform.forward;
    }

    //private Vector3 ApplyObstacleAvoidance(float _intensity, float _rayLength, Boid _boid)
    //{
    //    Vector3 bestDir = Vector3.zero;
    //    float bestDirCount = 0; //<- count of all directions with no obstacle

    //    for (int i = 0; i < _boid.rayDirections.Length; i++)
    //    {
    //        Vector3 dir = _boid.rayDirections[i] * _rayLength;
    //        Ray r = new Ray(_boid.transform.position, dir);

    //        //- All directions with no obstacle get added up
    //        if (!Physics.Raycast(r, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
    //        {
    //            bestDir += dir;
    //            bestDirCount++;
    //        }
    //        //else
    //        //Debug.DrawRay(_boid.transform.position, dir, Color.red);
    //    }

    //    if (bestDirCount > 0)
    //        bestDir /= bestDirCount; //<- get average of all directions

    //    return bestDir * _intensity;
    //}
    #endregion

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < Boids.Count; i++)
        {
            for (int n = 0; n < Boids[i].Neighbours.Count; n++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Boids[i].transform.position, Boids[i].Neighbours[n].transform.position);
            }
        }
        if (octtree != null)
            octtree.Show();
    }
}