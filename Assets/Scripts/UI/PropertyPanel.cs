using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanel : MonoBehaviour {

    [SerializeField]
    private PropertyElement _propertyNamePrefab;
    [SerializeField]
    private Transform _propertyParent;

    public static event System.Action OnChanged;

    public static IEnumerable<PropertyBase> SelectedProperties { get { return _currentlySelected.Values; } }

    private static Dictionary<PropertyElement, PropertyBase> _currentlySelected;
    private static List<PropertyElement> _elements;

    public static void Reset()
    {
        _currentlySelected.Clear();

        DrawSelection();
    }
    private static void DrawSelection()
    {
        foreach (PropertyElement element in _elements)
        {
            element.Toggle(_currentlySelected.ContainsKey(element));
        }
    }

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

        DrawSelection();

        if (OnChanged != null)
            OnChanged.Invoke();
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
