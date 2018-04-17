using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase {

    /// <summary>
    /// Declares an item with properties
    /// </summary>
    public ItemBase(params System.Type[] properties)
    {
        _properties = new PropertyCollection();

        foreach (System.Type type in properties)
        {
            if(!typeof(PropertyBase).IsAssignableFrom(type))
            {
                throw new System.ArgumentException("Tried to create a PropertyBase instance from " + type);
            }

            PropertyBase property = (PropertyBase)System.Activator.CreateInstance(type, this);
            _properties.RegisterProperty(property);
        }

        RaiseEvent(PropertyEventTypes.OnItemCreated, null);
    }

    /// <summary>
    /// Combines items
    /// </summary>
    public ItemBase(params ItemBase[] items)
    {
        _properties = new PropertyCollection();

        Merge(items);
    }

    public IEnumerable<PropertyBase> Properties { get { return _properties; } }

    private PropertyCollection _properties;

    public bool ContainsOutput(PropertyEventTypes type)
    {
        return _properties.ContainsOutput(type);
    }
    public bool ContainsInput(PropertyEventTypes type)
    {
        return _properties.ContainsInput(type);
    }
    public void RaiseEvent(PropertyEventTypes type, params object[] parameters)
    {
        _properties.RaiseEvent(type, parameters);
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
            _properties.RegisterProperties(item.Properties);
        }
    }
}
