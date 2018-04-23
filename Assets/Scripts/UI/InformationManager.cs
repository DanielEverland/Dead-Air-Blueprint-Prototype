using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour {

    static InformationManager()
    {
        _trackedObjects = new List<IWorldObject>();
    }

    [SerializeField]
    private InformationElement _elementPrefab;

    private static System.Action<IWorldObject> _onObjectAdded;
    private static System.Action<IWorldObject> _onObjectRemoved;
    private static List<IWorldObject> _trackedObjects;

    private Dictionary<IWorldObject, InformationElement> _elements;

    public static void Add(IWorldObject obj)
    {
        _trackedObjects.Add(obj);

        if(_onObjectAdded != null)
            _onObjectAdded.Invoke(obj);
    }
    public static void Remove(IWorldObject obj)
    {
        _trackedObjects.Remove(obj);

        if (_onObjectRemoved != null)
            _onObjectRemoved.Invoke(obj);
    }

    private void Awake()
    {
        _elements = new Dictionary<IWorldObject, InformationElement>();

        _onObjectAdded += OnObjectAdded;
        _onObjectRemoved += OnObjectRemoved;
    }
    private void OnObjectAdded(IWorldObject obj)
    {
        InformationElement element = Instantiate(_elementPrefab);
        element.Initialize(obj);

        element.transform.SetParent(transform);

        _elements.Add(obj, element);        
    }
    private void OnObjectRemoved(IWorldObject obj)
    {
        InformationElement element = _elements[obj];
        _elements.Remove(obj);

        Destroy(element.gameObject);
    }
}
