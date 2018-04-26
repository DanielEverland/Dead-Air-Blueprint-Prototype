using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAction {

    KeyCode ActivationKey { get; }

    void DoUpdate();
    void OnSelected();
    void OnDeselected();
}
