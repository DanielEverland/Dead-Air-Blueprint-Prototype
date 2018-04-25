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
    private const int ELEMENT_HEIGHT = 4;

    private ReorderableList _list;

    private void OnEnable()
    {
        _list = new ReorderableList(Target.Entries, typeof(SoundEmittionPattern.Entry));
        _list.drawElementCallback = DrawElement;
        _list.elementHeight = EditorGUIUtility.singleLineHeight * ELEMENT_HEIGHT + SPACING * (ELEMENT_HEIGHT - 1) + PADDING * 2;
        _list.drawHeaderCallback += (Rect rect) => { EditorGUI.LabelField(rect, "Emittion Patterns"); };
    }
    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        _list.DoLayoutList();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        SoundEmittionPattern.Entry element = Target.Entries[index];

        EditorGUIUtility.labelWidth = 100;

        //Name
        Rect nameRect = new Rect(rect.x, rect.y + PADDING, rect.width, EditorGUIUtility.singleLineHeight);
        element.Name = EditorGUI.TextField(nameRect, new GUIContent("Name"), element.Name);

        EditorGUI.BeginDisabledGroup(element.Name == string.Empty);
        //Clip
        Rect clipRect = new Rect(rect.x, nameRect.y + SPACING + nameRect.height, rect.width, EditorGUIUtility.singleLineHeight);
        element.Clip = (AudioClip)EditorGUI.ObjectField(clipRect, "Audio Clip", element.Clip, typeof(AudioClip), false);
        EditorGUI.EndDisabledGroup();
        
        EditorGUI.BeginDisabledGroup(element.Clip == null || element.Name == string.Empty);
        //Curve
        Rect curveRect = new Rect(rect.x, clipRect.y + SPACING + clipRect.height, rect.width - (BUTTON_WIDTH + SPACING), EditorGUIUtility.singleLineHeight);
        element.Curve = EditorGUI.CurveField(curveRect, "Emition Curve", element.Curve);

        //Sample
        Rect sampleButton = new Rect(curveRect.x + curveRect.width + SPACING, curveRect.y, BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(sampleButton, new GUIContent("Sample")))
        {
            element.Curve = SampleAudioClip(element.Clip);
        }
        
        //Coefficient
        Rect coefficientRect = new Rect(rect.x, curveRect.y + SPACING + curveRect.height, rect.width, EditorGUIUtility.singleLineHeight);
        element.Coefficient = EditorGUI.FloatField(coefficientRect, "Coefficient", element.Coefficient);
        EditorGUI.EndDisabledGroup();

        Target.Entries[index] = element;
    }
    private AnimationCurve SampleAudioClip(AudioClip clip)
    {
        int sampleCount = Mathf.FloorToInt(clip.length * SAMPLE_RESOLUTION);
        float rawLength = 0;

        AnimationCurve curve = new AnimationCurve();
        AnimationCurve rawCurve = GetRawCurve(clip, out rawLength);

        float[] selectedValues = new float[sampleCount];
        for (int i = 0; i < selectedValues.Length; i++)
        {
            float index = (float)i / (float)sampleCount * rawLength;
            selectedValues[i] = rawCurve.Evaluate(index);
        }

        float min, max;
        GetMinAndMax(selectedValues, out min, out max);
        
        for (int i = 0; i < selectedValues.Length; i++)
        {
            float difference = (selectedValues[i] - min) / (max - min);
            float time = i * SampleInterval;

            curve.AddKey(time, difference);
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
            curve.AddKey(new Keyframe(i, AdjustRawLevel(levelData[i])));
        }

        length = levelData.Length;

        return curve;
    }
    private void GetMinAndMax(float[] array, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] < min)
                min = array[i];

            if (array[i] > max)
                max = array[i];
        }
    }
    /// <summary>
    /// Converts the raw values from (-1:1) to (0:1)
    /// </summary>
    /// <param name="rawLevel"></param>
    /// <returns></returns>
    private float AdjustRawLevel(float rawLevel)
    {
        if(rawLevel >= 0)
        {
            return 0.5f + (rawLevel * 0.5f);
        }
        else
        {
            return 0.5f - Mathf.Abs(rawLevel) * 0.5f;
        }
    }
}
