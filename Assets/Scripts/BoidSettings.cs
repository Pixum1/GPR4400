using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Boid Settings")]
public class BoidSettings : ScriptableObject
{
    public float radius;
    public LayerMask boidGroup;
    public float cohesion;
    public float alignment;
    public float seperation;
}
