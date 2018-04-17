using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase {

    /// <summary>
    /// Combines items
    /// </summary>
    public ItemBase(params ItemBase[] items)
    {
        Merge(items);
    }

    public IEnumerable<PropertyBase> Properties { get { return _properties; } }

    private List<PropertyBase> _properties;

    public void Output(EventType eventType, object data)
    {
        foreach (PropertyBase property in _properties)
        {
            property.ReceiveInput(eventType, data);
        }
    }
    public GameObject CreateObject()
    {
        GameObject obj = new GameObject();

        foreach (PropertyBase property in _properties)
        {
            property.CreateInstance(obj);
        }

        return obj;
    }

    /// <summary>
    /// Copies the properties of <paramref name="items"/> into the current instance
    /// </summary>
    /// <param name="items"></param>
    public void Merge(IEnumerable<ItemBase> items)
    {
        foreach (ItemBase item in items)
        {
            _properties.AddRange(item.Properties);
        }
    }
}
