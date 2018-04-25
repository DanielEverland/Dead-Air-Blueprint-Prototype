using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IWorldElectricitySupplier
{
    public float MaxCharge { get { return 1000000; } }
    public float CurrentCharge { get; set; }

    public Vector2 Point { get { return transform.position; } }

    private void Start()
    {
        ElectricityManager.Add(this);
    }
    public string GetInformationString()
    {
        return "CurrentCharge: " + CurrentCharge.ToString("N0");
    }
    public void Remove()
    {
        ElectricityManager.Remove(this);
    }
}
