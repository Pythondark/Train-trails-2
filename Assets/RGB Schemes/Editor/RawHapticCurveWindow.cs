using System;
using System.IO;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RawHapticCurve))]
public class RawHapticCurveWindow : PropertyDrawer {
    // Minimum time of 1 millisecond.
    private const float min = 0.001f;
    // Max time of 10 seconds.
    private const float max = 10;
    private const int graph_height = 5;
    private const int total_height = graph_height + 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty scale = property.FindPropertyRelative("Time");
        SerializedProperty curve = property.FindPropertyRelative("HapticCurve");

        EditorGUI.Slider(
            new Rect(position.x, position.y, position.width, position.height / total_height),
            scale, min, max, "Total Time (In Seconds)");

        EditorGUI.PropertyField(
            new Rect(position.x, position.y + position.height / total_height, position.width, position.height / total_height * graph_height),
            curve, new GUIContent("Curve Over Frames"));

        // Impport and export buttons.
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Import..."))
        {
            Tuple<AnimationCurve,float> results = HapticWaveformFileUtils.Load(curve.animationCurveValue, scale.floatValue);
            curve.animationCurveValue = results.Item1;
            scale.floatValue = results.Item2;

            curve.serializedObject.ApplyModifiedProperties();
            scale.serializedObject.ApplyModifiedProperties();

            curve.serializedObject.Update();
            scale.serializedObject.Update();
        }
        if (GUILayout.Button("Export..."))
        {
            HapticWaveformFileUtils.Save(curve.animationCurveValue, scale.floatValue);
        }
        GUILayout.EndHorizontal();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * total_height;
    }
}
