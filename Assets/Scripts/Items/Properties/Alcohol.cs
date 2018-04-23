using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alcohol : LiquidPropertyBase
{
    public override string Name { get { return "Alcohol"; } }
    public override Color32 Color { get { return UnityEngine.Color.white; } }
    public override bool IsFlammable { get { return true; } }
}
