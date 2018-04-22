using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : PropertyBase, IPropertyOutput {
    
    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnTrigger; } }

    public override string Name { get { return "Timer"; } }

    private const float SECONDS_UNTIL_TRIGGER = 5;

    private float _time;

    public override void Update()
    {
        _time += Time.unscaledDeltaTime;
        
        if(_time >= SECONDS_UNTIL_TRIGGER)
        {
            Owner.RaiseEvent(PropertyEventTypes.OnTrigger, null);
            Owner.Remove(this);
        }
    }
    public override string[] GetInformation()
    {
        return new string[2]
        {
            "Time: " + _time.ToString("F0"),
            "Trigger Time: " + SECONDS_UNTIL_TRIGGER,
        };
    }
}
