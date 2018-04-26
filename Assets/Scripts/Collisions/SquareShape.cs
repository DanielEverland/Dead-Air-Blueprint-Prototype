using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareShape : IShape
{
    private SquareShape() { }
    public SquareShape(IInformationObject owner, Vector2 extend)
    {
        _owner = owner;
        _extend = extend;
    }

    private float XMin { get { return _owner.Point.x - _extend.x / 2; } }
    private float XMax { get { return _owner.Point.x + _extend.x / 2; } }
    private float YMin { get { return _owner.Point.y - _extend.y / 2; } }
    private float YMax { get { return _owner.Point.y + _extend.y / 2; } }

    private readonly IInformationObject _owner;
    private readonly Vector2 _extend;

    public bool Contains(Vector2 pos)
    {
        return
            pos.x >= XMin
            &&
            pos.x <= XMax
            &&
            pos.y >= YMin
            &&
            pos.y <= YMax;
    }
    public Vector2 GetEdge(Vector2 pos)
    {
        return new Vector2()
        {
            x = GetAxis(pos.x, XMin, XMax),
            y = GetAxis(pos.y, YMin, YMax),
        };
    }
    private float GetAxis(float point, float start, float end)
    {
        if(point < start)
        {
            return start;
        }
        else if(point > end)
        {
            return end;
        }
        else
        {
            return point;
        }
    }
}
