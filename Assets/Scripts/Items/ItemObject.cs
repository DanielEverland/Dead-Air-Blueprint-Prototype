using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ItemObject : MonoBehaviour, IWorldObject, IWorldElectricityObject {

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
    public IShape Shape { get { return _shape; } }
    public ElectricityGrid Grid { get; set; }

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
    private void Start()
    {
        InformationManager.Add(this);
    }
    public void PlaceInWorld(ElectricityGrid grid)
    {
        WorldObjectContainer.AddItemObject(this);
        Item.RaiseEvent(PropertyEventTypes.OnPlacedInWorld, null);
        
        Grid = grid;

        WorldItemEventHandler.Add(this);
        ElectricityManager.AddObject(this);
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
        ElectricityManager.RemoveObject(this);

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
    public string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        GetProperties(builder);

        return builder.ToString();
    }
    private void GetProperties(StringBuilder builder)
    {
        builder.Append("Grid");
        if(Grid == null)
        {
            builder.Append(": NONE");
        }
        else
        {
            builder.Append(Grid.ToString());
        }
        builder.AppendLine();

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
