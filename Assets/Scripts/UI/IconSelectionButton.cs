using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IconSelectionButton : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private RawImage _image;

    private IconSelectionPanel _owner;

    private void Start()
    {
        _image.AlignRatio();
    }
    public void Initialize(Texture2D icon, IconSelectionPanel owner)
    {
        _image.enabled = icon != null;

        _image.texture = icon;
        _owner = owner;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _owner.Select(_image.texture);
    }
}
