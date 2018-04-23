using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject {

    Vector2 Point { get; }
    float Radius { get; }

    void HandleCollision(IWorldObject obj);
    void RaiseEvent(PropertyEventTypes type, params object[] args);
}
