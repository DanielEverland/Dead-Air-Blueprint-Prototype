using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ElectricityLine : MonoBehaviour, IWorldElectricityObject, IInformationObject {
    
    public LineRenderer Renderer;
    public IShape Shape { get { return _shapeHandler.Shape; } }
    public Vector2 Point { get { return _shapeHandler.Shape.BoundingBox.Center; } }
    public ElectricityGrid Grid { get; set; }

    private const float THICKNESS = 0.2f;

    private List<IWorldElectricityObject> _connections;
    private ShapeHandler _shapeHandler;

    private void Awake()
    {
        _connections = new List<IWorldElectricityObject>();
        _shapeHandler = new ShapeHandler(Renderer);
        
        SanitizeComponent();        
    }
    public void Initialize(Vector2 startPos, Vector2 endPos)
    {
        CheckForObject(startPos);
        CheckForObject(endPos);

        Vector2 fixedStartPos = SetPosition(0, startPos, endPos);
        SetPosition(1, endPos, fixedStartPos);

        AssignGrid();

        ElectricityManager.AddObject(this);
        InformationManager.Add(this);
    }
    private void AssignGrid()
    {
        if(_connections.Count == 0)
        {
            Grid = new ElectricityGrid();
            Grid.Add(this);
        }
        else
        {
            ElectricityGrid largest = ElectricityGrid.Merge(_connections.DistinctBy(x => x.Grid).Select(x => x.Grid).Where(x => x != null).ToArray());

            foreach (IWorldElectricityObject connection in _connections)
            {
                //Required for objects without a grid
                connection.Grid = largest;
            }

            Grid = largest;
            Grid.Add(this);
        }
    }
    private Vector2 SetPosition(int index, Vector2 pos, Vector2 other)
    {
        Vector2 fixedPos = ElectricityManager.ResolvePosition(pos, other);
        Renderer.SetPosition(index, fixedPos);
        
        return fixedPos;
    }
    private void CheckForObject(Vector2 pos)
    {
        IWorldElectricityObject obj;
        if(ElectricityManager.Poll(pos, out obj))
        {
            _connections.Add(obj);
        }
    }
    private void SanitizeComponent()
    {
        Renderer.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        Renderer.startWidth = THICKNESS;
        Renderer.endWidth = THICKNESS;
    }
    private void OnValidate()
    {
        Renderer = GetComponent<LineRenderer>();
    }
    public void Remove()
    {
        Grid.Remove(this);

        ElectricityManager.RemoveObject(this);
        InformationManager.Remove(this);
    }

    public string GetInformationString()
    {
        return Grid.ToString();
    }

    private class ShapeHandler
    {
        public ShapeHandler(LineRenderer renderer)
        {
            _renderer = renderer;
        }
        
        public LineShape Shape
        {
            get
            {
                PollUpdate();

                return _shape;
            }
        }

        private readonly LineRenderer _renderer;

        private LineShape _shape;
        private Vector3 _oldStart;
        private Vector3 _oldEnd;

        private void PollUpdate()
        {
            if (ShouldUpdate())
            {
                _oldStart = _renderer.GetPosition(0);
                _oldEnd = _renderer.GetPosition(1);

                _shape = new LineShape(_oldStart, _oldEnd, THICKNESS);
            }
        }
        private bool ShouldUpdate()
        {
            return _renderer.GetPosition(0) != _oldStart || _renderer.GetPosition(1) != _oldEnd || _shape == null;
        }
    }
}
