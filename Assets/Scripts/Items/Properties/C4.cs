using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : PropertyBase, IPropertyInput
{
    public override string Name { get { return "C4"; } }

    public PropertyEventTypes InputTypes { get { return PropertyEventTypes.OnBlastingCap; } }

    private const string EXPLOSION_PREFAB_NAME = "Properties/Explosion";
    private const string SOUND_NAME = "C4";

    private void OnBlastingCap()
    {
        CreateAudioIndicator();
        CreateExplosionEffect();
    }
    private void CreateExplosionEffect()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(EXPLOSION_PREFAB_NAME));
        obj.transform.position = Owner.Object.transform.position;
    }
    private void CreateAudioIndicator()
    {
        SoundEmittionPattern.Entry entry = SoundEmittionPattern.GetEntry(SOUND_NAME);
        WorldAudioHandler indicator = WorldAudioHandler.Create(entry, Owner.Object);

        indicator.Loop = false;
        indicator.Toggle(true);
    }
}
