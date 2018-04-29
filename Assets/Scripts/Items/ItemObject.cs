using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ItemObject : ElectricalObject, IWorldObject {

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

    public override IShape Shape { get { return _shape; } }

    public ItemBase Item { get; private set; }
    public float Radius { get { return 0.5f; } }

    private CircleShape _shape;

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
    private void Awake()
    {
        _shape = new CircleShape(this);
    }
    public void PlaceInWorld(ElectricityGrid grid)
    {
        WorldObjectContainer.AddItemObject(this);
        Item.RaiseEvent(PropertyEventTypes.OnPlacedInWorld, null);
        
        Grid = grid;

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
    public override void Remove()
    {
        WorldObjectContainer.RemoveItemObject(this);
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
    public override string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        GetProperties(builder);

        builder.AppendLine();

        builder.Append(base.GetInformationString());

        return builder.ToString();
    }
    private void GetProperties(StringBuilder builder)
    {
        if (Item.Properties.Count() == 0)
        {
            builder.Append("NO PROPERTIES");
        }
        else
        {
            foreach (PropertyBase property in Item.Properties)
            {
                builder.Append(property.GetType().Name);
                builder.Append(':');
                Indent(builder);

                string[] propertyInfo = property.GetInformation();

                if (propertyInfo.Length != 0)
                {
                    builder.Append(string.Join(",    ", propertyInfo));
                }
                else
                {
                    builder.Append("NONE");
                }

                builder.AppendLine();
            }
        }
    }
    private void Indent(StringBuilder builder)
    {
        builder.Append("    ");
    }
}
