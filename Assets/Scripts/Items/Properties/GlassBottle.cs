using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBottle : BrekableLiquidsContainer
{
    public override string Name { get { return "Glass Bottle"; } }
    public override float Area { get { return 65; } }
}
