using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : PropertyBase, IPropertyInput {

    public Timer(ItemBase owner) : base(owner) { }

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnItemCreated; } }
    
}
