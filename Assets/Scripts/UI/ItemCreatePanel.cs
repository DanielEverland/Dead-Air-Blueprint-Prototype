using System.Linq;
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
            _panel.SetActive(!_panel.activeInHierarchy);
        }
    }
    public void CreateItem()
    {

    }
}
