using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemProperty {

    /// <summary>
    /// Callback allowing a property to add/get components
    /// on the object it's been added to
    /// </summary>
    void CreateInstance(GameObject obj);
}
