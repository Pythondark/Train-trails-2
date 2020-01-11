using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public static class HapticWaveformFileUtils
{
#if UNITY_EDITOR
    private static readonly string MAGIC = "RGBH";
    private static readonly string VERSION = "0110";

    private static string mSaveLocation = "Assets";
    private static string mFileName = "export.hwf";

    public static void Save(AnimationCurve animationCurve, float value)
    {
        string path = EditorUtility.SaveFilePanel("Export waveform to a file", mSaveLocation, mFileName, "hwf");
        if (path.Length > 0)
        {
            mSaveLocation = Path.GetDirectoryName(path);
            mFileName = Path.GetFileName(path);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MAGIC);
                    writer.Write(VERSION);
                    writer.Write(value);
                    writer.Write(animationCurve.length);
                    foreach (Keyframe keyframe in animationCurve.keys)
                    {
                        writer.Write(keyframe.time);
                        writer.Write(keyframe.value);
                        writer.Write((int)keyframe.weightedMode);
                        writer.Write(keyframe.inTangent);
                        writer.Write(keyframe.inWeight);
                        writer.Write(keyframe.outTangent);
                        writer.Write(keyframe.outWeight);
                    }
                    for (int i = 0; i < animationCurve.length; i++)
                    {
                        writer.Write((int)AnimationUtility.GetKeyLeftTangentMode(animationCurve, i));
                        writer.Write((int)AnimationUtility.GetKeyRightTangentMode(animationCurve, i));
                    }
                }
            }
        }
    }

    public static Tuple<AnimationCurve, float> Load(AnimationCurve animationCurve, float time)
    {
        string path = EditorUtility.OpenFilePanel("Import waveform from a file", mSaveLocation, "hwf");
        if (path.Length > 0)
        {
            mSaveLocation = Path.GetDirectoryName(path);
            mFileName = Path.GetFileName(path);

            using (Stream stream = new FileStream(path, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    reader.ReadChar();
                    string magic = new string(reader.ReadChars(4)).Trim();
                    reader.ReadChar();
                    string version = new string(reader.ReadChars(4)).Trim();

                    if (magic.Equals(MAGIC) && version.Equals(VERSION))
                    {
                        time = reader.ReadSingle();
                        int keyframeCount = reader.ReadInt32();
                        animationCurve = new AnimationCurve();
                        for (int i = 0; i < keyframeCount; i++)
                        {
                            Keyframe keyframe = new Keyframe(reader.ReadSingle(), reader.ReadSingle())
                            {
                                weightedMode = (WeightedMode)reader.ReadInt32(),
                                inTangent = reader.ReadSingle(),
                                inWeight = reader.ReadSingle(),
                                outTangent = reader.ReadSingle(),
                                outWeight = reader.ReadSingle()
                            };
                            animationCurve.AddKey(keyframe);
                        }
                        for (int i = 0; i < animationCurve.length && i < keyframeCount; i++)
                        {
                            AnimationUtility.SetKeyLeftTangentMode(animationCurve, i, (AnimationUtility.TangentMode)reader.ReadInt32());
                            AnimationUtility.SetKeyRightTangentMode(animationCurve, i, (AnimationUtility.TangentMode)reader.ReadInt32());
                        }
                    }
                }
            }
        }
        return Tuple.Create(animationCurve, time);
    }
#endif
}
