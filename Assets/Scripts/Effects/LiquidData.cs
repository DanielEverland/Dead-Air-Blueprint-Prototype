using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LiquidData : System.IEquatable<LiquidData> {

    public Color Color;
    public float Radius;
        
    public static bool operator ==(LiquidData a, LiquidData b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(LiquidData a, LiquidData b)
    {
        return !a.Equals(b);
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if(obj is LiquidData)
        {
            return Equals((LiquidData)obj);
        }

        return false;
    }
    public bool Equals(LiquidData other)
    {
        return other.Color == Color;
    }
    public override int GetHashCode()
    {
        int i = 13;

        unchecked
        {
            i += Color.GetHashCode() * 17;
            i += Radius.GetHashCode() * 13;
        }

        return i;
    }
}
