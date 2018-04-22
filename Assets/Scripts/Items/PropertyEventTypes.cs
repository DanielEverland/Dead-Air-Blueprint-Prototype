using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyEventTypes {

	None = 0,

    OnItemCreated = 1 << 0,
    OnElectricalInput = 1 << 1,
    OnTrigger = 1 << 2,
    OnPlacedInWorld = 1 << 3,
    OnElectricalInputChanged = 1 << 4,
    OnThrowBegins = 1 << 5,
    OnThrowEnds = 1 << 6,
    OnLiquidContainerBreaks = 1 << 7,
}
