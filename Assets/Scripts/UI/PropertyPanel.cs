using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanel : MonoBehaviour {

    [SerializeField]
    private PropertyElement _propertyNamePrefab;
    [SerializeField]
    private Transform _propertyParent;

    private PropertyElement _currentlySelected;
    private List<PropertyElement> _elements;

    private void Start()
    {
        _elements = new List<PropertyElement>();

        foreach (PropertyBase property in ReflectionManager.PropertyTypes)
        {
            PropertyElement element = Instantiate(_propertyNamePrefab);
            element.Initialize(property.Name, element.GetType(), this);

            element.transform.SetParent(_propertyParent);
        }
    }
    public void Select(PropertyElement active)
    {
        _currentlySelected = active;

        foreach (PropertyElement element in _elements)
        {
            if (element == active)
                element.Enable();
            else
                element.Disable();
        }
    }
}
