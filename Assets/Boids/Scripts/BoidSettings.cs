using UnityEngine;

[CreateAssetMenu(menuName = "Boids/BoidSettings", fileName = "BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [Header("Boid Settings")]
    [SerializeField] private float mMoveSpeed;

    [Header("Behaviour Intensity")]
    [SerializeField] private float mAlignmentIntensity;
    [SerializeField] private float mCohesionIntensity;
    [SerializeField] private float mSeperationIntensity;

    [Header("Obstacle Avoidance")]
    [SerializeField] private float mAvoidanceIntensity;
    [SerializeField, Range(1, 5)] private int mRayDensity;

    [Header("Behaviour Radius")]
    [SerializeField] private float mBoidRadius;
    [SerializeField] private float mAvoidanceRadius;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask mObstacleLayer;
    [SerializeField] private LayerMask mBoidLayer;


    public float Alignment => mAlignmentIntensity;
    public float Cohesion => mCohesionIntensity;
    public float Seperation => mSeperationIntensity;
    public float Radius => mBoidRadius;
    public LayerMask BoidLayer => mBoidLayer;
    public float Speed => mMoveSpeed;
    public float AvoidanceRadius => mAvoidanceRadius;
    public LayerMask ObstacleLayer => mObstacleLayer;
    public float AvoidanceIntensity => mAvoidanceIntensity;
    public int RayDensity => mRayDensity;
}