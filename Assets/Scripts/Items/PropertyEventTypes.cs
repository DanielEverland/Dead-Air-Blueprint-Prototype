using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropertyEventTypes {

	None = 0,

    OnItemCreated           = 1 << 0,
    OnTrigger               = 1 << 2,
    OnPlacedInWorld         = 1 << 3,
    OnThrowBegins           = 1 << 5,
    OnThrowEnds             = 1 << 6,
    OnLiquidContainerBreaks = 1 << 7,
    OnIgnite                = 1 << 8,
    OnLiquid                = 1 << 9,
    OnCollisionEnter        = 1 << 10,
    OnCollisionStay         = 1 << 11,
    OnCollisionExit         = 1 << 12,
    OnBlastingCap           = 1 << 13,
}
