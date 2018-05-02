using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightProperty : ElectricalProperty
{
    public override float ElectricityRequired { get { return 20; } }
    public override bool IsReceivingElectricity { get { return _light.enabled; } set { _light.enabled = value; } }

    public override string Name { get { return "Light"; } }
    
    private Light _light;
    
    public override void CreateInstance(GameObject obj)
    {
        IEnumerable<Component> components = PropertyHelper.CopyComponents("Light", obj);

        _light = components.First<Light>();
        _light.enabled = false;
    }
    public override string[] GetInformation()
    {
        return new string[1]
        {
            "Enabled: " + _light.enabled,
        };
    }
}