using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldElectricityObject {

    IShape Shape { get; }
    ElectricityGrid Grid { get; set; }
}
