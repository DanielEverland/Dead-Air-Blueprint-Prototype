using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElectronicSoundEmittingProperty : SoundEmittingProperty, IPropertyInput
{
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnElectricalInputChanged; } }

    private void OnElectricalInputChanged()
    {
        AudioIndicator.Toggle(Owner.IsElectricallyCharged);
    }
    public override string[] GetInformation()
    {
        List<string> info = new List<string>(base.GetInformation());
        info.Add("Electricity: " + (Owner.IsElectricallyCharged ? "On" : "Off"));

        return info.ToArray();
    }
}
