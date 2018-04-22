using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiquidPropertyBase : PropertyBase, ILiquid
{
    public virtual bool IsValid(IEnumerable<PropertyBase> properties, ref string errorMessage)
    {
        if(!properties.Any(x => x is ILiquidContainerProperty))
        {
            errorMessage = "Missing liquid container";
            return false;
        }

        return true;
    }
}
