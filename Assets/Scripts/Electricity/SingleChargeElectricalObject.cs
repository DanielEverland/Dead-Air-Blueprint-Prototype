using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A property that can only be fired once
/// </summary>
public abstract class SingleChargeElectricalObject : ElectricalProperty
{
    /// <summary>
    /// The amount of energy to use
    /// </summary>
    protected abstract float ChargeUsed { get; }

    public override float ElectricityRequired { get { return ChargeUsed * ElectricityManager.UPDATES_PER_SECOND; } }
    public virtual bool RemoveWhenUsed { get { return false; } }

    protected bool HasBeenUsed { get; private set; }

    public override void OnElectricalUpdate()
    {
        if(IsReceivingElectricity && !HasBeenUsed)
        {
            HasBeenUsed = true;

            OnChargeUsed();

            if (RemoveWhenUsed)
            {
                Owner.Remove(this);
            }
        }
    }
    protected abstract void OnChargeUsed();

    public override string[] GetInformation()
    {
        return new string[1] { $"Has Been Used: {HasBeenUsed}" };
    }
}
