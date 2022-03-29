using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorField : MonoBehaviour
{
    [SerializeField]
    private float mSize;
    [SerializeField]
    private int mResolution;

    [SerializeField]
    private int mIterationCount;
    [SerializeField, Range(0, 1)]
    private float mCohesionLerpAmount;
    [SerializeField]
    private float mStepTime;
    [SerializeField, Range(0, 1)]
    private float biasMultiplier;

    [SerializeField]
    private float objectAmount;
    [SerializeField]
    private GameObject mObject;

    private Vector3[,,] vectorField;

    [SerializeField]
    private Transform fieldModifier;
    [SerializeField]
    private Transform fieldModifier2;

    private void Awake()
    {
        InitVectorField();
        StartCoroutine(C_IterateField());
    }

    private void Start()
    {
        for (int i = 0; i < objectAmount; i++)
        {
            GameObject go = GameObject.Instantiate(mObject, null);
            go.transform.position = Random.insideUnitSphere * mSize / 3;
        }
    }

    private void InitVectorField()
    {
        this.transform.localScale = Vector3.one * mSize;

        vectorField = new Vector3[mResolution, mResolution, mResolution];
        for (int y = 0; y < mResolution; y++)
        {
            for (int z = 0; z < mResolution; z++)
            {
                for (int x = 0; x < mResolution; x++)
                {
                    vectorField[x, y, z] = Random.insideUnitSphere;
                    Vector3 rayPosition = GetRayPosition(x, y, z);
                    //vectorField[x, y, z] += Vector3.up * 0.2f; // bias for up facing vectors

                    //center force
                    //vectorField[x, y, z] += (transform.position - rayPosition) * biasMultiplier; //bias for center facing vectors

                    //Whirlpool field
                    //vectorField[x, y, z] = (rayPosition.z / (rayPosition.x * rayPosition.x + rayPosition.z * rayPosition.z)) * fieldModifier.position.normalized * biasMultiplier -
                    //    (rayPosition.x / (rayPosition.x * rayPosition.x + rayPosition.z * rayPosition.z)) * fieldModifier2.position.normalized;
                    
                    //Vortex field
                    Vector3 newDir = new Vector3(rayPosition.z, 0, -rayPosition.x);
                    vectorField[x, y, z] += (newDir * mResolution) * biasMultiplier;

                    vectorField[x, y, z].Normalize();
                }
            }
        }
    }
    private Vector3 GetRayPosition(float _x, float _y, float _z)
    {
        Vector3 percentualOffset = new Vector3(_x, _y, _z) / (mResolution - 1);
        Vector3 localPos = ((Vector3.one * mSize) * -1) / 2f + percentualOffset * mSize;

        return transform.position + localPos;
    }
    private Vector3Int GetPositionInVectorField(Vector3 _position)
    {
        Vector3 startPos = this.transform.position + ((-(Vector3.one * mSize)) / 2f);
        Vector3 dir = _position - startPos;
        Vector3 idx = ((dir * (mResolution - 1)) / mSize);

        int x = Mathf.RoundToInt(idx.x);
        int y = Mathf.RoundToInt(idx.y);
        int z = Mathf.RoundToInt(idx.z);

        return new Vector3Int(x, y, z);
    }
    public Vector3 GetForceDirection(Vector3 _position)
    {
        Vector3Int newPos = GetPositionInVectorField(_position);
        return GetAverageNeighbourhood(mResolution, newPos.x, newPos.y, newPos.z, true);
    }

    private IEnumerator C_IterateField()
    {
        int currRes = mResolution;
        for (int i = 0; i < mIterationCount; i++)
        {
            Vector3[,,] newIterationVectorField = new Vector3[currRes, currRes, currRes];

            //gehe durch alle Vektoren
            for (int y = 0; y < mResolution; y++)
            {
                for (int z = 0; z < mResolution; z++)
                {
                    for (int x = 0; x < mResolution; x++)
                    {
                        //ermittle die durchschnittliche "Richtung" in der Umgebungs jedes Vektors
                        Vector3 averageNeighbourshood = GetAverageNeighbourhood(currRes, x, y, z, true);

                        //passe / lerpe den vorhandenen Vektor zum Durschnitt
                        newIterationVectorField[x, y, z] = Vector3.Lerp(vectorField[x, y, z], averageNeighbourshood, mCohesionLerpAmount);
                        newIterationVectorField[x, y, z].Normalize();
                    }
                }
            }

            vectorField = newIterationVectorField;
            yield return new WaitForSeconds(mStepTime);
        }
    }

    private Vector3 GetAverageNeighbourhood(int _currRes, int _x, int _y, int _z, bool _countSelf = false)
    {
        Vector3 average = Vector3.zero;
        int count = 0;

        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dz = -1; dz <= 1; dz++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int currX = _x + dx;
                    int currY = _y + dy;
                    int currZ = _z + dz;

                    //Check if in the Vector field array
                    if (currX >= 0 && currX < _currRes &&
                        currY >= 0 && currY < _currRes &&
                        currZ >= 0 && currZ < _currRes)
                    {
                        //Check if not self OR should count self?
                        if (dx + dy + dz != 0 || _countSelf)
                        {
                            average += vectorField[currX, currY, currZ];
                            count++;
                        }
                    }
                }
            }
        }

        if (count > 0)
            average /= count;

        return average;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position, Vector3.one * mSize);

        if (vectorField != null)
        {
            for (int y = 0; y < mResolution; y++)
            {
                for (int z = 0; z < mResolution; z++)
                {
                    for (int x = 0; x < mResolution; x++)
                    {
                        Vector3 idx = new Vector3(x, y, z);
                        Vector3 percentualOffset = idx / (mResolution - 1); // prozentualer Offset pro Dimension
                        Vector3 localPos = ((Vector3.one * mSize) * -1) / 2f + percentualOffset * mSize;

                        Vector3 dir = vectorField[x, y, z];

                        Vector3 colorVec = (dir + Vector3.one) / 2f;

                        Gizmos.color = new Color(colorVec.x, colorVec.y, colorVec.z);
                        //Gizmos.DrawSphere(this.transform.position + localPos, 0.1f);
                        Gizmos.DrawRay(this.transform.position + localPos, dir);
                    }
                }
            }
        }
    }
}
