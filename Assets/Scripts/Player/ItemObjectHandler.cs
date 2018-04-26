using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectHandler : MonoBehaviour, IPlayerAction {
    
    public static void HandleItem(ItemObject obj)
    {
        _instance.DoHandleItem(obj);
    }

    [SerializeField]
    private float _radius = 1;

    private const float MAX_DISTANCE_FOR_PICKUP = 2;

    private static ItemObjectHandler _instance;

    private ItemObject _object;
    private Rigidbody _objectRigidbody;
    private ItemBase _previouslyHandledItem;

    public KeyCode ActivationKey { get { return KeyCode.None; } }

    private void Awake()
    {
        _instance = this;

        HandleItem(ItemObject.Create(new ItemBase(null, new Battery(), new LightProperty())));
    }
    public void DoUpdate()
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
        _objectRigidbody = obj.GetComponent<Rigidbody>();
        _previouslyHandledItem = obj.Item.CreateClone();

        ToggleRigidbody(false);
    }
    private void ToggleRigidbody(bool active)
    {
        if (_objectRigidbody == null)
            return;

        _objectRigidbody.isKinematic = !active;
        _objectRigidbody.detectCollisions = active;
    }
    private void PollInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReplicateOldItem();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            CheckForPickup();
        }
    }
    private void CheckForPickup()
    {
        Vector2 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<IWorldObject> objectsAtMouse = new List<IWorldObject>(WorldItemEventHandler.GetCollidingObjects(mouseInWorld, 2));

        foreach (IWorldObject obj in objectsAtMouse)
        {
            if (Vector2.Distance(transform.position, obj.Point) > MAX_DISTANCE_FOR_PICKUP)
                continue;

            if(obj is ItemObject)
            {
                DoHandleItem(obj as ItemObject);
                WorldItemEventHandler.Remove(obj);
            }
        }
    }
    private void HandleObject()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _object.Item.RaiseEvent(PropertyEventTypes.OnIgnite, null);
        }

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
        Release();
    }
    private void ThrowObject()
    {
        ToggleRigidbody(true);
        ThrowingHelper.ThrowObject(_object.gameObject);

        Release();
    }
    private void Release()
    {
        ToggleRigidbody(true);

        if (_object == null)
            return;

        _object.PlaceInWorld();
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
    public void OnSelected()
    {
    }
    public void OnDeselected()
    {
        PlaceOnGround();
    }
}
