using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyBase
{
    public PropertyBase()
    {

    }
    public PropertyBase(ItemBase owner)
    {
        AssignOwner(owner);
    }

    protected ItemBase Owner { get; private set; }
    
    public abstract string Name { get; }
    
    public virtual void Update() { }
    public virtual void CreateInstance(GameObject obj) { }

    public void AssignOwner(ItemBase owner)
    {
        if (Owner != null)
            throw new System.InvalidCastException("Owner has already been assigned");

        Owner = owner;
    }
}
