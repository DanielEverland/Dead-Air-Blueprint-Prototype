﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlamableProperty : PropertyBase, IFlamableProperty
{
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnIgnite | PropertyEventTypes.OnPlacedInWorld | PropertyEventTypes.OnLiquid; } }

    private bool _isIgnited;
    
    private void OnLiquid(Liquid liquid)
    {
        if (!liquid.IsFlammable)
        {
            _isIgnited = false;
        }
        else if (liquid.IsOnFire && !_isIgnited)
        {
            _isIgnited = true;
        }
    }
    private void OnPlacedInWorld()
    {
        if (_isIgnited)
        {
            WorldItemEventHandler.RaiseEvent(Owner.Object, PropertyEventTypes.OnIgnite);
        }
    }
    private void OnIgnite()
    {
        _isIgnited = true;
    }
    public override string[] GetInformation()
    {
        return new string[1] { "IsIgnited: " + _isIgnited };
    }
}
