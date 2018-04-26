using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour {

    private static List<ElectricityGrid> _grids;

    private const int UPDATES_PER_SECOND = 10;

    private float UpdateInterval { get { return 1 / (float)UPDATES_PER_SECOND; } }

    private float _time;

    private void Awake()
    {
        _grids = new List<ElectricityGrid>();
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
}
