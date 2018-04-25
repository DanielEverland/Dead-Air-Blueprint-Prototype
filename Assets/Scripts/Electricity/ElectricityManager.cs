using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ElectricityManager {

    static ElectricityManager()
    {
        _suppliers = new List<IWorldElectricitySupplier>();
    }

    private static List<IWorldElectricitySupplier> _suppliers;

    public static void Add(IWorldElectricitySupplier supplier)
    {
        supplier.CurrentCharge = supplier.MaxCharge;


        _suppliers.Add(supplier);

        InformationManager.Add(supplier);
    }
    public static void Remove(IWorldElectricitySupplier supplier)
    {
        _suppliers.Remove(supplier);

        InformationManager.Remove(supplier);
    }
}
