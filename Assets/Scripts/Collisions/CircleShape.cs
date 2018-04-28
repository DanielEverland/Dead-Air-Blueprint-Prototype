using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShape : IShape
{
    private CircleShape() { }
    public CircleShape(IWorldObject obj)
    {
        _obj = obj;
    }

    private readonly IWorldObject _obj;

    public bool Contains(Vector2 pos)
    {
        Vector2 delta = pos - _obj.Point;

        return _obj.Radius >= delta.magnitude;
    }
    public Vector2 GetEdge(Vector2 pos)
    {
        Vector2 delta = pos - _obj.Point;

        return delta.normalized * _obj.Radius;
    }
}
