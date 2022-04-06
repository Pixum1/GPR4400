using System.Collections;
using UnityEngine;

public class Box
{
    public Vector3 Center;
    public Vector3[] Bounds;
    public Vector3 Size;

    public Box(float _width, float _height, float _depth, Vector3 _centerPos)
    {
        Center = _centerPos;
        Size = new Vector3(_width, _depth, _height);
        Bounds = new Vector3[]
        {
            new Vector3(_centerPos.x - _width/2, _centerPos.y - _height/2, _centerPos.z - _depth/2), //1
            new Vector3(_centerPos.x + _width/2, _centerPos.y - _height/2, _centerPos.z - _depth/2), //2
            new Vector3(_centerPos.x + _width/2, _centerPos.y - _height/2, _centerPos.z + _depth/2), //3
            new Vector3(_centerPos.x - _width/2, _centerPos.y - _height/2, _centerPos.z + _depth/2), //4
            new Vector3(_centerPos.x - _width/2, _centerPos.y + _height/2, _centerPos.z - _depth/2), //5
            new Vector3(_centerPos.x + _width/2, _centerPos.y + _height/2, _centerPos.z - _depth/2), //6
            new Vector3(_centerPos.x + _width/2, _centerPos.y + _height/2, _centerPos.z + _depth/2), //7
            new Vector3(_centerPos.x - _width/2, _centerPos.y + _height/2, _centerPos.z + _depth/2) //8
        };
    }

    public bool IsWithinBounds(Vector3 _position)
    {
        if (_position.x > Bounds[0].x && _position.x < Bounds[1].x)
            if (_position.y > Bounds[0].y && _position.y < Bounds[4].y)
                if (_position.z > Bounds[0].z && _position.z < Bounds[2].z)
                    return true;

        return false;
    }
}