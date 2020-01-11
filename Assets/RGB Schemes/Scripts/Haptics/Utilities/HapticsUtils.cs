using UnityEngine;
using UnityEngine.XR;

public static class HapticsUtils
{
#region Configuration definitions.

    private static HapticCapabilities sHapticCapabilities;

    private static HapticCapabilities Capabilities
    {
        get
        {
            if (!InputDevices
                .GetDeviceAtXRNode(XRNode.LeftHand)
                .TryGetHapticCapabilities(out sHapticCapabilities)
                )
            {
                InputDevices
                    .GetDeviceAtXRNode(XRNode.RightHand)
                    .TryGetHapticCapabilities(out sHapticCapabilities);
            }
            return sHapticCapabilities;
        }
    }

	/// <summary>
	/// The sample rate to send haptic data to the controllers at.
	/// </summary>
	public static uint SampleRateHz
    {
        get
        {
            return Capabilities.supportsBuffer ? Capabilities.bufferFrequencyHz : (uint) DisplayFrequency + 1;
		}
    }

    /// <summary>
    /// The maximum buffer size supported by this device.
    /// </summary>
    public static uint MaxBufferSize
    {
        get
        {
#if UNITY_2019_2_OR_NEWER
            return Capabilities.supportsBuffer ? Capabilities.bufferMaxSize : (Capabilities.supportsImpulse ? 1u : 0);
#else
            // Return 10 samples by default to match what Oculus Touch gen 1 recommends.
            return 10;
#endif
        }
    }

    /// <summary>
	/// The display frequency to sync the haptic data with.
	/// </summary>
    public static float DisplayFrequency
    {
        get
        {
            return XRDevice.refreshRate;
		}
	}
    #endregion

    #region Helper methods to abstract from each platform.

    /// <summary>
    /// Sends haptic data to the specified hand.
    /// </summary>
    /// <param name="hand">The hand to send the haptic data to.</param>
    /// <param name="hapticsClip">The clip containing the haptic data.</param>
    /// <param name="mix">Whether to mix or overwrite the existing haptic clips queued up.</param>
    public static void SendHapticData(XRNode hand, HapticsClip hapticsClip, bool mix)
    {
        if (Capabilities.supportsBuffer)
        {
            InputDevices
                .GetDeviceAtXRNode(hand).SendHapticBuffer(0, hapticsClip.Samples);
        }
        else if (Capabilities.supportsImpulse && hapticsClip.Samples.Length > 0)
        {
            float sample = 0.0f;
            foreach (byte piece in hapticsClip.Samples)
            {
                sample += piece;
            }
            sample /= hapticsClip.Samples.Length;
            InputDevices
                .GetDeviceAtXRNode(hand).SendHapticImpulse(0, sample / 255.0f, (float)hapticsClip.Samples.Length / (float)SampleRateHz);
        }
        else
        {
            Debug.LogWarning("This XRNode does not support any haptic capabilities!");
        }
    }

    #endregion
}
