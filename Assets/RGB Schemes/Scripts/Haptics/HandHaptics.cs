using UnityEngine.XR;

public class HandHaptics : HapticsAbstractClass {
    public XRNode hand = XRNode.LeftHand;
    public bool DisableHaptics;

    protected override void DoRumble(int id, HapticsClip hapticsClip, bool mix = false)
    {
        base.DoRumble(id, hapticsClip, mix);
        if (!DisableHaptics)
        {
            HapticsUtils.SendHapticData(hand, hapticsClip, mix);
        }
    }

    public void SetHapticsEnabled(bool enabled)
    {
        DisableHaptics = !enabled;
    }
}
