using System.Collections.Generic;

/// <summary>
/// Used to define dependencies with properties.
/// 
/// For instance, a liquid property, such as Alcohol or Water,
/// would require a liquid container property
/// </summary>
public interface IPropertyRequirement {

    /// <summary>
    /// Determines whether a collection of properties contains all required types 
    /// </summary>
    /// <returns>True if <paramref name="properties"/> is valid</returns>
    bool IsValid(IEnumerable<PropertyBase> properties, ref string errorMessage);
}
