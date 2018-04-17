using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PropertyCollection : IEnumerable<PropertyBase> {

    public PropertyCollection()
    {
        _properties = new List<PropertyBase>();
        _allOutput = new Dictionary<PropertyEventTypes, List<IPropertyOutput>>();
        _allInput = new Dictionary<PropertyEventTypes, List<InputDefinition>>();
    }
    
    private Dictionary<PropertyEventTypes, List<IPropertyOutput>> _allOutput;
    private Dictionary<PropertyEventTypes, List<InputDefinition>> _allInput;
    private List<PropertyBase> _properties;

    private static BindingFlags _memberBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public void Remove(PropertyBase property)
    {
        if (!_properties.Contains(property))
            throw new System.ArgumentException("Tried to remove property that doesn't exist");

        _properties.Remove(property);

        if (property is IPropertyInput)
        {
            IPropertyInput input = property as IPropertyInput;

            TraverseTypes(input.InputTypes, x =>
            {
                foreach (InputDefinition definition in _allInput[x].Where(y => y.Instance == input))
                {
                    _allInput[x].Remove(definition);
                }
            });
        }
        if (property is IPropertyOutput)
        {
            IPropertyOutput output = property as IPropertyOutput;

            TraverseTypes(output.OutputTypes, x =>
            {
                _allOutput[x].Remove(output);
            });
        }
    }
    public void Update()
    {
        foreach (PropertyBase property in _properties)
        {
            property.Update();
        }
    }
    public bool ContainsOutput(PropertyEventTypes type)
    {
        return _allOutput.ContainsKey(type);
    }
    public bool ContainsInput(PropertyEventTypes type)
    {
        return _allInput.ContainsKey(type);
    }
    public void RaiseEvent(PropertyEventTypes type, params object[] parameters)
    {
        Debug.Log("Raise event " + type);

        if (!_allInput.ContainsKey(type))
            return;
        
        foreach (InputDefinition input in _allInput[type])
        {
            input.Method.Invoke(input.Instance, parameters);
        }
    }
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

            TraverseTypes(input.InputTypes, x => RegisterInput(x, input));
        }
        if(property is IPropertyOutput)
        {
            IPropertyOutput output = property as IPropertyOutput;

            TraverseTypes(output.OutputTypes, x => RegisterOutput(x, output));
        }
    }
    private void TraverseTypes(PropertyEventTypes types, System.Action<PropertyEventTypes> predicate)
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
    private void RegisterOutput(PropertyEventTypes type, IPropertyOutput output)
    {
        if (!_allOutput.ContainsKey(type))
            _allOutput.Add(type, new List<IPropertyOutput>());
        
        _allOutput[type].Add(output);
    }
    private void RegisterInput(PropertyEventTypes type, IPropertyInput input)
    {
        if (!_allInput.ContainsKey(type))
            _allInput.Add(type, new List<InputDefinition>());

        MethodInfo method = GetMethod(type, input);
        InputDefinition definition = new InputDefinition(input, method);

        _allInput[type].Add(definition);
    }
    private MethodInfo GetMethod(PropertyEventTypes type, IPropertyIO instance)
    {
        string methodName = GetMethodName(type);
        MethodInfo method = instance.GetType().GetMethod(methodName, _memberBindingFlags);

        if (method == null)
            throw new System.NotImplementedException("Missing method declaration " + GetFullMethodName(type) + " on " + instance.GetType());

        if(!MatchesParameters(type, method))
            throw new System.ArgumentException("Parameter types do not match for " + GetFullMethodName(type) + " on " + instance.GetType());

        return method;
    }
    private string GetMethodName(PropertyEventTypes type)
    {
        return type.ToString();
    }
    private bool MatchesParameters(PropertyEventTypes type, MethodInfo method)
    {
        System.Type[] parameterTypes = MethodDefinitions.GetParameterInfo(type);
        ParameterInfo[] methodParameters = method.GetParameters();

        if (parameterTypes == null && methodParameters.Length == 0)
            return true;

        for (int i = 0; i < parameterTypes.Length; i++)
        {
            //Index mismatch
            if (methodParameters.Length - 1 < i)
                return false;

            if (parameterTypes[i] != methodParameters[i].ParameterType)
                return false;
        }

        return true;
    }
    private string GetFullMethodName(PropertyEventTypes type)
    {
        return string.Format("{0}{1}", GetMethodName(type), GetParameterString(type));
    }
    private string GetParameterString(PropertyEventTypes type)
    {
        System.Type[] types = MethodDefinitions.GetParameterInfo(type);

        if (types == null)
            return "()";

        return string.Format("({0})", string.Join(", ", types.Select(x => x.Name).ToArray()));
    }
    private class InputDefinition
    {
        private InputDefinition() { }
        public InputDefinition(IPropertyInput instance, MethodInfo method)
        {
            Instance = instance;
            Method = method;
        }

        public IPropertyInput Instance;
        public MethodInfo Method;
    }

    public IEnumerator<PropertyBase> GetEnumerator()
    {
        return _properties.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
