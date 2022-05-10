using System.Collections;
using UnityEngine;

public struct Box
{
    public Vector3 Center;
    public Vector3[] Bounds;
    public Vector3 Size;

    public Box(float _width, float _height, float _depth, Vector3 _location)
    {
        this.Center = _location;
        this.Size = new Vector3(_width, _depth, _height);
        this.Bounds = new Vector3[]
        {
            new Vector3(_location.x - _width/2, _location.y - _height/2, _location.z - _depth/2), //1
            new Vector3(_location.x + _width/2, _location.y - _height/2, _location.z - _depth/2), //2
            new Vector3(_location.x + _width/2, _location.y - _height/2, _location.z + _depth/2), //3
            new Vector3(_location.x - _width/2, _location.y - _height/2, _location.z + _depth/2), //4
            new Vector3(_location.x - _width/2, _location.y + _height/2, _location.z - _depth/2), //5
            new Vector3(_location.x + _width/2, _location.y + _height/2, _location.z - _depth/2), //6
            new Vector3(_location.x + _width/2, _location.y + _height/2, _location.z + _depth/2), //7
            new Vector3(_location.x - _width/2, _location.y + _height/2, _location.z + _depth/2) //8
        };
    }

    /// <summary>
    /// Checks if position is within bounds of the box
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public bool Contains(Vector3 _position)
    {
        if (_position.x > Bounds[0].x && _position.x < Bounds[1].x)
            if (_position.y > Bounds[0].y && _position.y < Bounds[4].y)
                if (_position.z > Bounds[0].z && _position.z < Bounds[2].z)
                    return true;

        return false;
    }

    /// <summary>
    /// Returns the index of the quadrant the position is in
    /// </summary>
    /// <param name="_position"></param>
    /// <returns>index of quadrant, -1 if position is not in box</returns>
    public int GetQuadrant(Vector3 _position)
    {
        bool isInTopQuadrant = _position.y > Center.y && _position.y < (Center.y + Size.y / 2);
        bool isInBottomQuadrant = _position.y > (Center.y - Size.y / 2) && _position.y < Center.y;

        bool isInLeftQuadrant = _position.x > (Center.x - Size.x / 2f) && _position.x < Center.x;
        bool isInRightQuadrant = _position.x > Center.x && _position.x < (Center.x + Size.x / 2f);

        bool isInFrontQuadrant = _position.z > (Center.z - Size.z / 2f) && _position.z < Center.z;
        bool isInBackQuadrant = _position.z > Center.z && _position.z < (Center.z + Size.z / 2f);

        //-- Get Location of given position within this node
        if (isInBottomQuadrant)
        {
            if (isInLeftQuadrant)
            {
                if (isInFrontQuadrant)
                {
                    return 0;
                }
                else if (isInBackQuadrant)
                {
                    return 3;
                }
            }
            else if (isInRightQuadrant)
            {
                if (isInFrontQuadrant)
                {
                    return 1;
                }
                else if (isInBackQuadrant)
                {
                    return 2;
                }
            }
        }
        else if (isInTopQuadrant)
        {
            if (isInLeftQuadrant)
            {
                if (isInFrontQuadrant)
                {
                    return 4;
                }
                else if (isInBackQuadrant)
                {
                    return 7;
                }
            }
            else if (isInRightQuadrant)
            {
                if (isInFrontQuadrant)
                {
                    return 5;
                }
                else if (isInBackQuadrant)
                {
                    return 6;
                }
            }
        }

        return -1; //<- If not in any Quadrant
    }

    public void Show(Color _c)
    {
        Gizmos.color = _c;
        Gizmos.DrawCube(Center, Size);
    }
}

public struct FibonacciSphere
{
    public Vector3[] points;

    public FibonacciSphere(int _nPoints, float _size)
    {
        this.points = new Vector3[_nPoints];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2f;
        float increment = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < _nPoints; i++)
        {
            float t = (float)i / _nPoints;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = increment * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            points[i] = new Vector3(x, y, z) * _size;
        }
    }
    public void Show(Color _c)
    {
        Gizmos.color = _c;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], .25f);
        }
    }
}