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
    public void Initialize(Vector2 startPos, Vector2 endPos)
    {
        CheckForObject(startPos);
        CheckForObject(endPos);

        Vector2 fixedStartPos = SetPosition(0, startPos, endPos);
        SetPosition(1, endPos, fixedStartPos);
    }
    private Vector2 SetPosition(int index, Vector2 pos, Vector2 other)
    {
        Vector2 fixedPos = ElectricityManager.ResolvePosition(pos, other);
        Renderer.SetPosition(index, fixedPos);

        return fixedPos;
    }
    private void CheckForObject(Vector2 pos)
    {
        IWorldElectricityObject obj;
        if(ElectricityManager.Poll(pos, out obj))
        {
            _connections.Add(obj);
        }
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
