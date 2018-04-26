using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityGrid {

    public ElectricityGrid()
    {
        _suppliers = new List<IElectricitySupplier>();
        _users = new List<IElectricityUser>();

        _gridIndex = _gridCount;
        _gridCount++;

        ElectricityManager.Register(this);
    }

    private static int _gridCount;

    public int ID { get { return _gridIndex; } }

    private readonly int _gridIndex;

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
    public void AddUser(IElectricityUser user)
    {
        _users.Add(user);
    }
    public void RemoveUser(IElectricityUser user)
    {
        _users.Remove(user);
    }
    public void AddSupplier(IElectricitySupplier supplier)
    {
        _suppliers.Add(supplier);
    }
    public void RemoveSupplier(IElectricitySupplier supplier)
    {
        _suppliers.Remove(supplier);
    }
}
