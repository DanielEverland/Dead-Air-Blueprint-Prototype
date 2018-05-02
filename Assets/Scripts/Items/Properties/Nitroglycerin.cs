using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitroglycerin : LiquidPropertyBase
{
    public override Color32 Color { get { return UnityEngine.Color.white; } }

    public override bool IsFlammable => true;
    public override string Name => "Nitroglycerin";

    private const string EXPLOSION_PREFAB_NAME = "Properties/Explosion";
    private const string SOUND_NAME = "C4";

    protected override void OnLiquidContainerBreaks(ILiquidContainerProperty container)
    {
        OnExplode();
    }
    private void OnExplode()
    {
        Owner.Remove(this);

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
