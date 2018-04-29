using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public abstract class ElectricalObject : MonoBehaviour, IWorldElectricityObject, IInformationObject
{
    public abstract IShape Shape { get; }

    public virtual Vector2 Point { get { return transform.position; } }
    public virtual IEnumerable<IWorldElectricityObject> Connections { get { return _connections; } }

    public ElectricityGrid Grid
    {
        get
        {
            return _grid;
        }
        set
        {
            _grid = value;

            AssignGridToConnections();
        }
    }
    
    protected virtual void ConnectionAdded(IWorldElectricityObject obj) { }
    protected virtual void ConnectionRemoved(IWorldElectricityObject obj) { }

    private List<IWorldElectricityObject> _connections = new List<IWorldElectricityObject>();
    private ElectricityGrid _grid;

    public virtual void Start()
    {
        Initialize();
    }
    protected void Initialize()
    {
        GetGrid();

        if (Grid != null)
            Grid.Add(this);

        InformationManager.Add(this);
        ElectricityManager.AddObject(this);
    }
    private void GetGrid()
    {
        if (Grid != null)
            return;
        
        if (_connections.Count > 0)
        {
            Grid = ElectricityGrid.Merge(_connections.DistinctBy(x => x.Grid).Select(x => x.Grid).Where(x => x != null).ToArray());
        }
    }
    private void AssignGridToConnections()
    {
        if (Grid == null)
            return;

        foreach (IWorldElectricityObject connection in Connections)
        {
            if (connection.Grid == null)
            {
                Grid.Add(connection);
            }
        }
    }
    public void AddConnection(IWorldElectricityObject obj)
    {
        if (!_connections.Contains(obj))
        {
            _connections.Add(obj);

            ConnectionAdded(obj);
        }            
    }
    public void RemoveConnection(IWorldElectricityObject obj)
    {
        if (_connections.Contains(obj))
        {
            _connections.Remove(obj);

            ConnectionRemoved(obj);
        }
    }
    public bool ContainsConnection(IWorldElectricityObject obj)
    {
        return _connections.Contains(obj);
    }
    protected void CheckForObjects(params Vector2[] positions)
    {
        foreach (Vector2 pos in positions)
        {
            IWorldElectricityObject obj;
            if (ElectricityManager.Poll(pos, out obj))
            {
                AddConnection(obj);

                obj.AddConnection(this);
            }
        }
        
    }
    public virtual void Remove()
    {
        if(Grid != null)
            Grid.Remove(this);

        InformationManager.Remove(this);
        ElectricityManager.RemoveObject(this);

        Destroy(gameObject);
    }
    public virtual string GetInformationString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Connections: ");
        builder.Append(_connections.Count);

        builder.AppendLine();

        builder.Append(GetGridInformation());

        return builder.ToString();
    }
    private string GetGridInformation()
    {
        if (Grid != null)
        {
            return Grid.ToString();
        }
        else
        {
            return "ID: NONE";
        }
    }
}
