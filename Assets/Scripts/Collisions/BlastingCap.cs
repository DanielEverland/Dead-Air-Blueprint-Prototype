using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastingCap : ElectricalProperty
{
    public override string Name { get { return "Blasting Cap"; } }
    public override float ElectricityRequired { get { return 5; } }

    private bool _hasBlasted;

    public override void OnElectricalUpdate()
    {
        if (!_hasBlasted && IsReceivingElectricity)
        {
            Blast();
        }
    }
    private void Blast()
    {
        _hasBlasted = true;

        Owner.RaiseEvent(PropertyEventTypes.OnBlastingCap);

        Owner.Remove(this);
    }
    public override string[] GetInformation()
    {
        return new string[1] { $"HasBlasted: {_hasBlasted}" };
    }
}
