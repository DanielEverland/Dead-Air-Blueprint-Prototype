using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour {

    [SerializeField]
    private ElectricityLine _linePrefab;

    public static ElectricityLine LinePrefab { get; private set; }

    private static List<ElectricityGrid> _grids;
    private static List<IWorldElectricityObject> _objects;

    private const int UPDATES_PER_SECOND = 10;

    private float UpdateInterval { get { return 1 / (float)UPDATES_PER_SECOND; } }

    private float _time;

    private void Awake()
    {
        LinePrefab = _linePrefab;

        _grids = new List<ElectricityGrid>();
        _objects = new List<IWorldElectricityObject>();
    }
    private void Update()
    {
        _time += Time.deltaTime;

        if(_time >= UpdateInterval)
        {
            UpdateGrids();

            _time = 0;
        }
    }
    private void UpdateGrids()
    {
        foreach (ElectricityGrid grid in _grids)
        {
            grid.Update();
        }
    }
    public static void Register(ElectricityGrid grid)
    {
        _grids.Add(grid);
    }
    public static bool Poll(Vector2 position, out IWorldElectricityObject hitObject)
    {
        foreach (IWorldElectricityObject obj in _objects)
        {
            if(obj.Shape.Contains(position))
            {
                hitObject = obj;
                return true;
            }
        }

        hitObject = null;
        return false;
    }
    public static void AddObject(IWorldElectricityObject obj)
    {
        if (!_objects.Contains(obj))
            _objects.Add(obj);
    }
    public static void RemoveObject(IWorldElectricityObject obj)
    {
        if (_objects.Contains(obj))
            _objects.Remove(obj);
    }
    public static void ResolvePositions(Vector2 a, Vector2 b, out Vector2 fixedA, out Vector2 fixedB)
    {
        fixedA = ResolvePosition(a, b);
        fixedB = ResolvePosition(b, a);
    }
    public static Vector2 ResolvePosition(Vector2 a, Vector2 b)
    {
        IWorldElectricityObject obj;
        if(Poll(a, out obj))
        {
            return obj.Shape.GetEdge(b);
        }
        else
        {
            return a;
        }
    }
}
