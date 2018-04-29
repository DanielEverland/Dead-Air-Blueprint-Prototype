using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(LineRenderer))]
public class ElectricityLine : ElectricalObject {
    
    public LineRenderer Renderer;

    public override IShape Shape { get { return _shapeHandler.Shape; } }
    public override Vector2 Point { get { return _shapeHandler.Shape.BoundingBox.Center; } }

    private const float THICKNESS = 0.2f;
    
    private ShapeHandler _shapeHandler;

    private void Awake()
    {
        _shapeHandler = new ShapeHandler(Renderer);
        
        SanitizeComponent();        
    }
    public override void Start()
    {
    }
    public void Initialize(Vector2 startPos, Vector2 endPos)
    {
        CheckForObjects(startPos, endPos);

        Vector2 fixedStartPos = SetPosition(0, startPos, endPos);
        SetPosition(1, endPos, fixedStartPos);

        base.Initialize();
    }           
    private Vector2 SetPosition(int index, Vector2 pos, Vector2 other)
    {
        Vector2 fixedPos = ElectricityManager.ResolvePosition(pos, other);
        Renderer.SetPosition(index, fixedPos);
        
        return fixedPos;
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
