using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectHandler : MonoBehaviour {
    
    public static void HandleItem(ItemObject obj)
    {
        _instance.DoHandleItem(obj);
    }

    [SerializeField]
    private float _radius = 1;

    

    private static ItemObjectHandler _instance;

    private ItemObject _object;
    private ItemBase _previouslyHandledItem;

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        PollInput();

        if (_object == null)
            return;

        AlignObject();
        HandleObject();
    }
    private void DoHandleItem(ItemObject obj)
    {
        if (_object != null)
            PlaceOnGround();

        _object = obj;
        _previouslyHandledItem = obj.Item;
    }
    private void PollInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReplicateOldItem();
        }
    }
    private void HandleObject()
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
    private void ReplicateOldItem()
    {
        if(_previouslyHandledItem != null && _object == null)
        {
            ItemObject obj = ItemObject.Create(_previouslyHandledItem);

            DoHandleItem(obj);
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
        
        _object.Position = transform.position + (Vector3)objPos;
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
