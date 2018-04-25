using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInformationObject {

    Vector2 Point { get; }
    string GetInformationString();
}
