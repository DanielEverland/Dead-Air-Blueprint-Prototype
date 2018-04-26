using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityLine : MonoBehaviour {

    private bool _active;

    public void Toggle(bool active)
    {
        _active = active;
    }
}
