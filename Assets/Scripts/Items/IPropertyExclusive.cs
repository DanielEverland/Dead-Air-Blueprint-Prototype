using System;

/// <summary>
/// Types with this interface can only contain
/// one instance of them at a time
/// 
/// Important for types likes bottles. You can't
/// have a Glass Bottle and a Plastic Bottle in
/// the same item
/// </summary>
public interface IPropertyExclusive {

    Type BlockedType { get; }
    string ErrorMessage { get; }
}
