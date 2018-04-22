using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemBase {

    /// <summary>
    /// Declares an item with properties
    /// </summary>
    public ItemBase(Sprite sprite, params PropertyBase[] properties)
    {
        Sprite = sprite;

        _properties = new PropertyCollection();

        if(properties != null)
        {
            foreach (PropertyBase property in properties)
            {
                property.AssignOwner(this);

                _properties.RegisterProperty(property);
            }
        }        

        RaiseEvent(PropertyEventTypes.OnItemCreated, null);

        if (!_properties.ContainsOutput(PropertyEventTypes.OnTrigger))
            RaiseEvent(PropertyEventTypes.OnTrigger, null);
    }

    public ItemObject Object { get; set; }

    /// <summary>
    /// Types that can only be received as input. They can never be sent as output
    /// </summary>
    public static IEnumerable<PropertyEventTypes> BlockedOutputTypes { get { return _outputEventBlockers; } }

    /// <summary>
    /// Types that can only be sent as output. They can never be received as input
    /// </summary>
    public static IEnumerable<PropertyEventTypes> BlockedInputTypes { get { return _inputEventBlockers.Keys; } }

    private static HashSet<PropertyEventTypes> _outputEventBlockers = new HashSet<PropertyEventTypes>()
    {
        PropertyEventTypes.OnElectricalInputChanged,
    };
    private static Dictionary<PropertyEventTypes, System.Action<ItemBase>> _inputEventBlockers = new Dictionary<PropertyEventTypes, System.Action<ItemBase>>()
    {
        { PropertyEventTypes.OnElectricalInput, ReceiveElectricity },
    };

    public Sprite Sprite { get; private set; }
    
    /// <summary>
    /// Defines whether or not the object has been placed
    /// </summary>
    public bool PlacedInWorld { get; private set; }

    /// <summary>
    /// Defines whether the item receives electricity
    /// </summary>
    public bool IsElectricallyCharged { get; private set; }

    public bool _receivesElectricity;

    /// <summary>
    /// Combines items
    /// </summary>
    public ItemBase(params ItemBase[] items)
    {
        _properties = new PropertyCollection();

        Merge(items);
    }

    public IEnumerable<PropertyBase> Properties { get { return _properties; } }

    private PropertyCollection _properties;
    
    public void Remove(PropertyBase property)
    {
        _properties.Remove(property);
    }
    public void Update()
    {
        PollElectricity();
        
        _properties.Update();
    }
    private void PollElectricity()
    {
        if(!IsElectricallyCharged && _receivesElectricity)
        {
            IsElectricallyCharged = true;
            DoRaiseEvent(PropertyEventTypes.OnElectricalInputChanged, null);
        }
        else if(IsElectricallyCharged && !_receivesElectricity)
        {
            IsElectricallyCharged = false;
            DoRaiseEvent(PropertyEventTypes.OnElectricalInputChanged, null);
        }

        _receivesElectricity = false;
    }
    public bool ContainsOutput(PropertyEventTypes type)
    {
        return _properties.ContainsOutput(type);
    }
    public bool ContainsInput(PropertyEventTypes type)
    {
        return _properties.ContainsInput(type);
    }
    public void RaiseEvent(PropertyEventTypes type, params object[] parameters)
    {
        if (_outputEventBlockers.Contains(type))
            throw new System.InvalidOperationException("Tried to raise blocked output event!");

        DoRaiseEvent(type, parameters);
    }
    private void DoRaiseEvent(PropertyEventTypes type, params object[] parameters)
    {
        if (_inputEventBlockers.ContainsKey(type))
        {
            System.Action<ItemBase> action = _inputEventBlockers[type];

            if (action != null)
                action.Invoke(this);

            return;
        }

        if (type == PropertyEventTypes.OnPlacedInWorld)
            PlacedInWorld = true;
        
        _properties.RaiseEvent(type, parameters);
    }
    public GameObject CreateObject(GameObject obj)
    {
        foreach (PropertyBase property in _properties)
        {
            property.CreateInstance(obj);
        }

        return obj;
    }
    private static void ReceiveElectricity(ItemBase item)
    {
        item._receivesElectricity = true;
    }

    public ItemBase CreateClone()
    {
        List<PropertyBase> properties = new List<PropertyBase>();

        foreach (PropertyBase property in _properties)
        {
            properties.Add((PropertyBase)System.Activator.CreateInstance(property.GetType()));
        }

        return new ItemBase(Sprite, properties.ToArray());
    }

    /// <summary>
    /// Copies the properties of <paramref name="items"/> into the current instance
    /// </summary>
    /// <param name="items"></param>
    private void Merge(IEnumerable<ItemBase> items)
    {
        foreach (ItemBase item in items)
        {
            _properties.RegisterProperties(item.Properties);
        }
    }
}
