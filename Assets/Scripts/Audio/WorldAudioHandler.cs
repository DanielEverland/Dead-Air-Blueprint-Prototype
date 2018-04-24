﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WorldAudioHandler : MonoBehaviour {

    [SerializeField]
    private AudioSource _source;

    public float Scale { get; private set; }
    
    private const float SOUND_CHANGE_SPEED = 10;

    private SoundEmittionPattern.Entry _entry;
    private IWorldObject _obj;
    private bool _isActive;
    
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
        if(!active && _isActive)
        {
            _source.Stop();
        }
        else if(active && !_isActive)
        {
            _source.Play();
        }

        _isActive = active;
    }
    private void Update()
    {
        SetScale();
        Transform();
    }
    private void SetScale()
    {
        if (!_isActive)
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
