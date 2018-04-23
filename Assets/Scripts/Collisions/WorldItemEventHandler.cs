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

    private static List<Event> _events = new List<Event>();
    private static int _lastFrameCalled = -1;

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
    public static void Update()
    {
        if (_lastFrameCalled == Time.frameCount)
            throw new System.InvalidOperationException("Update called more than once this frame");

        PollEvents();

        _lastFrameCalled = Time.frameCount;
    }
    private static void PollEvents()
    {
        foreach (Event item in _events)
        {
            item.Poll();
        }
    }
    public static void RaiseEvent(IWorldObject obj, PropertyEventTypes type, params object[] args)
    {
        _events.Add(new Event(type, 0, obj.Point, obj.Radius, args));
    }
    public static void RaiseEvent(IWorldObject obj, float waitTime, PropertyEventTypes type, params object[] args)
    {
        _events.Add(new Event(type, waitTime, obj.Point, obj.Radius, args));
    }
    public static void RaiseEvent(Vector2 point, float radius, PropertyEventTypes type, params object[] args)
    {
        _events.Add(new Event(type, 0, point, radius, args));
    }
    public static void RaiseEvent(Vector2 point, float radius, float waitTime, PropertyEventTypes type, params object[] args)
    {
        _events.Add(new Event(type, waitTime, point, radius, args));
    }
    private static void RaiseEventInternal(Vector2 point, float radius, PropertyEventTypes type, params object[] args)
    {
        foreach (IWorldObject obj in GetCollidingObjects(point, radius))
        {
            obj.RaiseEvent(type, args);
        }
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
    private static IEnumerable<IWorldObject> GetCollidingObjects(Vector2 point, float radius)
    {
        List<IWorldObject> collidingObjects = new List<IWorldObject>();

        foreach (IWorldObject obj in _objects)
        {
            if(Collides(point, radius, obj.Point, obj.Radius))
            {
                collidingObjects.Add(obj);
            }
        }

        return collidingObjects;
    }
    private static bool Collides(IWorldObject a, IWorldObject b)
    {
        return Collides(a.Point, a.Radius, b.Point, b.Radius);
    }
    private static bool Collides(Vector2 aPoint, float aRadius, Vector2 bPoint, float bRadius)
    {
        float distance = Vector2.Distance(aPoint, bPoint);

        return distance <= aRadius + bRadius;
    }

    private struct Event
    {
        public Event(PropertyEventTypes type, float waitTime, Vector2 point, float radius, params object[] args)
        {
            _type = type;
            _waitTime = waitTime;
            _args = args;
            _radius = radius;
            _point = point;
        }

        private PropertyEventTypes _type;
        private Vector2 _point;
        private float _waitTime;
        private float _radius;
        private object[] _args;

        public void Poll()
        {
            _waitTime -= Time.deltaTime;

            if(_waitTime <= 0)
            {
                RaiseEvent(_point, _radius, _type, _args);
            }
        }
    }
}
