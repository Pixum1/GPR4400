using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private BezierPoints[] mPoints;

    [SerializeField, Range(0, 100)]
    private int mVertsPerCurve;

    [SerializeField]
    private bool mAnimFlag;
    [SerializeField]
    private float mAnimSpeed;
    private float currAnimStep;

    [SerializeField]
    private Color mGizmosColorLayer0;
    [SerializeField]
    private Color mGizmosColorLayer1;
    [SerializeField]
    private Color mGizmosColorLayer2;
    [SerializeField]
    private float mGizmosSphereSize;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (mAnimFlag)
        {
            currAnimStep += Time.deltaTime * mAnimSpeed;
            if (currAnimStep > 1)
                currAnimStep %= 1;
        }
        else
        {
            currAnimStep = 1;
        }

        float stepSize = 1f / mVertsPerCurve;

        List<Vector3> curvePoints = new List<Vector3>();
        for (int i = 0; i < mPoints.Length - 1; i++)
        {
            for (float j = 0; j <= currAnimStep; j += stepSize)
            {
                //From 4 to 3 points
                Vector3 lerpA = Vector3.Lerp(mPoints[i].transform.position, mPoints[i].HandlePos, j);
                Vector3 lerpB = Vector3.Lerp(mPoints[i].HandlePos, mPoints[i + 1].transform.position, j);
                Vector3 lerpC = Vector3.Lerp(mPoints[i + 1].HandlePos, mPoints[i + 1].transform.position, j);

                //From 3 to 2 points
                Vector3 lerpPrimeA = Vector3.Lerp(lerpA, lerpB, j);
                Vector3 lerpPrimeB = Vector3.Lerp(lerpB, lerpC, j);

                //Get curve point
                Vector3 curvePoint = Vector3.Lerp(lerpPrimeA, lerpPrimeB, j);

                curvePoints.Add(curvePoint);
            }
        }

        lineRenderer.positionCount = curvePoints.Count;
        lineRenderer.SetPositions(curvePoints.ToArray());
    }

    private void OnDrawGizmosSelected()
    {
        if(mAnimFlag)
        {
            for (int i = 0; i < mPoints.Length - 1; i++)
            {
                //From 4 to 3 points
                Vector3 lerpA = Vector3.Lerp(mPoints[i].transform.position, mPoints[i].HandlePos, currAnimStep);
                Vector3 lerpB = Vector3.Lerp(mPoints[i].HandlePos, mPoints[i + 1].transform.position, currAnimStep);
                Vector3 lerpC = Vector3.Lerp(mPoints[i + 1].HandlePos, mPoints[i + 1].transform.position, currAnimStep);

                Gizmos.color = mGizmosColorLayer0;
                Gizmos.DrawSphere(lerpA, mGizmosSphereSize);
                Gizmos.DrawSphere(lerpB, mGizmosSphereSize);
                Gizmos.DrawSphere(lerpC, mGizmosSphereSize);
                Gizmos.DrawLine(lerpA, lerpB);
                Gizmos.DrawLine(lerpB, lerpC);

                //From 3 to 2 points
                Vector3 lerpPrimeA = Vector3.Lerp(lerpA, lerpB, currAnimStep);
                Vector3 lerpPrimeB = Vector3.Lerp(lerpB, lerpC, currAnimStep);

                Gizmos.color = mGizmosColorLayer1;
                Gizmos.DrawSphere(lerpPrimeA, mGizmosSphereSize);
                Gizmos.DrawSphere(lerpPrimeB, mGizmosSphereSize);
                Gizmos.DrawLine(lerpPrimeA, lerpPrimeB);

                //Get curve point
                Vector3 curvePoint = Vector3.Lerp(lerpPrimeA, lerpPrimeB, currAnimStep);
                Gizmos.DrawSphere(curvePoint, mGizmosSphereSize);

                Gizmos.color = mGizmosColorLayer2;
            }
        }
    }
}
