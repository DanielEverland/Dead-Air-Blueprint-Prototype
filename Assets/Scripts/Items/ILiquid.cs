using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquid : IPropertyRequirement {

    Color32 Color { get; }
    bool IsFlammable { get; }
}
