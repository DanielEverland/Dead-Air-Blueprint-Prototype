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
    [SerializeField]
    private IconSelectionPanel _iconSelectionPanel;

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
        _image.AlignRatio();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        IconSelectionPanel panel = Instantiate(_iconSelectionPanel);

        RectTransform rectTrans = (RectTransform)transform;
        while (rectTrans.GetComponent<Canvas>() == null)
        {
            rectTrans = (RectTransform)rectTrans.parent;
        }

        panel.transform.SetParent(rectTrans, false);
        panel.Initialize();

        panel.Callback += x =>
        {
            _selectedIcon = x;
            Reload();
        };
    }
}
