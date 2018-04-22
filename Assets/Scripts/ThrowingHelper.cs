using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingHelper : MonoBehaviour {

    public System.Action OnDone;

    private const float MIN_DISTANCE = 0.01f;

    private bool _initialized;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private ItemObject _obj;

    private float _force;

    public void Initialize(ItemObject obj, Vector2 targetPosition, float force = 30)
    {
        _initialized = true;

        _obj = obj;
        _force = force;
        _startPosition = transform.position;
        _targetPosition = targetPosition;

        obj.Item.RaiseEvent(PropertyEventTypes.OnThrowBegins, null);
    }
    private void Update()
    {
        if (_initialized)
        {
            Move();
            PollDone();
        }        
    }
    private void PollDone()
    {
        if(Vector2.Distance(transform.position, _targetPosition) <= MIN_DISTANCE)
        {
            ItemObject.AssignPosition(transform, _targetPosition);
            
            if (OnDone != null)
                OnDone.Invoke();

            _obj.Item.RaiseEvent(PropertyEventTypes.OnThrowEnds, null);

            Destroy(this);
        }
    }
    private void Move()
    {
        Vector2 delta = _targetPosition - (Vector2)transform.position;

        Vector3 direction = delta.normalized;
        float distance = delta.magnitude;
        float forceAdjusted = _force * Time.deltaTime;

        if (distance < forceAdjusted)
        {
            ItemObject.AssignPosition(transform, _targetPosition);
            return;
        }

        Vector3 position = transform.position;

        //Add horizontal velocity
        position += direction * forceAdjusted;

        //Assign parabolic height
        position.z = ItemObject.DEPTH - GetHeight();

        transform.position = position;
    }
    private float GetHeight()
    {
        float fullLength = (_targetPosition - _startPosition).magnitude;
        float currentTraveled = ((Vector2)transform.position - _startPosition).magnitude;

        float percentageDelta = Mathf.Clamp(currentTraveled / fullLength, 0, 1);
        
                                //Shift from 0-1 to -1-1
        return ParabolicFunction(percentageDelta * 2 - 1);
    }

    private const float A = -2;
    private const float B = 0;
    private const float C = 2;

    private float ParabolicFunction(float x)
    {
        return A * (x * x) + B * x + C;
    }
}
