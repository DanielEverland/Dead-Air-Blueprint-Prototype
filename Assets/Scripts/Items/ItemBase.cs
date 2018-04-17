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

    public IEnumerable<IItemProperty> Properties { get { return _properties; } }

    private List<IItemProperty> _properties;

    public GameObject CreateObject()
    {
        GameObject obj = new GameObject();

        foreach (IItemProperty property in _properties)
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
