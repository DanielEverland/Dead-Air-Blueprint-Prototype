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
        GameObject gameObject = Resources.Load<GameObject>("ItemObject");
        ItemObject itemObject = gameObject.GetComponent<ItemObject>();

        itemObject.Initialize(item);

        return itemObject;
    }
    public void Initialize(ItemBase item)
    {
        _renderer.sprite = item.Sprite;

        item.RaiseEvent(PropertyEventTypes.OnPlacedInWorld, null);
    }
    private void Update()
    {
        Item.Update();
    }
}
