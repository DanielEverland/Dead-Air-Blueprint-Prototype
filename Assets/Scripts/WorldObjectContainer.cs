using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldObjectContainer {

    static WorldObjectContainer()
    {
        _itemObjects = new List<ItemObject>();
    }

    public static IEnumerable<ItemObject> ItemObjects { get { return _itemObjects; } }

    private static List<ItemObject> _itemObjects;

    public static void AddItemObject(ItemObject obj)
    {
        if (!_itemObjects.Contains(obj))
            _itemObjects.Add(obj);
    }
    public static void RemoveItemObject(ItemObject obj)
    {
        if(_itemObjects.Contains(obj))
            _itemObjects.Remove(obj);
    }
}