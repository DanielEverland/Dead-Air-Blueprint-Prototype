using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : LiquidPropertyBase
{
    public override string Name { get { return "Water"; } }
    public override Color32 Color { get { return UnityEngine.Color.cyan; } }
    public override bool IsFlammable { get { return false; } }
}
