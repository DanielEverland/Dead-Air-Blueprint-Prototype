using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour {

    static InformationManager()
    {
        _trackedObjects = new List<ItemObject>();
    }

    [SerializeField]
    private InformationElement _elementPrefab;

    private static System.Action<ItemObject> _onObjectAdded;
    private static System.Action<ItemObject> _onObjectRemoved;
    private static List<ItemObject> _trackedObjects;

    private Dictionary<ItemObject, InformationElement> _elements;

    public static void Add(ItemObject obj)
    {
        _trackedObjects.Add(obj);

        if(_onObjectAdded != null)
            _onObjectAdded.Invoke(obj);
    }
    public static void Remove(ItemObject obj)
    {
        _trackedObjects.Remove(obj);

        if (_onObjectRemoved != null)
            _onObjectRemoved.Invoke(obj);
    }

    private void Awake()
    {
        _elements = new Dictionary<ItemObject, InformationElement>();

        _onObjectAdded += OnObjectAdded;
        _onObjectRemoved += OnObjectRemoved;
    }
    private void OnObjectAdded(ItemObject obj)
    {
        InformationElement element = Instantiate(_elementPrefab);
        element.Initialize(obj);

        element.transform.SetParent(transform);

        _elements.Add(obj, element);        
    }
    private void OnObjectRemoved(ItemObject obj)
    {
        InformationElement element = _elements[obj];
        _elements.Remove(obj);

        Destroy(element.gameObject);
    }
}
