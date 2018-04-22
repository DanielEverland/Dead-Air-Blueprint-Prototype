using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropertyBase
{
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
    public virtual string[] GetInformation()
    {
        return new string[0];
    }
}
