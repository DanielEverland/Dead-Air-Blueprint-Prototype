using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEmittionPattern.asset", menuName = "Sound Emittion Pattern", order = 200)]
public class SoundEmittionPattern : ScriptableObject {

    private static SoundEmittionPattern Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<SoundEmittionPattern>("SoundEmittionPattern");

            return _instance;
        }
    }
    private static SoundEmittionPattern _instance;

    public static AnimationCurve GetCurve(AudioClip clip)
    {
        Entry entry = _instance.Entries.FirstOrDefault(x => x.Clip == clip);

        if (entry == null)
            throw new System.NotImplementedException("No emittion pattern has been created for " + clip);

        return entry.Curve;
    }

    public List<Entry> Entries = new List<Entry>();

    [System.Serializable]
    public class Entry
    {
        public AudioClip Clip = null;
        public AnimationCurve Curve = new AnimationCurve();
        public float Coefficient = 1;
    }
}
