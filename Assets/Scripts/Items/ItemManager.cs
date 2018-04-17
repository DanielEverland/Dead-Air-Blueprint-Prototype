using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager {
    
    public static void Spawn(ItemBase item)
    {
        GameObject obj = item.CreateObject();
        ItemObject container = obj.AddComponent<ItemObject>();

        container.Item = item;

        item.RaiseEvent(PropertyEventTypes.OnPlacedInWorld, null);
    }
    public static void Test()
    {
        ItemBase item = new ItemBase(ReflectionManager.PropertyTypes.ToArray());

        Spawn(item);
    }
}
