using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField]
    private ChunkManager chunkManager;

    [HideInInspector]
    public List<Boid> Boids = new List<Boid>();

    private List<Boid> returnedObjects = new List<Boid>();

    private void Update()
    {
        chunkManager.octree.Clear();
        for (int b = 0; b < Boids.Count; b++)
        {
            chunkManager.octree.Insert(Boids[b]);

            if (!chunkManager.octree.box.Contains(Boids[b].transform.position))
                Boids[b].transform.gameObject.SetActive(false);

            else
                Boids[b].transform.gameObject.SetActive(true);
        }

        for (int i = 0; i < Boids.Count; i++)
        {
            Boid b = Boids[i];

            if (!b.gameObject.activeInHierarchy)
                continue;

            #region Octtree / Neighbour check
            b.Neighbours.Clear();

            returnedObjects.Clear();
            chunkManager.octree.Retrieve(returnedObjects, b);

            for (int f = 0; f < returnedObjects.Count; f++)
            {
                Boid boidToCheck = returnedObjects[f];

                if ((boidToCheck.transform.position - b.transform.position).sqrMagnitude <= b.m_Settings.BoidRadius * b.m_Settings.BoidRadius)
                {

                    if (boidToCheck.m_Settings.BoidLayer == b.m_Settings.BoidLayer)
                        b.Neighbours.Add(returnedObjects[f]);
                }
            }
            #endregion

            #region Boid Settings
            b.transform.localScale = Vector3.one * b.m_Settings.Size;

            b.transform.LookAt(b.transform.position + b.rb.velocity); //<- face rigidbody move direction
            #endregion

            #region Boid Behaviour and Obstacle Avoidance
            //b.rb.velocity += Random.insideUnitSphere* 8;
            if (IsHeadingForObstacle(b))
            {
                //Boids[i].rb.velocity += ApplyObstacleAvoidance(Boids[i].m_Settings.AvoidanceIntensity * (Boids[i].m_Settings.Speed * Time.deltaTime), Boids[i].m_Settings.AvoidanceRadius, Boids[i]); //<- apply obstacle avoidance
                b.rb.velocity += ApplyObstacleAvoidance(b.m_Settings.AvoidanceIntensity * (b.m_Settings.Speed * Time.deltaTime), b); //<- apply obstacle avoidance
            }

            //--Target Height
            //float targetHeight = chunkManager.octree.box.Size.y / 2 - b.m_Settings.TargetHeight;
            //if (b.transform.localPosition.y != targetHeight)
            //{
            //    if (b.transform.localPosition.y > targetHeight)
            //        b.rb.velocity -= Vector3.up * b.m_Settings.TargetHeightIntensity;
            //    else
            //        b.rb.velocity += Vector3.up * b.m_Settings.TargetHeightIntensity;
            //}

            b.rb.velocity += ApplyAlignment(b.m_Settings.Alignment, b); //<- apply alignment
            b.rb.velocity += ApplyCohesion(b.m_Settings.Cohesion, b); //<- apply cohesion
            b.rb.velocity += ApplySeperation(b.m_Settings.Seperation, b); //<- apply seperation

            b.rb.velocity = Vector3.ClampMagnitude(b.rb.velocity, b.m_Settings.Speed * Time.deltaTime); //<- limit rigidbody velocity
            #endregion
        }

    }


    #region Boid behaviour
    private Vector3 ApplyAlignment(float _intensity, Boid _b)
    {
        Vector3 alignment = Vector3.zero;

        if (_b.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _b.Neighbours.Count; i++)
        {
            alignment += _b.Neighbours[i].rb.velocity;
        }

        alignment /= _b.Neighbours.Count;

        return alignment * _intensity;
    }

    private Vector3 ApplyCohesion(float _intensity, Boid _b)
    {
        Vector3 cohesion = Vector3.zero;

        if (_b.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _b.Neighbours.Count; i++)
        {
            cohesion += _b.Neighbours[i].transform.position - _b.transform.position;
        }

        cohesion /= _b.Neighbours.Count;

        return cohesion * _intensity;
    }

    private Vector3 ApplySeperation(float _intensity, Boid _b)
    {
        Vector3 seperation = Vector3.zero;

        if (_b.Neighbours.Count == 0)
            return Vector3.zero;

        for (int i = 0; i < _b.Neighbours.Count; i++)
        {
            seperation -= _b.Neighbours[i].transform.position - _b.transform.position;
        }

        seperation /= _b.Neighbours.Count;

        return seperation * _intensity;
    }

    #endregion

    #region ObstacleAvoidance

    private bool IsHeadingForObstacle(Boid _boid)
    {
        if (Physics.SphereCast(_boid.transform.position, .25f, _boid.rb.velocity.normalized, out RaycastHit hit, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
            return true;
        else
            return false;
    }

    private Vector3 ApplyObstacleAvoidance(float _intensity, Boid _boid)
    {
        Physics.Raycast(_boid.transform.position, _boid.rayDirections[0], out RaycastHit hitInfo, _boid.m_Settings.ObstacleLayer);
        if (hitInfo.distance > 0)
            _intensity /= hitInfo.distance;

        for (int i = 0; i < _boid.rayDirections.Length; i++)
        {
            Vector3 dir = _boid.transform.TransformDirection(_boid.rayDirections[i]);
            Ray r = new Ray(_boid.transform.position, dir);
            if (!Physics.SphereCast(r, .2f, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
            {
                return dir * _intensity;
            }
        }

        return _boid.transform.forward;
    }
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
        if (chunkManager != null)
            if (chunkManager.octree != null)
                chunkManager.octree.Show();
    }
}