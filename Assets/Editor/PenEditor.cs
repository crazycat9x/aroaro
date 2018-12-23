using Aroaro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Defines the <see cref="PenEditor" />
/// </summary>
[CustomEditor(typeof(Pen))]
public class PenEditor : Editor
{
    SerializedProperty penColor;

    internal void OnEnable()
    {
        penColor = serializedObject.FindProperty("penColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Pen penScript = (Pen)target;
        Color settedColor = EditorGUILayout.ColorField(penScript.PenColor);
        penScript.penSize = EditorGUILayout.IntSlider(penScript.penSize, 1, 10);
        penScript.PenColor = settedColor;
        penColor.colorValue = settedColor;
        serializedObject.ApplyModifiedProperties();
    }
}
