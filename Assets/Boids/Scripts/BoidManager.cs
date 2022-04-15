﻿using System.Collections;
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

            Boid b = Boids[i];

            #region Octtree / Neighbour check
            b.Neighbours.Clear();

            octtree.Insert(b);

            returnedObjects.Clear();
            octtree.Retrieve(returnedObjects, b);

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

            if (vField != null)
            {
                Vector3 vectorForce = vField.GetForceDirection(transform.position);
                b.rb.velocity += vectorForce.normalized * b.m_Settings.CurrentIntensity;
            }

            float targetHeight = boxSize/2 - b.m_Settings.TargetHeight;
            if (b.transform.localPosition.y != targetHeight)
            {
                if (b.transform.localPosition.y > targetHeight)
                    b.rb.velocity -= Vector3.up * b.m_Settings.TargetHeightIntensity;
                else
                    b.rb.velocity += Vector3.up * b.m_Settings.TargetHeightIntensity;
            }

            b.rb.velocity += ApplyAlignment(b.m_Settings.Alignment, b); //<- apply alignment
            b.rb.velocity += ApplyCohesion(b.m_Settings.Cohesion, b); //<- apply cohesion
            b.rb.velocity += ApplySeperation(b.m_Settings.Seperation, b); //<- apply seperation

            b.rb.velocity = Vector3.ClampMagnitude(b.rb.velocity, b.m_Settings.Speed * Time.deltaTime); //<- limit rigidbody velocity
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

        return seperation * _intensity;
    }

    #endregion

    #region ObstacleAvoidance

    private bool IsHeadingForObstacle(Boid _boid)
    {
        if (Physics.SphereCast(_boid.transform.position, .25f, transform.forward, out RaycastHit hit, _boid.m_Settings.AvoidanceRadius, _boid.m_Settings.ObstacleLayer))
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
        if (octtree != null)
            octtree.Show();

        Gizmos.DrawWireCube(transform.position, Vector3.one * boxSize);
    }
}