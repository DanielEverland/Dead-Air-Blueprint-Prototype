using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectHandler : MonoBehaviour {
    
    public static void HandleItem(ItemObject obj)
    {
        if (_instance._object != null)
            _instance.PlaceOnGround();

        _instance._object = obj;
    }

    [SerializeField]
    private float _radius = 1;

    private static ItemObjectHandler _instance;

    private ItemObject _object;

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        if (_object == null)
            return;

        AlignObject();
        PollInput();
    }
    private void PollInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaceOnGround();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ThrowObject();
        }

    }
    private void PlaceOnGround()
    {
        _object.PlaceInWorld();
        _object = null;
    }
    private void ThrowObject()
    {
        ThrowingHelper helper = _object.gameObject.AddComponent<ThrowingHelper>();
        helper.Initialize(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        helper.OnDone += () => { helper.GetComponent<ItemObject>().PlaceInWorld(); };
        
        _object = null;
    }
    private void AlignObject()
    {
        float radians = GetRadians();
        Vector2 vector = GetLocalVector(radians);

        Vector2 objPos = vector * _radius;

        _object.transform.position = transform.position + (Vector3)objPos;
    }
    private Vector2 GetLocalVector(float radians)
    {
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
    private float GetRadians()
    {
        Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        
        return Mathf.Atan2(mouseInWorld.y - playerPos.y, mouseInWorld.x - playerPos.x);
    }
}
