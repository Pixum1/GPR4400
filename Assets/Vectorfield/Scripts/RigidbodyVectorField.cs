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

    private void Update()
    {
        Debug.DrawRay(transform.position, rb.velocity);
    }

    private void FixedUpdate()
    {
        if (vectorField != null)
        {
            Vector3 vectorForce = vectorField.GetForceDirection(transform.position);
            rb.velocity += vectorForce * forceMultiplier;
        }
    }
}