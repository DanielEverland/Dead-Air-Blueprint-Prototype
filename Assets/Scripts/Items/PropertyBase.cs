using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyBase
{
    private PropertyBase() { }
    public PropertyBase(ItemBase owner)
    {
        _owner = owner;
    }
    
    private readonly ItemBase _owner;
        
    public virtual void CreateInstance(GameObject obj) { }
}
