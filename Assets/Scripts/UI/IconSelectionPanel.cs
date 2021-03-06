﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelectionPanel : MonoBehaviour {

    [SerializeField]
    private IconSelectionButton _iconButton;
    [SerializeField]
    private RectTransform _buttonParent;

    public System.Action<Sprite> Callback;

    public void Initialize()
    {
        //So we can de-select an icon
        AddButton(null);

        foreach (Sprite icon in IconManager.Icons)
        {
            AddButton(icon);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_buttonParent);
    }
    private void AddButton(Sprite icon)
    {
        IconSelectionButton button = Instantiate(_iconButton);
        button.transform.SetParent(_buttonParent);

        button.Initialize(icon, this);
    }
    public void Select(Sprite icon)
    {
        if (Callback != null)
            Callback.Invoke(icon);

        Destroy(gameObject);
    }
}
