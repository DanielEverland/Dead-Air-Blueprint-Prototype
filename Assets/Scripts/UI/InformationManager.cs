using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationManager : MonoBehaviour {

    static InformationManager()
    {
        _trackedObjects = new List<IInformationObject>();
    }

    [SerializeField]
    private InformationElement _elementPrefab;

    private static System.Action<IInformationObject> _onObjectAdded;
    private static System.Action<IInformationObject> _onObjectRemoved;
    private static List<IInformationObject> _trackedObjects;

    private Dictionary<IInformationObject, InformationElement> _elements;

    public static void Add(IInformationObject obj)
    {
        _trackedObjects.Add(obj);

        if(_onObjectAdded != null)
            _onObjectAdded.Invoke(obj);
    }
    public static void Remove(IInformationObject obj)
    {
        _trackedObjects.Remove(obj);

        if (_onObjectRemoved != null)
            _onObjectRemoved.Invoke(obj);
    }

    private void Awake()
    {
        _elements = new Dictionary<IInformationObject, InformationElement>();

        _onObjectAdded += OnObjectAdded;
        _onObjectRemoved += OnObjectRemoved;
    }
    private void OnObjectAdded(IInformationObject obj)
    {
        InformationElement element = Instantiate(_elementPrefab);
        element.Initialize(obj);

        element.transform.SetParent(transform);

        _elements.Add(obj, element);        
    }
    private void OnObjectRemoved(IInformationObject obj)
    {
        InformationElement element = _elements[obj];
        _elements.Remove(obj);

        Destroy(element.gameObject);
    }
}
