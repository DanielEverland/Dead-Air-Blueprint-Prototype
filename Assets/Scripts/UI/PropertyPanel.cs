using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanel : MonoBehaviour {

    [SerializeField]
    private PropertyElement _propertyNamePrefab;
    [SerializeField]
    private Transform _propertyParent;

    public static IEnumerable<PropertyBase> SelectedProperties
    {
        get
        {
            return _currentlySelected.Values;
        }
    }

    private static Dictionary<PropertyElement, PropertyBase> _currentlySelected;

    private List<PropertyElement> _elements;

    private void Start()
    {
        _elements = new List<PropertyElement>();
        _currentlySelected = new Dictionary<PropertyElement, PropertyBase>();

        foreach (PropertyBase property in ReflectionManager.PropertyTypes)
        {
            PropertyElement element = Instantiate(_propertyNamePrefab);
            element.Initialize(property.Name, property.GetType(), this);

            element.transform.SetParent(_propertyParent);

            _elements.Add(element);
        }
    }
    public void Toggle(PropertyElement active)
    {
        if (_currentlySelected.ContainsKey(active))
        {
            Remove(active);
        }
        else
        {
            Create(active);
        }

        foreach (PropertyElement element in _elements)
        {
            element.Toggle(_currentlySelected.ContainsKey(element));
        }
    }
    private void Create(PropertyElement element)
    {
        PropertyBase property = (PropertyBase)System.Activator.CreateInstance(element.PropertyType);

        _currentlySelected.Add(element, property);
    }
    private void Remove(PropertyElement element)
    {
        _currentlySelected.Remove(element);
    }
}
