using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Generator : ElectricalObject, IElectricitySupplier
{
    public override IShape Shape { get { return _shape; } }

    public float MaxCharge { get { return 1000000; } }
    public float CurrentCharge { get; set; }

    private SquareShape _shape;

    private void Awake()
    {
        _shape = new SquareShape(this, transform.localScale);
        CurrentCharge = MaxCharge;
    }
    public override void Start()
    {
        Grid = new ElectricityGrid();
        Grid.Add(this);

        base.Start();
    }
    public override string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("CurrentCharge: ");
        builder.Append(CurrentCharge.ToString("N0"));

        builder.AppendLine();

        builder.Append(base.GetInformationString());

        return builder.ToString();
    }
}
