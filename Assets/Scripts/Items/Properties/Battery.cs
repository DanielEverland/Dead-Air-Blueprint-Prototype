using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PropertyBase, IPropertyOutput {

    public Battery(ItemBase owner) : base(owner) { }

    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnElectricalInput; } }

    public override void Update()
    {
        Owner.RaiseEvent(PropertyEventTypes.OnElectricalInput, null);
    }
}
