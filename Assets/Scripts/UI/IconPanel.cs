using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconPanel : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private Text _text;
    [SerializeField]
    private RawImage _image;

    private Texture2D _selectedIcon;

    private void Start()
    {
        Reload();
    }
    private void Reload()
    {
        bool selected = _selectedIcon != null;

        _text.enabled = !selected;
        _image.enabled = selected;

        _image.texture = _selectedIcon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test");
    }
}
