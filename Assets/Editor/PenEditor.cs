using Aroaro;
using UnityEditor;

/// <summary>
/// Defines the <see cref="PenEditor" />
/// </summary>
[CustomEditor(typeof(Pen))]
public class PenEditor : Editor
{
    /// <summary>
    /// The OnInspectorGUI
    /// </summary>
    public override void OnInspectorGUI()
    {
        Pen penScript = (Pen)target;

        penScript.penSize = EditorGUILayout.IntSlider("Pen Size", penScript.penSize, 1, 10);
        penScript.PenColor = EditorGUILayout.ColorField("Pen Color", penScript.PenColor);
    }
}
