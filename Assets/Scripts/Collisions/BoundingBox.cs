using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BoundingBox : System.IEquatable<BoundingBox> {

    public BoundingBox(Vector3 size)
    {
        Size = size;

        Center = Vector3.zero;
        Rotation = Quaternion.identity;
    }
    public BoundingBox(Vector3 center, Vector3 size)
    {
        Center = center;
        Size = size;

        Rotation = Quaternion.identity;
    }
    public BoundingBox(Vector3 center, Vector3 size, Vector3 eulerAngles)
    {
        Center = center;
        Size = size;
        Rotation = Quaternion.Euler(eulerAngles);
    }
    public BoundingBox(Vector3 center, Vector3 size, Quaternion rotation)
    {
        Center = center;
        Size = size;
        Rotation = rotation;
    }

    public Vector3 Center;
    public Vector3 Size;
    public Quaternion Rotation;

    public bool Contains(Vector3 point)
    {
        Vector3 rotatedPoint = Matrix4x4.TRS(Center, Rotation, Vector3.one).inverse.MultiplyPoint(point);

        return new Bounds(Vector3.zero, Size).Contains(rotatedPoint);
    }
    public Vector3 ClosestPoint(Vector3 point)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(Center, Rotation, Vector3.one);

        Vector3 rotatedPoint = matrix.inverse.MultiplyPoint(point);
        Vector3 AABBclosestPoint = new Bounds(Vector3.zero, Size).ClosestPoint(rotatedPoint);

        return matrix.MultiplyPoint(AABBclosestPoint);
    }
    public static BoundingBox Create(GameObject obj)
    {
        return Create(obj.transform);
    }
    public static BoundingBox Create(Transform transform)
    {
        return new BoundingBox(transform.position, transform.localScale, transform.rotation);
    }
    public static BoundingBox Create(Vector2 start, Vector2 end, float thickness)
    {
        Vector2 min = Vector2.Min(start, end);
        Vector2 max = Vector2.Max(start, end);
        Vector2 rawDelta = end - start;
        Vector2 fixedDelta = max - min;

        if (fixedDelta.sqrMagnitude == 0)
            return new BoundingBox(start, end, Quaternion.identity);

        float angle = Mathf.Atan2(rawDelta.y, rawDelta.x) * Mathf.Rad2Deg;

        Vector2 center = min + fixedDelta / 2;
        Vector3 size = new Vector3(Mathf.Abs(fixedDelta.magnitude), thickness, 1);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        return new BoundingBox(center, size, rotation);
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (obj is BoundingBox)
        {
            return Equals((BoundingBox)obj);
        }

        return false;
    }
    public bool Equals(BoundingBox other)
    {
        return
            other.Center == Center
            &&
            other.Size == Size
            &&
            other.Rotation == Rotation;
    }
    public override int GetHashCode()
    {
        int i = 17;

        unchecked
        {
            i += Center.GetHashCode() * 7;
            i += Size.GetHashCode() * 5;
            i += Rotation.GetHashCode() * 23;
        }

        return i;
    }
    public override string ToString()
    {
        return base.ToString();
    }
}