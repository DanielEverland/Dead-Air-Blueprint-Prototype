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
    public ElectricityGrid Grid { get; set; }

    public Vector2 Point { get { return transform.position; } }

    private SquareShape _shape;

    private void Awake()
    {
        _shape = new SquareShape(this, transform.localScale);
        CurrentCharge = MaxCharge;
    }
    private void Start()
    {
        Grid = new ElectricityGrid();
        Grid.Add(this);

        InformationManager.Add(this);
        ElectricityManager.AddObject(this);
    }
    public string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("CurrentCharge: ");
        builder.Append(CurrentCharge.ToString("N0"));

        builder.AppendLine();

        builder.Append(Grid.ToString());

        return builder.ToString();
    }
    public void Remove()
    {
        Grid.Remove(this);
        InformationManager.Remove(this);
        ElectricityManager.RemoveObject(this);
    }
}
