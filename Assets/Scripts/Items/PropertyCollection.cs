using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PropertyCollection : IEnumerable<PropertyBase> {

    public PropertyCollection()
    {
        _properties = new List<PropertyBase>();
        _allInput = new Dictionary<PropertyEventTypes, List<IPropertyInput>>();
        _allOutput = new Dictionary<PropertyEventTypes, List<IPropertyOutput>>();
    }
    
    private Dictionary<PropertyEventTypes, List<IPropertyOutput>> _allOutput;
    private Dictionary<PropertyEventTypes, List<IPropertyInput>> _allInput;
    private List<PropertyBase> _properties;

    public void RegisterProperties(IEnumerable<PropertyBase> properties)
    {
        foreach (PropertyBase property in properties)
        {
            RegisterProperty(property);
        }
    }
    public void RegisterProperty(PropertyBase property)
    {
        if(property is IPropertyInput)
        {
            IPropertyInput input = property as IPropertyInput;

            RegisterEvents(input.InputTypes, x => RegisterInput(x, input));
        }
        if(property is IPropertyOutput)
        {
            IPropertyOutput output = property as IPropertyOutput;

            RegisterEvents(output.OutputTypes, x => RegisterOutput(x, output));
        }
    }
    private void RegisterEvents(PropertyEventTypes types, System.Action<PropertyEventTypes> predicate)
    {
        System.Array array = System.Enum.GetValues(typeof(PropertyEventTypes));

        foreach (int value in array)
        {
            //We're not interested in PropertyEventTypes.None
            if (value == 0)
                continue;

            if (((int)types & value) == value)
            {
                PropertyEventTypes type = (PropertyEventTypes)value;

                predicate.Invoke(type);
            }
        }
    }
    private void RegisterOutput(PropertyEventTypes type, IPropertyOutput input)
    {
        if (!_allOutput.ContainsKey(type))
            _allOutput.Add(type, new List<IPropertyOutput>());

        _allOutput[type].Add(input);
    }
    private void RegisterInput(PropertyEventTypes type, IPropertyInput input)
    {
        if (!_allInput.ContainsKey(type))
            _allInput.Add(type, new List<IPropertyInput>());
        
        _allInput[type].Add(input);
    }

    public IEnumerator<PropertyBase> GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _properties.GetEnumerator();
    }
}
