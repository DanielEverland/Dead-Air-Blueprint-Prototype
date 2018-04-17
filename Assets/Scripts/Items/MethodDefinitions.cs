using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MethodDefinitions {
    
    private static readonly Dictionary<PropertyEventTypes, System.Type[]> _parameterTypes = new Dictionary<PropertyEventTypes, System.Type[]>()
    {
        { PropertyEventTypes.OnItemCreated, null },
        { PropertyEventTypes.OnElectricalInput, null },
        { PropertyEventTypes.OnTrigger, null },
        { PropertyEventTypes.OnPlacedInWorld, null },
    };

    public static System.Type[] GetParameterInfo(PropertyEventTypes type)
    {
        if (!_parameterTypes.ContainsKey(type))
            throw new System.NotImplementedException("Missing parameter info definition for " + type);

        return _parameterTypes[type];
    }
}
