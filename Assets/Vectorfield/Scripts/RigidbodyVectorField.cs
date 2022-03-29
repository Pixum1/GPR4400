using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyVectorField : MonoBehaviour
{
    [SerializeField]
    private VectorField vectorField;

    private Rigidbody rb;

    [SerializeField]
    private float forceMultiplier;

    private void Awake()
    {
        if(vectorField == null)
            vectorField = FindObjectOfType<VectorField>();

         rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (vectorField != null)
        {
            Vector3 vectorForce = vectorField.GetForceDirection(transform.position);
            rb.AddForce(vectorForce * forceMultiplier);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, rb.velocity.normalized);
    }
}