using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects, CustomEditor(typeof(RaycastElement), false)]
public class RaycastElementEditor : GraphicEditor
{
    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
        // skipping AppearanceControlsGUI
        base.RaycastControlsGUI();
        base.serializedObject.ApplyModifiedProperties();
    }
}