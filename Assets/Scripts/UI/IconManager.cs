using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IconManager {

    public static IEnumerable<Sprite> Icons
    {
        get
        {
            if (_icons == null)
                LoadIcons();

            return _icons;
        }
    }

    private static List<Sprite> _icons;

    private static void LoadIcons()
    {
        _icons = new List<Sprite>(Resources.LoadAll<Sprite>("Icons"));
    }
}
