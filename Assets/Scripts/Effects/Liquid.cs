using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[RequireComponent(typeof(SpriteRenderer))]
public class Liquid : MonoBehaviour, IWorldObject {
    
    [SerializeField]
    private SpriteRenderer _renderer;

    public static readonly Color FireColor = Color.red;

    public Vector2 Point { get { return transform.position; } }
    public float Radius { get { return _radius; } }
    public bool IsOnFire { get { return _data.IsOnFire; } }
    public bool IsFlammable { get { return _data.IsFlammable; } }

    private const float MIN_RADIUS = 0.5f;
    private const float RADIUS_SPEED = 20;
    private const float FIRE_WAIT_TIME = 1;

    private LiquidData _data;
    private float _radius;
    private float? _targetRadius;

    private void Awake()
    {
        _radius = MIN_RADIUS;

        WorldItemEventHandler.Add(this);
        InformationManager.Add(this);
    }
    public static Liquid Create(LiquidData data)
    {
        Liquid liquid = Instantiate(Resources.Load<GameObject>("Liquid")).GetComponent<Liquid>();
        liquid.Initialize(data);

        return liquid;
    }
    public void Initialize(LiquidData data)
    {
        _data = data;

        SetRenderState();
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (_targetRadius.HasValue && ((_targetRadius.Value - _radius) > 0.01f))
        {
            _radius = Mathf.Lerp(_radius, _targetRadius.Value, Time.deltaTime * RADIUS_SPEED);

            transform.localScale = new Vector3(_radius, _radius, 1);

            WorldItemEventHandler.Poll(this);
        }
    }
    private void SetRenderState()
    {
        _renderer.material.color = _data.Color;
        _renderer.material.SetColor("_SpecColor", _data.Color);

        Color color = _renderer.material.GetColor("_EmissionColor"); 
        _renderer.material.SetColor("_EmissionColor", color * _data.Color);

        _targetRadius = _data.Radius;
    }
    private void OnValidate()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void SetOnFire()
    {
        if (_data.IsOnFire || !_data.IsFlammable)
            return;

        _data.IsOnFire = true;
        _data.Color = FireColor;

        SetRenderState();

        WorldItemEventHandler.RaiseEvent(this, FIRE_WAIT_TIME, PropertyEventTypes.OnIgnite);
    }
    public void HandleCollision(IWorldObject obj)
    {
        if(obj is Liquid)
        {
            Liquid other = obj as Liquid;

            if(other._data != _data)
            {
                Debug.Log(other);
            }
        }
    }
    public void RaiseEvent(PropertyEventTypes type, params object[] args)
    {
        if(type == PropertyEventTypes.OnIgnite)
        {
            SetOnFire();
        }
    }
    public string GetInformationString()
    {
        return _data.ToString();
    }
    public void Destroy()
    {
        InformationManager.Add(this);

        Destroy(gameObject);
    }
}