using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class HapticFrame
{
    public enum Type
    {
        Raw, Audio
    };

    /// <summary>
    /// The type of clip to use.
    /// </summary>
    public Type ClipType = Type.Raw;
    /// <summary>
    /// Indicates whether to load an audio file on a separate thread.
    /// </summary>
    public bool LoadOnSeparateThread;

    /// <summary>
    /// The audio clip to use for this particular frame.
    /// </summary>
    [SerializeField]
    private AudioClip HapticAudioClip;
    /// <summary>
    /// The raw haptic curve to use for this particular frame.
    /// </summary>
    [SerializeField]
    private RawHapticCurve RawHapticCurve = new RawHapticCurve();
    /// <summary>
    /// The actual data being sent.
    /// </summary>
    private byte[] mHapticData;
    /// <summary>
    /// Indicates whether we have already cached the haptic clip.
    /// </summary>
    private bool mHaveCachedClip;
    /// <summary>
    /// The cached haptic clip we create.
    /// </summary>
    private HapticsClip mHapticClip;

    /// <summary>
    /// Sets the source of the haptic and ensures that the
    /// cached version is updated with this new source. Also
    /// updates the ClipType to be an AudioClip.
    /// </summary>
    /// <param name="clip">The <seealso cref="AudioClip"/> to use.</param>
    public void SetHapticSource(AudioClip clip)
    {
        HapticAudioClip = clip;
        ClipType = Type.Audio;
        mHaveCachedClip = false;
    }

    /// <summary>
    /// Sets the source of the haptic and ensures that the
    /// cached version is updated with this new source. Also
    /// updates the ClipType to be a Raw clip.
    /// </summary>
    /// <param name="curve">The <seealso cref="RawHapticCurve"/> to use.</param>
    public void SetHapticSource(RawHapticCurve curve)
    {
        RawHapticCurve = curve;
        ClipType = Type.Raw;
        mHaveCachedClip = false;
    }

    /// <summary>
    /// Creates an empty clip with no haptics in use.
    /// </summary>
    public HapticFrame()
    {
        Debug.Log("New haptic frame created!");
    }

    /// <summary>
    /// Default constructor, allows you to set the default time for a <see cref="RawHapticCurve"/>.
    /// </summary>
    /// <param name="defaultRawTime">The default time for the <see cref="RawHapticCurve"/>. 1 second by default.</param>
    public HapticFrame(float defaultRawTime = 1.0f)
    {
        RawHapticCurve.Time = defaultRawTime;
    }

    /// <summary>
    /// The haptic clip for this frame.
    /// </summary>
    public HapticsClip Clip
    {
        get
        {
            if (!mHaveCachedClip)
            {
                GenerateClip();
            }
            return mHapticClip;
        }
    }

    /// <summary>
    /// Gets the Haptics clip to actually send to the controller.
    /// </summary>
    /// <returns>The haptic clip to be played.</returns>
    private void GenerateClip()
    {
        mHaveCachedClip = true;
        switch (ClipType)
        {
            case Type.Audio:
                if (HapticAudioClip.loadState == AudioDataLoadState.Loaded)
                {
                    mHapticClip = new HapticsClip(HapticAudioClip, LoadOnSeparateThread);
                }
                else
                {
                    Debug.Log("Audio clip not loaded, feeding empty clip.");
                    mHapticClip = new HapticsClip();
                    mHaveCachedClip = false;
                }
                break;
            case Type.Raw:
                if (mHapticData == null || mHapticData.Length != RawHapticCurve.FrameCount)
                {
                    mHapticData = new byte[RawHapticCurve.FrameCount];
                    for (int i = 0; i < RawHapticCurve.FrameCount; i++)
                    {
                        float pos = (float)i / RawHapticCurve.FrameCount;
                        mHapticData[i] = (byte)(RawHapticCurve.HapticCurve.Evaluate(pos) * 255);
                    }
                }
                mHapticClip = new HapticsClip(mHapticData, mHapticData.Length);
                break;
            default:
                Debug.Log("Invalid haptic frame type!");
                mHapticClip = null;
                break;
        }
    }

#if UNITY_EDITOR
        public static void createEditor(SerializedProperty property)
    {
        EditorGUILayout.LabelField(property.displayName);
        EditorGUI.indentLevel = 1;
        SerializedProperty clipType = property.FindPropertyRelative("ClipType");
        SerializedProperty RawHapticCurve = property.FindPropertyRelative("RawHapticCurve");
        SerializedProperty AudioClip = property.FindPropertyRelative("HapticAudioClip");

        EditorGUILayout.PropertyField(clipType);

        switch (clipType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(RawHapticCurve, true);
                break;
            case 1:
                EditorGUILayout.PropertyField(AudioClip);
                break;
            default:
                break;
        }
        EditorGUI.indentLevel = 0;
    }
#endif
}
