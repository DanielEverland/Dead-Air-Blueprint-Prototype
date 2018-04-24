using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElectricityUser {

    float ElectricityRequired { get; }
    bool IsOn { get; set; }
}
