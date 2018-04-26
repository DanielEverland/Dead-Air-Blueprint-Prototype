using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingOnCollisionHelper : MonoBehaviour {

    private ItemObject _itemObject;

    private void Awake()
    {
        _itemObject = GetComponent<ItemObject>();

        if (_itemObject != null)
            _itemObject.RaiseEvent(PropertyEventTypes.OnThrowBegins, null);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_itemObject != null)
            _itemObject.RaiseEvent(PropertyEventTypes.OnThrowEnds, null);
    }
}
