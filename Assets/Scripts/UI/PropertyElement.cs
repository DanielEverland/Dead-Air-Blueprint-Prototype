using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyElement : MonoBehaviour {

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
    private void OnClick()
    {
        _owner.Select(this);
    }
    public void Enable()
    {
        _idleBackground.gameObject.SetActive(false);
        _activeBackground.gameObject.SetActive(true);
    }
    public void Disable()
    {
        _idleBackground.gameObject.SetActive(true);
        _activeBackground.gameObject.SetActive(false);
    }
}
