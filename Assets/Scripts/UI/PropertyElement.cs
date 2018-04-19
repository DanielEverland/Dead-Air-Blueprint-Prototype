using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropertyElement : MonoBehaviour, IPointerClickHandler, System.IEquatable<PropertyElement> {

    public System.Type PropertyType { get; private set; }

    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _activeBackground;
    [SerializeField]
    private Image _idleBackground;

    private PropertyPanel _owner;

    public void Initialize(string name, System.Type propertyType, PropertyPanel owner)
    {
        this.name = name;

        _text.text = name;
        _owner = owner;

        PropertyType = propertyType;
    }
    public void Toggle(bool isActive)
    {
        _activeBackground.gameObject.SetActive(isActive);
        _idleBackground.gameObject.SetActive(!isActive);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _owner.Toggle(this);
    }
    public override bool Equals(object other)
    {
        if (other == null)
            return false;

        if(other is PropertyElement)
        {
            return Equals(other as PropertyElement);
        }

        return false;
    }
    public bool Equals(PropertyElement other)
    {
        return other.PropertyType == PropertyType;
    }
    public override int GetHashCode()
    {
        return PropertyType.GetHashCode();
    }
}
