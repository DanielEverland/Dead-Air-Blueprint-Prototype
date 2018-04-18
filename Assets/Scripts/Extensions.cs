using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    public static T First<T>(this IEnumerable collection)
    {
        foreach (object obj in collection)
        {
            if (obj.GetType() == typeof(T))
                return (T)obj;
        }

        throw new System.NullReferenceException();
    }
}
