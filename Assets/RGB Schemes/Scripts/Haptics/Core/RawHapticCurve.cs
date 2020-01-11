using System;
using UnityEngine;

[Serializable]
public class RawHapticCurve {
    public float Time = 1;
    public AnimationCurve HapticCurve = AnimationCurve.Constant(0.0f, 1.0f, 0.25f);

    public int FrameCount
    {
        get
        {
            return (int)(Time * HapticsUtils.SampleRateHz);
        }
    }
}
