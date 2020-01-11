#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(ContinuousHaptics))]
[CanEditMultipleObjects]
public class ContinuousHapticsEditor : Editor
{
    SerializedProperty Haptic, PlayOnAwake, Loop, SyncAudioSource, LoadOnSeparateThread;
    protected static bool AdvancedSettings;

    private void OnEnable()
    {
        Haptic = serializedObject.FindProperty("Haptic");
        PlayOnAwake = serializedObject.FindProperty("PlayOnAwake");
        Loop = serializedObject.FindProperty("Loop");
        SyncAudioSource = serializedObject.FindProperty("SyncAudioSource");
        LoadOnSeparateThread = Haptic.FindPropertyRelative("LoadOnSeparateThread");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(PlayOnAwake);
        EditorGUILayout.PropertyField(Loop);
        HapticFrame.createEditor(Haptic);

        AdvancedSettings = EditorGUILayout.Foldout(AdvancedSettings, "Advanced Settings");
        if (AdvancedSettings)
        {
            EditorGUI.indentLevel++;

            if (Haptic.FindPropertyRelative("ClipType").enumValueIndex == 1)
            {
                // Currently broken because Unity doesn't like multithreaded code.
                // EditorGUILayout.PropertyField(LoadOnSeparateThread);
            }
            EditorGUILayout.PropertyField(SyncAudioSource);

            EditorGUI.indentLevel--;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
