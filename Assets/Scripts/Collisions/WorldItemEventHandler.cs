using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles events and collisions between world-based items
/// such as liquids and items
/// </summary>
public static class WorldItemEventHandler {

    /// <summary>
    /// No data-model to optimize collision handling. Simple brute force
    /// </summary>
    private static List<IWorldObject> _objects = new List<IWorldObject>();

    public static void Add(IWorldObject obj)
    {
        _objects.Add(obj);

        Poll(obj);
    }
    public static void Remove(IWorldObject obj)
    {
        _objects.Remove(obj);
    }
    public static void Poll(IWorldObject obj)
    {
        ResolveCollisions(obj);
    }
    private static void ResolveCollisions(IWorldObject obj)
    {
        foreach (IWorldObject otherObject in _objects)
        {
            if (otherObject == obj)
                continue;

            if(Collides(otherObject, obj))
            {
                obj.HandleCollision(otherObject);
                otherObject.HandleCollision(obj);
            }
        }
    }
    private static bool Collides(IWorldObject a, IWorldObject b)
    {
        float distance = Vector2.Distance(a.Point, b.Point);

        return distance < a.Radius + b.Radius;
    }
}
