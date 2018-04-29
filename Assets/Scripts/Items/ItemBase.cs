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
    public Sprite Sprite { get; private set; }
    public float RequiredElectricity { get { return GetRequiredCharge(); } }
    
    /// <summary>
    /// Defines whether or not the object has been placed
    /// </summary>
    public bool PlacedInWorld { get; private set; }
    
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
        if (Object.IsReceivingElectricity)
        {
            foreach (IElectricityUser user in _properties.ElectricityUsers)
            {
                if (!user.IsReceivingElectricity)
                    user.IsReceivingElectricity = true;
            }
        }
        else
        {
            UseInternalCharge();
        }        
    }
    private void UseInternalCharge()
    {
        float totalChargeAvailable = GetAvailableElectricity();

        foreach (IElectricityUser user in _properties.ElectricityUsers)
        {
            float required = user.ElectricityRequired * Time.deltaTime;

            if (totalChargeAvailable >= required)
            {
                Charge(user, required);

                totalChargeAvailable -= required;

                if (!user.IsReceivingElectricity)
                {
                    user.IsReceivingElectricity = true;
                }
            }
            else if (user.IsReceivingElectricity)
            {
                user.IsReceivingElectricity = false;
            }
        }
    }
    private float GetRequiredCharge()
    {
        float requiredCharge = 0;

        foreach (IElectricityUser user in _properties.ElectricityUsers)
        {
            requiredCharge += user.ElectricityRequired;
        }

        return requiredCharge;
    }
    private void Charge(IElectricityUser user, float required)
    {
        foreach (IElectricitySupplier supplier in _properties.ElectricitySuppliers)
        {
            if(supplier.CurrentCharge >= required)
            {
                supplier.CurrentCharge -= required;
                required = 0;
            }
            else
            {
                required -= supplier.CurrentCharge;
                supplier.CurrentCharge = 0;
            }

            if (required == 0)
                return;
        }

        if (required != 0)
            throw new System.InvalidOperationException("Something's gone wrong, we weren't able to charge " + user + " fully");
    }
    private float GetAvailableElectricity()
    {
        float total = 0;

        foreach (IElectricitySupplier supplier in _properties.ElectricitySuppliers)
        {
            total += supplier.CurrentCharge;
        }

        return total;
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
