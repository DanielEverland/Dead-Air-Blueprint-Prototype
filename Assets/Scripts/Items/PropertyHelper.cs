﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PropertyHelper {

    /// <summary>
    /// Grabs a GameObject from Resources using <paramref name="objectName"/> and copies all its components to <paramref name="target"/>
    /// </summary>
	public static IEnumerable<Component> CopyComponents(string objectName, GameObject target)
    {
        GameObject obj = Resources.Load<GameObject>("Properties/" + objectName);

        return CopyComponents(obj, target);
    }
    /// <summary>
    /// Copies all components from <paramref name="from"/> and adds them to <paramref name="to"/>
    /// </summary>
    public static IEnumerable<Component> CopyComponents(GameObject from, GameObject to)
    {
        List<Component> components = new List<Component>();

        foreach (Component comp in from.GetComponents<Component>())
        {
            if (typeof(Transform).IsAssignableFrom(comp.GetType()))
                continue;

            Component newComp = to.AddComponent(comp.GetType());
            UnityEditor.EditorUtility.CopySerialized(comp, newComp);

            components.Add(newComp);
        }

        return components;
    }
}
