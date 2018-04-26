using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ElectricityLine : MonoBehaviour {
    
    public LineRenderer Renderer;

    private List<IWorldElectricityObject> _connections;

    private void Awake()
    {
        _connections = new List<IWorldElectricityObject>();

        SanitizeComponent();
    }
    private void SanitizeComponent()
    {
        Renderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
    }
    private void OnValidate()
    {
        Renderer = GetComponent<LineRenderer>();
    }
}
