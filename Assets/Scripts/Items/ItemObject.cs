using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IWorldObject {

    [SerializeField]
    private SpriteRenderer _renderer;

    public const float DEPTH = -1;

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            AssignPosition(transform, value);
        }
    }

    public ItemBase Item { get; private set; }
    public float Radius { get { return 0.5f; } }
    public Vector2 Point { get { return Position; } }

    public static void AssignPosition(Transform transform, Vector3 position)
    {
        position.z = DEPTH;

        transform.position = position;
    }
    public static ItemObject Create(ItemBase item)
    {
        GameObject gameObject = Instantiate(Resources.Load<GameObject>("ItemObject"));
        ItemObject itemObject = gameObject.GetComponent<ItemObject>();

        itemObject.Initialize(item);

        return itemObject;
    }
    private void Start()
    {
        InformationManager.Add(this);
    }
    public void PlaceInWorld()
    {
        WorldObjectContainer.AddItemObject(this);
        Item.RaiseEvent(PropertyEventTypes.OnPlacedInWorld, null);

        WorldItemEventHandler.Add(this);
    }
    public void Initialize(ItemBase item)
    {
        Item = item;

        _renderer.sprite = item.Sprite;

        item.Object = this;
        item.CreateObject(gameObject);
    }
    private void Update()
    {
        Item.Update();
    }
    private void Destroy()
    {
        InformationManager.Remove(this);
        WorldObjectContainer.RemoveItemObject(this);

        Destroy(gameObject);
    }
    public void HandleCollision(IWorldObject obj)
    {
        if(obj is Liquid)
        {
            Liquid liquid = obj as Liquid;

            Item.RaiseEvent(PropertyEventTypes.OnLiquid, liquid);
        }
    }
    public void RaiseEvent(PropertyEventTypes type, params object[] args)
    {
        Item.RaiseEvent(type, args);
    }
}
