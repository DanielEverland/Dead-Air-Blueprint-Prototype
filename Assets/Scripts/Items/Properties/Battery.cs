using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PropertyBase, IPropertyOutput, IPropertyInput {

    public Battery(ItemBase owner) : base(owner) { }

    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnElectricalInput; } }
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnTrigger; } }

    private bool _isEnabled;

    private void OnTrigger()
    {
        _isEnabled = true;
    }
    public override void Update()
    {
        if (_isEnabled)
            Owner.RaiseEvent(PropertyEventTypes.OnElectricalInput, null);
    }
}
