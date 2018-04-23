using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LiquidData : System.IEquatable<LiquidData> {

    public LiquidData(ILiquidContainerProperty container, LiquidPropertyBase liquid)
    {
        Radius = Utility.AreaToRadius(container.Area);
        Color = liquid.Color;
        IsFlammable = liquid.IsFlammable;
        IsOnFire = false;
    }

    public bool IsFlammable;
    public bool IsOnFire;
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
        return
            other.Color == Color
            &&
            other.IsFlammable == IsFlammable
            &&
            other.IsOnFire == IsOnFire;
    }
    public override int GetHashCode()
    {
        int i = 13;

        unchecked
        {
            i += Color.GetHashCode() * 17;
            i += Radius.GetHashCode() * 13;
            i += IsFlammable.GetHashCode() * 5;
            i += IsOnFire.GetHashCode() * 7;
        }

        return i;
    }
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Color: " + Color);
        builder.AppendLine("Radius: " + Radius);
        builder.AppendLine("IsFlammable: " + IsFlammable);
        builder.AppendLine("IsOnFire: " + IsOnFire);

        return builder.ToString();
    }
}
