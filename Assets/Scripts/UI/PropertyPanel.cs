using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanel : MonoBehaviour {

    [SerializeField]
    private PropertyElement _propertyNamePrefab;
    [SerializeField]
    private Transform _propertyParent;

    public static IEnumerable<System.Type> PropertyTypes
    {
        get
        {
            return _currentlySelected.Select(x => x.PropertyType);
        }
    }

    private static HashSet<PropertyElement> _currentlySelected;

    private List<PropertyElement> _elements;

    private void Start()
    {
        _elements = new List<PropertyElement>();
        _currentlySelected = new HashSet<PropertyElement>();

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
        if (_currentlySelected.Contains(active))
            _currentlySelected.Remove(active);
        else
            _currentlySelected.Add(active);

        foreach (PropertyElement element in _elements)
        {
            element.Toggle(_currentlySelected.Contains(element));
        }
    }
}
