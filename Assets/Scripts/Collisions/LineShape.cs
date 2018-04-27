using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Since the line has a thickness, it's essentially just a non-axis-aligned bounding box
public class LineShape : IShape {

    private LineShape() { }
    public LineShape(Vector2 start, Vector2 end, float thickness)
    {
        _start = start;
        _end = end;
        _thickness = thickness;

        RecalculateBoundingBox();
    }

    public BoundingBox BoundingBox { get { return _boundingBox; } }
    public Vector2 Start
    {
        get
        {
            return _start;
        }
        set
        {
            _start = value;

            RecalculateBoundingBox();
        }
    }
    public Vector2 End
    {
        get
        {
            return _end;
        }
        set
        {
            _end = value;

            RecalculateBoundingBox();
        }
    }
    public float Thickness
    {
        get
        {
            return _thickness;
        }
        set
        {
            _thickness = value;

            RecalculateBoundingBox();
        }
    }

    private Vector2 _start;
    private Vector2 _end;
    private float _thickness;

    private BoundingBox _boundingBox;

    public bool Contains(Vector2 point)
    {
        return _boundingBox.Contains(point);
    }
    public Vector2 GetEdge(Vector2 point)
    {
        return _boundingBox.ClosestPoint(point);
    }
    public void RecalculateBoundingBox()
    {
        _boundingBox = BoundingBox.Create(Start, End, Thickness);
    }
}
