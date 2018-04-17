using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyBase
{
    private PropertyBase() { }
    public PropertyBase(ItemBase owner)
    {
        _owner = owner;
        _subscriptions = new Dictionary<EventType, System.Action<object>>();
    }

    private readonly ItemBase _owner;

    private Dictionary<EventType, System.Action<object>> _subscriptions;

    /// <summary>
    /// Subscribe to a given event
    /// </summary>
    protected void Subscribe(EventType type, System.Action<object> callback)
    {
        if (_subscriptions.ContainsKey(type))
            throw new System.ArgumentException("You've already subscribed to " + type);

        if (callback == null)
            throw new System.NullReferenceException("Callback is null!");

        _subscriptions.Add(type, callback);
    }
    public void SendEvent(EventType type, object data = null)
    {
        _owner.Output(type, data);
    }
    public void ReceiveInput(EventType type, object data = null)
    {
        if (_subscriptions.ContainsKey(type))
            _subscriptions[type].Invoke(data);
    }

    public abstract void CreateInstance(GameObject obj);
}
