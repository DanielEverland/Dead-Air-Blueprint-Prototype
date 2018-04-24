using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : ElectronicSoundEmittingProperty
{
    public override string SoundName { get { return "Alarm"; } }
    public override string Name { get { return "Electric Alarm"; } }
}
