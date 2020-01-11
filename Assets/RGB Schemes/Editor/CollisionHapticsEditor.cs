#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(CollisionHaptics))]
[CanEditMultipleObjects]
public class CollisionHapticsEditor : Editor
{
    SerializedProperty Haptic, TransferImmediate, SyncAudioSource, LoadOnSeparateThread;
    protected static bool AdvancedSettings;

    private void OnEnable()
    {
        Haptic = serializedObject.FindProperty("Haptic");
        TransferImmediate = serializedObject.FindProperty("TransferImmediate");
        SyncAudioSource = serializedObject.FindProperty("SyncAudioSource");
        LoadOnSeparateThread = Haptic.FindPropertyRelative("LoadOnSeparateThread");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        HapticFrame.createEditor(Haptic);

        AdvancedSettings = EditorGUILayout.Foldout(AdvancedSettings, "Advanced Settings");
        if (AdvancedSettings)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(TransferImmediate);
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
