using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBottle : PropertyBase, ILiquidContainerProperty, IPropertyInput
{
    public override string Name { get { return "Glass Bottle"; } }

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnThrowEnds; } }

    private void OnThrowEnds()
    {
        Break();
    }
    private void Break()
    {
        Owner.Remove(this);
    }
}
