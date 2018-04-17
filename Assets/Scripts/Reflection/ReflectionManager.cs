using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectionManager {

    public static IEnumerable<Type> PropertyTypes
    {
        get
        {
            if (_propertyTypes == null)
                Initialize();

            return _propertyTypes;
        }
    }

    private static List<Type> _propertyTypes;

    private static void Initialize()
    {
        _propertyTypes = new List<Type>();

        ExecuteReflection();
    }
    private static void ExecuteReflection()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsAbstract)
                continue;

            if (typeof(PropertyBase).IsAssignableFrom(type))
                _propertyTypes.Add(type);
        }
    }
}
