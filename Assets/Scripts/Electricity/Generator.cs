using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Generator : MonoBehaviour, IWorldElectricitySupplier
{
    public IShape Shape { get { return _shape; } }
    public float MaxCharge { get { return 1000000; } }
    public float CurrentCharge { get; set; }

    public Vector2 Point { get { return transform.position; } }

    private SquareShape _shape;
    private ElectricityGrid _grid;

    private void Awake()
    {
        _shape = new SquareShape(this, transform.localScale);
        CurrentCharge = MaxCharge;
    }
    private void Start()
    {
        _grid = new ElectricityGrid();
        _grid.AddSupplier(this);

        InformationManager.Add(this);
        ElectricityManager.AddObject(this);
    }
    public string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("CurrentCharge: ");
        builder.Append(CurrentCharge.ToString("N0"));

        builder.AppendLine();

        builder.Append("Grid ID: ");
        builder.Append(_grid.ID);

        return builder.ToString();
    }
    public void Remove()
    {
        _grid.RemoveSupplier(this);
        InformationManager.Remove(this);
        ElectricityManager.RemoveObject(this);
    }
}
