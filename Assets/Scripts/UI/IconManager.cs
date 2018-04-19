using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IconManager {

    public static IEnumerable<Texture2D> Icons
    {
        get
        {
            if (_icons == null)
                LoadIcons();

            return _icons;
        }
    }

    private static List<Texture2D> _icons;

    private static void LoadIcons()
    {
        _icons = new List<Texture2D>(Resources.LoadAll<Texture2D>("Icons"));
    }
}
