using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrol : LiquidPropertyBase
{
    public override string Name { get { return "Petrol"; } }
    public override Color32 Color { get { return UnityEngine.Color.yellow; } }
    public override bool IsFlammable { get { return true; } }
}
