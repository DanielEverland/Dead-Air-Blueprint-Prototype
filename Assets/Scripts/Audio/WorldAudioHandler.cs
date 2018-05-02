using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WorldAudioHandler : MonoBehaviour {

    [SerializeField]
    private AudioSource _source;

    public AudioSource Source { get { return _source; } }
    public float Scale { get; private set; }
    public bool IsActive { get; private set; }
    public bool Loop { get { return Source.loop; } set { Source.loop = value; } }
    
    private const float SOUND_CHANGE_SPEED = 10;

    private SoundEmittionPattern.Entry _entry;
    private IWorldObject _obj;
    
    public static WorldAudioHandler Create(SoundEmittionPattern.Entry entry, IWorldObject obj)
    {
        WorldAudioHandler indicator = Instantiate(Resources.Load<GameObject>("SoundIndicator")).GetComponent<WorldAudioHandler>();
        indicator.Initialize(entry, obj);

        return indicator;
    }
    public void Initialize(SoundEmittionPattern.Entry entry, IWorldObject obj)
    {
        _obj = obj;
        _entry = entry;
        _source.clip = entry.Clip;

        Toggle(false);
    }
    public void Toggle(bool active)
    {
        if(!active && IsActive)
        {
            _source.Stop();
        }
        else if(active && !IsActive)
        {
            _source.Play();
        }

        IsActive = active;
    }
    private void Update()
    {
        SetScale();
        Transform();
    }
    private void SetScale()
    {
        if (!IsActive)
        {
            Scale = 0;
        }
        else
        {
            float target = _entry.Curve.Evaluate(_source.time) * _entry.Coefficient;
            Scale = Mathf.Lerp(Scale, target, SOUND_CHANGE_SPEED * Time.deltaTime);
        }        
    }
    private void Transform()
    {
        transform.position = _obj.Point;
        transform.localScale = new Vector3(Scale, Scale, 1);
    }
    private void OnValidate()
    {
        _source = GetComponent<AudioSource>();
    }
}
