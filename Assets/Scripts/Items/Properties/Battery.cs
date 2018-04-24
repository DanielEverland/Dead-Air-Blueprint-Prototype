using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PropertyBase, IPropertyInput, IElectricitySupplier {
    
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnTrigger; } }
    public override string Name { get { return "Battery"; } }
    public float MaxCharge { get { return 100; } }
    public float CurrentCharge { get; set; }
    
    private bool _isEnabled;

    private void OnTrigger()
    {
        _isEnabled = true;
    }
    public override string[] GetInformation()
    {
        return new string[2]
        {
            "Enabled: " + _isEnabled,
            "Charge: " + CurrentCharge.ToString("F0"),
        };
    }
}
