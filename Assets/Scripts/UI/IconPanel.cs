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
    private Image _image;
    [SerializeField]
    private IconSelectionPanel _iconSelectionPanel;

    public static Sprite SelectedIcon { get; private set; }

    private void Start()
    {
        Reload();
    }
    private void Reload()
    {
        bool selected = SelectedIcon != null;

        _text.enabled = !selected;
        _image.enabled = selected;

        _image.sprite = SelectedIcon;
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
            SelectedIcon = x;
            Reload();
        };
    }
}
