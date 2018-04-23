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
        //We have to do some caching here, so we can add
        //new events during the execution without worrying
        //about overflowing the stack
        List<Event> currentEvents = new List<Event>(_events);
        foreach (Event item in currentEvents)
        {
            if (item.Poll())
            {
                RaiseEventInternal(item);
            }
        }
    }
    public static void RaiseEvent(IWorldObject obj, PropertyEventTypes type, params object[] args)
    {
        RaiseEvent(obj, obj.Point, obj.Radius, 0, type, args);
    }
    public static void RaiseEvent(IWorldObject obj, float waitTime, PropertyEventTypes type, params object[] args)
    {
        RaiseEvent(obj, obj.Point, obj.Radius, waitTime, type, args);
    }
    public static void RaiseEvent(Vector2 point, float radius, PropertyEventTypes type, params object[] args)
    {
        RaiseEvent(null, point, radius, 0, type, args);
    }
    public static void RaiseEvent(IWorldObject sender, Vector2 point, float radius, float waitTime, PropertyEventTypes type, params object[] args)
    {
        Debug.Log("Adding world event " + type);

        _events.Add(new Event(sender, type, waitTime, point, radius, args));
    }
    private static void RaiseEventInternal(Event item)
    {
        _events.Remove(item);

        foreach (IWorldObject obj in GetCollidingObjects(item.Point, item.Radius))
        {
            if (item.Sender == obj)
                continue;

            obj.RaiseEvent(item.Type, item.Arguments);
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

    private class Event
    {
        public Event(IWorldObject sender, PropertyEventTypes type, float waitTime, Vector2 point, float radius, params object[] args)
        {
            Sender = sender;
            Type = type;
            WaitTime = waitTime;
            Arguments = args;
            Radius = radius;
            Point = point;
        }

        public IWorldObject Sender;
        public PropertyEventTypes Type;
        public Vector2 Point;
        public float WaitTime;
        public float Radius;
        public object[] Arguments;

        public bool Poll()
        {
            WaitTime -= Time.deltaTime;

            if(WaitTime <= 0)
            {
                return true;
            }

            return false;
        }
    }
}
