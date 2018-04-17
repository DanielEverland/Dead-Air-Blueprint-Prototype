using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : PropertyBase, IPropertyInput, IPropertyOutput {

    public Timer(ItemBase owner) : base(owner) { }

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnPlacedInWorld; } }
    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnTrigger; } }
    
    private void OnPlacedInWorld()
    {

    }
}
