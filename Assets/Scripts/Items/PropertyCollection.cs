using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PropertyCollection : IEnumerable<PropertyBase> {

    public PropertyCollection()
    {
        _properties = new List<PropertyBase>();
        _allInput = new Dictionary<PropertyEventTypes, List<IODefintion<IPropertyInput>>>();
        _allOutput = new Dictionary<PropertyEventTypes, List<IODefintion<IPropertyOutput>>>();
    }
    
    private Dictionary<PropertyEventTypes, List<IODefintion<IPropertyOutput>>> _allOutput;
    private Dictionary<PropertyEventTypes, List<IODefintion<IPropertyInput>>> _allInput;
    private List<PropertyBase> _properties;

    private static BindingFlags _methodBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

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
        if (!_allInput.ContainsKey(type))
            return;

        foreach (IODefintion<IPropertyInput> input in _allInput[type])
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
    private void RegisterOutput(PropertyEventTypes type, IPropertyOutput output)
    {
        if (!_allOutput.ContainsKey(type))
            _allOutput.Add(type, new List<IODefintion<IPropertyOutput>>());

        MethodInfo method = GetMethod(type, output);
        IODefintion<IPropertyOutput> definition = new IODefintion<IPropertyOutput>(output, method);

        _allOutput[type].Add(definition);
    }
    private void RegisterInput(PropertyEventTypes type, IPropertyInput input)
    {
        if (!_allInput.ContainsKey(type))
            _allInput.Add(type, new List<IODefintion<IPropertyInput>>());

        MethodInfo method = GetMethod(type, input);
        IODefintion<IPropertyInput> definition = new IODefintion<IPropertyInput>(input, method);

        _allInput[type].Add(definition);
    }
    private MethodInfo GetMethod(PropertyEventTypes type, IPropertyIO instance)
    {
        string methodName = type.ToString();
        MethodInfo method = instance.GetType().GetMethod(methodName, _methodBindingFlags);

        if (method == null)
            throw new System.NotImplementedException("Missing method declaration " + GetFullMethodName(type) + " on " + instance.GetType());

        if(!MatchesParameters(type, method))
            throw new System.ArgumentException("Parameter types do not match for " + GetFullMethodName(type) + " on " + instance.GetType());

        return method;
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
        return string.Format("{0}{1}", type.ToString(), GetParameterString(type));
    }
    private string GetParameterString(PropertyEventTypes type)
    {
        System.Type[] types = MethodDefinitions.GetParameterInfo(type);

        if (types == null)
            return "()";

        return string.Format("({0})", string.Join(", ", types.Select(x => x.Name).ToArray()));
    }

    private class IODefintion<T> where T : IPropertyIO
    {
        private IODefintion() { }
        public IODefintion(T instance, MethodInfo method)
        {
            Instance = instance;
            Method = method;
        }

        public T Instance;
        public MethodInfo Method;
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
