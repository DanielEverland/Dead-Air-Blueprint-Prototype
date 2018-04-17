using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropertyInput : IPropertyIO {
    
    PropertyEventTypes InputTypes { get; }
}
