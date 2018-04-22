using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Liquid : MonoBehaviour {

    public LiquidData Data
    {
        get
        {
            return _data;
        }
        set
        {
            _data = value;

            SetRenderState();
        }
    }

    [SerializeField]
    private SpriteRenderer _renderer;
    
    private const float MIN_RADIUS = 0.5f;
    private const float RADIUS_SPEED = 20;

    private LiquidData _data;
    private float _radius;
    private float? _targetRadius;

    private void Awake()
    {
        _radius = MIN_RADIUS;
    }
    public static Liquid Create(LiquidData data)
    {
        Liquid liquid = Instantiate(Resources.Load<GameObject>("Liquid")).GetComponent<Liquid>();
        liquid.Initialize(data);

        return liquid;
    }
    public void Initialize(LiquidData data)
    {
        Data = data;
    }
    private void Update()
    {
        if (_targetRadius.HasValue)
        {
            _radius = Mathf.Lerp(_radius, _targetRadius.Value, Time.deltaTime * RADIUS_SPEED);

            transform.localScale = new Vector3(_radius, _radius, 1);
        }
    }
    private void SetRenderState()
    {
        _renderer.color = Data.Color;
        _targetRadius = Data.Radius;
    }
    private void OnValidate()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}