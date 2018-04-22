using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreatePanel : MonoBehaviour {

    [SerializeField]
    private GameObject _panel;

    private void Start()
    {
        CreateItem(IconManager.Icons.Random(), null);
    }
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
        CreateItem(IconPanel.SelectedIcon, PropertyPanel.SelectedProperties.ToArray());
    }
    public void CreateItem(Sprite sprite, PropertyBase[] propertyTypes)
    {
        ItemBase item = new ItemBase(sprite, propertyTypes);
        ItemObject obj = ItemObject.Create(item);

        ItemObjectHandler.HandleItem(obj);

        _panel.SetActive(false);
    }
    private void TogglePanel()
    {
        _panel.SetActive(!_panel.activeInHierarchy);
        _panel.transform.SetAsLastSibling();
    }
}
