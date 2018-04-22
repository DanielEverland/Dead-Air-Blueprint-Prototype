using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiquidPropertyBase : PropertyBase, ILiquid, IPropertyInput
{
    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnLiquidContainerBreaks; } }
    
    public abstract Color32 Color { get; }

    public virtual bool IsValid(IEnumerable<PropertyBase> properties, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!properties.Any(x => x is ILiquidContainerProperty))
        {
            errorMessage = "Missing liquid container";
            return false;
        }

        return true;
    }
    private void OnLiquidContainerBreaks(ILiquidContainerProperty container)
    {
        LiquidData data = new LiquidData()
        {
            Radius = Utility.AreaToRadius(container.Area),
            Color = Color,
        };

        Liquid liquid = Liquid.Create(data);
        liquid.transform.position = Owner.Object.transform.position;

        Owner.Remove(this);
    }
}
