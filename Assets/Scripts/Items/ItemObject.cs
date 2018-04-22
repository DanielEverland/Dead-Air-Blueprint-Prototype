using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer _renderer;

    public ItemBase Item { get; private set; }

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
