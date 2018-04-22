using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrekableLiquidsContainer : LiquidContainerBase, IPropertyInput {

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnThrowEnds; } }

    private void OnThrowEnds()
    {
        Break();
    }
    private void Break()
    {
        Owner.RaiseEvent(PropertyEventTypes.OnLiquidContainerBreaks, this);
        Owner.Remove(this);
    }
}
