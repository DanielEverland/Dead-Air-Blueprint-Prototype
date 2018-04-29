using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitch : PropertyBase, IPropertyInput, IPropertyOutput
{
    public override string Name { get { return "Pressure Switch"; } }

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnCollisionEnter; } }
    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnTrigger; } }

    private bool _hasTriggered;

    private void OnCollisionEnter(Transform hit)
    {
        if (!_hasTriggered)
        {
            _hasTriggered = true;
            Owner.RaiseEvent(PropertyEventTypes.OnTrigger, null);
        }        
    }
    public override string[] GetInformation()
    {
        return new string[1] { "HasTriggered: " + _hasTriggered };
    }
}
