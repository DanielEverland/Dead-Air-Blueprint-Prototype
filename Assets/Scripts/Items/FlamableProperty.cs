using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlamableProperty : PropertyBase, IFlamableProperty
{
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnIgnite; } }

    private bool _isIgnited;

    private void OnIgnite()
    {
        _isIgnited = true;
    }
    public override string[] GetInformation()
    {
        return new string[1] { "IsIgnited: " + _isIgnited };
    }
}
