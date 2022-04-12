using UnityEngine;

[CreateAssetMenu(menuName = "Boids/BoidSettings", fileName = "BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [Header("Boid Settings")]
    [SerializeField, Range(0, 1000)] private float m_MoveSpeed;
    [SerializeField, Range(0.1f, 3)] private float m_Size;

    [Header("Behaviour Intensity")]
    [SerializeField,Range(0,5)] private float m_AlignmentIntensity;
    [SerializeField, Range(0, 5)] private float m_CohesionIntensity;
    [SerializeField, Range(0, 5)] private float m_SeperationIntensity;
    [SerializeField, Range(0, 5)] private float m_targetHeightIntensity;

    [Header("Obstacle Avoidance")]
    [SerializeField, Range(0, 5)] private float m_AvoidanceIntensity;
    //[SerializeField, Range(1, 5)] private int m_RayDensity;
    [SerializeField] private int m_RayCount;

    [Header("Vectorfield")]
    [SerializeField, Range(0, 5)] private float m_currentIntensity;

    [Header("Behaviour Radius")]
    [SerializeField] private float m_BoidRadius;
    [SerializeField] private float m_AvoidanceRadius;
    [SerializeField] private float m_targetHeight;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask m_ObstacleLayer;
    [SerializeField] private LayerMask m_BoidLayer;


    public float Size => m_Size;
    public int RayCount => m_RayCount;
    public float Alignment => m_AlignmentIntensity;
    public float Cohesion => m_CohesionIntensity;
    public float Seperation => m_SeperationIntensity;
    public float BoidRadius => m_BoidRadius;
    public LayerMask BoidLayer => m_BoidLayer;
    public float Speed => m_MoveSpeed;
    public float AvoidanceRadius => m_AvoidanceRadius;
    public LayerMask ObstacleLayer => m_ObstacleLayer;
    public float AvoidanceIntensity => m_AvoidanceIntensity;
    public float CurrentIntensity => m_currentIntensity;
    public float TargetHeight => m_targetHeight;
    public float TargetHeightIntensity => m_targetHeightIntensity;
    //public int RayDensity => m_RayDensity;
}