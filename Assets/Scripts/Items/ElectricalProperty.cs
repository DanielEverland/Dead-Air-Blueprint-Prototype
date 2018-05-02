using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElectricalProperty : PropertyBase, IElectricityUser
{
    public abstract float ElectricityRequired { get; }

    public virtual bool IsReceivingElectricity { get; set; }

    public virtual void OnElectricalUpdate() { }
}
