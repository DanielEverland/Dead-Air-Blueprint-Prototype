using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math {

    public static Vector2 ClosestPointOnLine(Vector2 start, Vector2 end, Vector2 point)
    {
        Vector2 startToPoint = point - start;
        Vector2 startToEnd = end - start;

        float squaredMagnitude = startToEnd.sqrMagnitude;
        float dotProduct = Vector2.Dot(startToPoint, startToEnd);
        float normalizedDistance = dotProduct / squaredMagnitude;

        return new Vector2()
        {
            x = start.x + startToEnd.x * normalizedDistance,
            y = start.y + startToEnd.y * normalizedDistance,
        };
    }
}
