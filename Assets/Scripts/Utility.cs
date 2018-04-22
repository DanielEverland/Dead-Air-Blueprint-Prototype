using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static float AreaToRadius(float area)
    {
        return Mathf.Sqrt(area / Mathf.PI);
    }
}
