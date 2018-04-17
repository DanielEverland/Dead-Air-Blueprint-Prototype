using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPropertyOutput : IPropertyIO {

    PropertyEventTypes OutputTypes { get; }
}
public delegate void OutputDelegate(params object[] parameters);
