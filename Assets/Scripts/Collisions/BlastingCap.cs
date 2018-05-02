using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastingCap : SingleChargeElectricalObject
{
    public override string Name { get { return "Blasting Cap"; } }

    protected override float ChargeUsed { get { return 5; } }

    public override bool RemoveWhenUsed => true;

    protected override void OnChargeUsed()
    {
        Owner.RaiseEvent(PropertyEventTypes.OnBlastingCap);

        Owner.Remove(this);
    }
}
