using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiquidContainerBase : PropertyBase, ILiquidContainerProperty {

    public string ErrorMessage { get { return "Too many liquid containers!"; } }

    public System.Type BlockedType { get { return typeof(ILiquidContainerProperty); } }

    public abstract float Area { get; }
}
