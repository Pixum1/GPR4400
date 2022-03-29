using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoints : MonoBehaviour
{
    [SerializeField]
    private Transform mHandle;
    public Vector3 HandlePos => mHandle.position;

    [SerializeField]
    private Color GizmosPointColor;
    [SerializeField]
    private Color GizmosHandleColor;
    [SerializeField]
    private Color GizmosConnectionColor;

    [SerializeField]
    private float GizmosPointSize;
    [SerializeField]
    private float GizmosHandlesize;

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmosPointColor;
        Gizmos.DrawSphere(this.transform.position, GizmosPointSize);

        if(mHandle != null)
        {
            Gizmos.color = GizmosHandleColor;
            Gizmos.DrawWireSphere(mHandle.position, GizmosHandlesize);

            Gizmos.color = GizmosConnectionColor;
            Gizmos.DrawLine(this.transform.position, mHandle.position);
        }
    }
}
