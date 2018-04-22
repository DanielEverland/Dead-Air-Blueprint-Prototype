using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer _renderer;

    private const float DEPTH = -1;

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
    }
    public void Initialize(ItemBase item)
    {
        Item = item;

        _renderer.sprite = item.Sprite;
        
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
}
