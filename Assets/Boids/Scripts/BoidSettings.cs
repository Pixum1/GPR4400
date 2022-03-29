using UnityEngine;

[CreateAssetMenu(menuName = "Boids/BoidSettings", fileName = "BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [SerializeField] private float alignment;
    [SerializeField] private float cohesion;
    [SerializeField] private float seperation;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask boidLayer;
    [SerializeField] private float speed;
    [SerializeField] private int avoidanceRadius;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float avoidanceSensitivity;

    public float Alignment => alignment;
    public float Cohesion => cohesion;
    public float Seperation => seperation;
    public float Radius => radius;
    public LayerMask BoidLayer => boidLayer;
    public float Speed => speed;
    public int AvoidanceRadius => avoidanceRadius;
    public LayerMask ObstacleLayer => obstacleLayer;
    public float AvoidanceSensitivity => avoidanceSensitivity;
}