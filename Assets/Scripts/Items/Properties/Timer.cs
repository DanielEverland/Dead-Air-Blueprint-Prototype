using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : PropertyBase, IPropertyOutput {

    public Timer(ItemBase owner) : base(owner) { }
    
    public PropertyEventTypes OutputTypes { get { return PropertyEventTypes.OnTrigger; } }

    private const float SECONDS_UNTIL_TRIGGER = 5;

    private float _time;

    public override void Update()
    {
        _time += Time.unscaledDeltaTime;

        if(_time >= SECONDS_UNTIL_TRIGGER)
        {
            RaiseEvent(PropertyEventTypes.OnTrigger, null);

            //Destroy self
            throw new System.NotImplementedException();
        }
    }
}
