using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PropertyConflictManager {

    private static HashSet<System.Type> _exclusiveTypes;

    public static Result Resolve(IEnumerable<PropertyBase> properties)
    {
        Result result = new Result();

        _exclusiveTypes = new HashSet<System.Type>();

        foreach (string message in GetErrorMessages(properties))
        {
            result.AddErrorMessage(message);
        }

        return result;
    }
    private static List<string> GetErrorMessages(IEnumerable<PropertyBase> properties)
    {
        List<string> messages = new List<string>();

        foreach (PropertyBase property in properties)
        {
            PollRequirement(property, properties, messages);
            PollExclusion(property, properties, messages);
        }

        return messages;
    }
    private static void PollExclusion(PropertyBase property, IEnumerable<PropertyBase> properties, List<string> messages)
    {
        if(property is IPropertyExclusive)
        {
            IPropertyExclusive exclusive = property as IPropertyExclusive;

            if (_exclusiveTypes.Contains(exclusive.BlockedType))
                return;
            
            List<string> propertyNames = new List<string>();

            foreach (PropertyBase propertyElement in properties)
            {
                if (exclusive.BlockedType.IsAssignableFrom(propertyElement.GetType()))
                {
                    propertyNames.Add(propertyElement.Name);
                }
            }

            if(propertyNames.Count > 1)
            {
                messages.Add(string.Format("{0} ({1})", exclusive.ErrorMessage, string.Join(", ", propertyNames.ToArray())));
            }

            _exclusiveTypes.Add(exclusive.BlockedType);
        }
    }
    private static void PollRequirement(PropertyBase property, IEnumerable<PropertyBase> properties, List<string> messsages)
    {
        if (property is IPropertyRequirement)
        {
            IPropertyRequirement requirement = property as IPropertyRequirement;

            string message = string.Empty;
            if (!requirement.IsValid(properties, out message))
            {
                messsages.Add(message);
            }
        }
    }

    public class Result
    {
        public Result()
        {
            Succeeded = true;
        }

        public bool Succeeded { get; private set; }
        public IEnumerable<string> Messages { get { return _messages; } }

        private List<string> _messages = new List<string>();

        public void AddErrorMessage(string message)
        {
            Succeeded = false;

            _messages.Add(message);
        }
    }
}
