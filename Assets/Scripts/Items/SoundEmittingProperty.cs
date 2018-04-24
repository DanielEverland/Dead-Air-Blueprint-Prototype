using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundEmittingProperty : PropertyBase {

    public abstract string SoundName { get; }

    protected WorldAudioHandler AudioIndicator { get; private set; }

    public override void CreateInstance(GameObject obj)
    {
        SoundEmittionPattern.Entry entry = SoundEmittionPattern.GetEntry(SoundName);
        AudioIndicator = WorldAudioHandler.Create(entry, Owner.Object);
    }
    public override string[] GetInformation()
    {
        if(AudioIndicator == null)
        {
            return new string[1] { "NO INDICATOR" };
        }
        else
        {
            return new string[1] { AudioIndicator.Scale.ToString("F0") };
        }       
    }
}
