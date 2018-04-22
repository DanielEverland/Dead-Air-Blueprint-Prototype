using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PropertyConflictManager {

    public static Result Resolve(IEnumerable<PropertyBase> properties)
    {
        Result result = new Result();

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
            if(property is IPropertyRequirement)
            {
                IPropertyRequirement requirement = property as IPropertyRequirement;

                string message = string.Empty;
                if (!requirement.IsValid(properties, out message))
                {
                    messages.Add(message);
                }
            }
        }

        return messages;
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
