using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ElectricityGrid {

    public ElectricityGrid()
    {
        _suppliers = new List<IElectricitySupplier>();
        _users = new List<IElectricityUser>();
        _objects = new List<IWorldElectricityObject>();

        _gridIndex = _gridCount;
        _gridCount++;

        ElectricityManager.Register(this);
    }

    private static int _gridCount;

    public int ID { get { return _gridIndex; } }
    public int Count { get { return _objects.Count; } }

    private readonly int _gridIndex;

    private List<IWorldElectricityObject> _objects;
    private List<IElectricitySupplier> _suppliers;
    private List<IElectricityUser> _users;

    /// <summary>
    /// Only used to check whether we can charge objects.
    /// </summary>
    private float _availableCharge;
    private float _chargeUsedThisFrame;

    public void Update()
    {
        PollCharge();
        CallUsers();
        DrainSuppliers();
    }
    private void DrainSuppliers()
    {
        float chargeLeft = _chargeUsedThisFrame;
        float chargePerSupplier = chargeLeft / _suppliers.Count;

        for (int i = 0; i < _suppliers.Count; i++)
        {
            IElectricitySupplier supplier = _suppliers[i];

            if(supplier.CurrentCharge >= chargePerSupplier)
            {
                supplier.CurrentCharge -= chargePerSupplier;
                chargeLeft -= chargePerSupplier;
            }
            else
            {
                float excessCharge = chargePerSupplier - supplier.CurrentCharge;

                chargeLeft -= supplier.CurrentCharge;
                supplier.CurrentCharge = 0;

                int remainingSuppliers = _suppliers.Count - (i + 1);
                float extraChargePerSupplier = excessCharge / (float)remainingSuppliers;

                chargePerSupplier += extraChargePerSupplier;
            }
        }

        if (chargeLeft != 0)
            throw new System.Exception("Excess charge in grid");
    }
    private void PollCharge()
    {
        _availableCharge = 0;

        foreach (IElectricitySupplier supplier in _suppliers)
        {
            _availableCharge += supplier.CurrentCharge;
        }
    }
    private void CallUsers()
    {
        foreach (IElectricityUser user in _users)
        {
            if (CanCharge(user.ElectricityRequired))
            {
                user.IsOn = true;

                Drain(user.ElectricityRequired);
            }
            else
            {
                user.IsOn = false;
            }
        }
    }
    private void Drain(float charge)
    {
        if (!CanCharge(charge))
            throw new System.InvalidOperationException("Tried to drain a grid without sufficient energy");

        _chargeUsedThisFrame += charge;
        _availableCharge -= charge;
    }
    public bool CanCharge(float requiredCharge)
    {
        return _availableCharge >= requiredCharge;
    }
    public void Add(IWorldElectricityObject obj)
    {
        AddToList(_objects, obj);
        AddToList(_suppliers, obj);
        AddToList(_users, obj);

        obj.Grid = this;
    }
    public void Remove(IWorldElectricityObject obj)
    {
        RemoveFromList(_objects, obj);
        RemoveFromList(_suppliers, obj);
        RemoveFromList(_users, obj);
    }
    private void AddToList<T>(List<T> list, object obj)
    {
        if (obj is T)
        {
            T castObject = (T)obj;

            if (!list.Contains(castObject))
            {
                list.Add(castObject);
            }
        }
    }
    private void RemoveFromList<T>(List<T> list, object obj)
    {
        if(obj is T)
        {
            T castObject = (T)obj;

            if (list.Contains(castObject))
            {
                list.Remove(castObject);
            }
        }
    }
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("ID: ");
        builder.Append(_gridIndex);

        builder.AppendLine();

        builder.Append("Objects: ");
        builder.Append(_objects.Count);

        builder.AppendLine();

        builder.Append("Suppliers: ");
        builder.Append(_suppliers.Count);

        builder.AppendLine();

        builder.Append("Users: ");
        builder.Append(_users.Count);

        return builder.ToString();
    }

    public static ElectricityGrid Merge(params ElectricityGrid[] grids)
    {
        if (grids.Length == 0)
            return null;

        ElectricityGrid largestGrid = GetLargestGrid(grids);

        foreach (ElectricityGrid smallerGrid in grids)
        {
            if (smallerGrid == largestGrid)
                continue;

            for (int i = smallerGrid._objects.Count - 1; i >= 0; i--)
            {
                IWorldElectricityObject obj = smallerGrid._objects[i];

                largestGrid.Add(obj);
                smallerGrid.Remove(obj);
            }
        }

        return largestGrid;
    }
    private static ElectricityGrid GetLargestGrid(params ElectricityGrid[] grids)
    {
        int maxCount = grids.Max(x => x.Count);

        return grids.FirstOrDefault(x => x.Count == maxCount);
    }
}
