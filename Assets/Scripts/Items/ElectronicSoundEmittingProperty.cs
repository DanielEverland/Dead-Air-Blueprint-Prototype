using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElectronicSoundEmittingProperty : SoundEmittingProperty,  IElectricityUser
{
    public bool IsReceivingElectricity { get { return AudioIndicator.IsActive; } set { AudioIndicator.Toggle(value); } }

    public abstract float ElectricityRequired { get; }
    
    public override string[] GetInformation()
    {
        List<string> info = new List<string>(base.GetInformation());
        info.Add("Electricity: " + (IsReceivingElectricity ? "On" : "Off"));

        return info.ToArray();
    }
}
