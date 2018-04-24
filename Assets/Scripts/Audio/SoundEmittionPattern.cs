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

    public static Entry GetEntry(string name)
    {
        Entry entry = Instance.Entries.FirstOrDefault(x => x.Name == name);

        if (entry == null)
            throw new System.NotImplementedException("No emittion pattern has been created for " + name);

        return entry;
    }

    public List<Entry> Entries = new List<Entry>();

    [System.Serializable]
    public class Entry
    {
        public string Name = string.Empty;
        public AudioClip Clip = null;
        public AnimationCurve Curve = new AnimationCurve();
        public float Coefficient = 1;
    }
}
