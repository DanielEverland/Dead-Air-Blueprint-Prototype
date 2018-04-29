using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldElectricityObject {

    IShape Shape { get; }
    ElectricityGrid Grid { get; set; }
    IEnumerable<IWorldElectricityObject> Connections { get; }

    void AddConnection(IWorldElectricityObject obj);
    void RemoveConnection(IWorldElectricityObject obj);
    bool ContainsConnection(IWorldElectricityObject obj);
}
