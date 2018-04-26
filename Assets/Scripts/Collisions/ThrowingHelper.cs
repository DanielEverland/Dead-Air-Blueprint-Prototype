using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ThrowingHelper {

    private const float FORCE_COEFFICIENT = 3.5f;
    private const float ANGLE = 25;

    public static void ThrowObject(GameObject obj)
    {
        Rigidbody rigidBody = GetRigidbody(obj);
        Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float force = GetForce(mouseInWorld, obj);

        Vector3 direction = (mouseInWorld - (Vector2)obj.transform.position).normalized;
        Vector3 rotationalAxis = Vector3.Cross(direction, Vector3.back);
        direction = Quaternion.AngleAxis(ANGLE, rotationalAxis) * direction;
        
        rigidBody.AddForce(direction * force, ForceMode.Impulse);

        obj.AddComponent<ThrowingOnCollisionHelper>();
    }
    private static float GetForce(Vector2 mouseInWorld, GameObject obj)
    {
        float distance = Vector2.Distance(mouseInWorld, obj.transform.position);

        return Mathf.Sqrt(distance) * FORCE_COEFFICIENT;
    }
    private static Rigidbody GetRigidbody(GameObject obj)
    {
        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        
        if(rigidbody == null)
        {
            Debug.LogException(new System.InvalidOperationException("Cannot throw " + obj + " since it doesn't have a rigidbody"), obj);
        }

        return rigidbody;
    }
}
