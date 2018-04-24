using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a property that supplies electricity to an item
/// </summary>
public interface IElectricitySupplier {

    float MaxCharge { get; }
    float CurrentCharge { get; set; }
}
