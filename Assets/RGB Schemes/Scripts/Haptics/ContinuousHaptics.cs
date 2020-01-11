using System;
using UnityEngine;

[Serializable]
public class ContinuousHaptics : StandardHapticsBase
{
    /// <summary>
    /// Whether to start the haptic once the object is awake.
    /// </summary>
    public bool PlayOnAwake;

    /// <summary>
    /// Begin playing this haptic.
    /// </summary>
    public new void Play()
    {
        base.Play();
    }

    /// <summary>
    /// Pause this haptic.
    /// </summary>
    public void Pause()
    {
        mPlaying = false;
    }

    /// <summary>
    /// Stop this haptic altogether.
    /// </summary>
    public new void Stop()
    {
        base.Stop();
    }

    #region Standard Unity object methods.
    private void Awake()
    {
        eventID = RANDOM.Next();
        if (PlayOnAwake)
        {
            Play();
        }
    }

    #endregion
}
