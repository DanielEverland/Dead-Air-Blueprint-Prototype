using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MethodDefinitions {

    private static readonly Dictionary<PropertyEventTypes, System.Type[]> _parameterTypes = new Dictionary<PropertyEventTypes, System.Type[]>()
    {
        { PropertyEventTypes.OnItemCreated, null },
        { PropertyEventTypes.OnTrigger, null },
        { PropertyEventTypes.OnPlacedInWorld, null },
        { PropertyEventTypes.OnThrowBegins, null },
        { PropertyEventTypes.OnThrowEnds, null },
        { PropertyEventTypes.OnLiquidContainerBreaks, new System.Type[1] { typeof(ILiquidContainerProperty) } },
        { PropertyEventTypes.OnIgnite, null },
        { PropertyEventTypes.OnLiquid, new System.Type[1] { typeof(Liquid) } },
        { PropertyEventTypes.OnCollisionEnter, new System.Type[1] { typeof(Transform) } },
        { PropertyEventTypes.OnCollisionStay, new System.Type[1] { typeof(Transform) } },
        { PropertyEventTypes.OnCollisionExit, new System.Type[1] { typeof(Transform) } },
        { PropertyEventTypes.OnBlastingCap, null },
    };

    public static System.Type[] GetParameterInfo(PropertyEventTypes type)
    {
        if (!_parameterTypes.ContainsKey(type))
            throw new System.NotImplementedException("Missing parameter info definition for " + type);

        return _parameterTypes[type];
    }
}
