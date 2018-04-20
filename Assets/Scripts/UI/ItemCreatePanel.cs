﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreatePanel : MonoBehaviour {

    [SerializeField]
    private GameObject _panel;

    private void Update()
    {
        PollToggle();
    }
    private void PollToggle()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TogglePanel();
        }
    }
    public void CreateItem()
    {
        ItemBase item = new ItemBase(IconPanel.SelectedIcon, PropertyPanel.PropertyTypes.ToArray());
        ItemObject obj = ItemObject.Create(item);

        TogglePanel();
    }
    private void TogglePanel()
    {
        _panel.SetActive(!_panel.activeInHierarchy);
    }
}
