using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(SoundEmittionPattern))]
public class SoundEmittionPatternEditor : Editor {

    private SoundEmittionPattern Target { get { return (SoundEmittionPattern)target; } }

    private float SampleInterval { get { return 1f / SAMPLE_RESOLUTION; } }

    /// <summary>
    /// How many keyframes to assign per clip
    /// </summary>
    private const float SAMPLE_RESOLUTION = 50;

    private const float SPACING = 3;
    private const float BUTTON_WIDTH = 80;
    private const float PADDING = 5;

    private ReorderableList _list;

    private void OnEnable()
    {
        _list = new ReorderableList(Target.Entries, typeof(SoundEmittionPattern.Entry));
        _list.drawElementCallback = DrawElement;
        _list.elementHeight = EditorGUIUtility.singleLineHeight * 2 + SPACING * 1 + PADDING * 2;
    }
    public override void OnInspectorGUI()
    {
        _list.DoLayoutList();
    }
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SoundEmittionPattern.Entry element = Target.Entries[index];

        EditorGUIUtility.labelWidth = 100;

        rect.height = EditorGUIUtility.singleLineHeight;
        rect.y += PADDING;
        
        //Clip
        element.Clip = (AudioClip)EditorGUI.ObjectField(rect, "Audio Clip", element.Clip, typeof(AudioClip), false);
        rect.y += EditorGUIUtility.singleLineHeight + SPACING;

        EditorGUI.BeginDisabledGroup(element.Clip == null);
        
        //Curve
        rect.width -= SPACING + BUTTON_WIDTH;
        element.Curve = EditorGUI.CurveField(rect, "Emition Curve", element.Curve);

        //Sample
        rect.x = rect.width + BUTTON_WIDTH / 2;
        rect.width = BUTTON_WIDTH;
        if (GUI.Button(rect, new GUIContent("Sample")))
        {
            element.Curve = SampleAudioClip(element.Clip);
        }
        EditorGUI.EndDisabledGroup();

        Target.Entries[index] = element;
    }
    private AnimationCurve SampleAudioClip(AudioClip clip)
    {
        int sampleCount = Mathf.FloorToInt(clip.length * SAMPLE_RESOLUTION);
        float rawLength = 0;

        AnimationCurve curve = new AnimationCurve();
        AnimationCurve rawCurve = GetRawCurve(clip, out rawLength);

        for (int i = 0; i < sampleCount; i++)
        {
            float time = i * SampleInterval;
            float delta = (float)i / (float)sampleCount;
            float index = delta * rawLength;
            float value = rawCurve.Evaluate(index);

            curve.AddKey(time, value);
        }
        
        return curve;
    }
    private AnimationCurve GetRawCurve(AudioClip clip, out float length)
    {
        float[] levelData = new float[clip.samples * clip.channels];
        clip.GetData(levelData, 0);
        AnimationCurve curve = new AnimationCurve();

        for (int i = 0; i < levelData.Length; i++)
        {
            curve.AddKey(new Keyframe(i, levelData[i]));
        }

        length = levelData.Length;

        return curve;
    }
}
