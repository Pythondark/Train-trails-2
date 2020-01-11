using System;
using System.Collections.Generic;
using UnityEngine;

public class StandardHapticsBase : HapticsAbstractClass
{
    /// <summary>
    /// The haptic frame to play.
    /// </summary>
    public HapticFrame Haptic = new HapticFrame(0.03125f);

    /// <summary>
    /// Set to true to loop, false otherwise.
    /// </summary>
    public bool Loop;
    /// <summary>
    /// Set an audio souce to synchronize the playback with.
    /// </summary>
    public AudioSource SyncAudioSource;

    /// <summary>
    /// The index of where we are currently playing haptics sounds from.
    /// </summary>
    protected int mSampleCopyIndex;
    /// <summary>
    /// The start time of the haptic. Used to calculate where in the playback we are.
    /// </summary>
    protected float mStartTime;
    /// <summary>
    /// The last update time for when we sent our haptics.
    /// </summary>
    protected float mLastTime;
    /// <summary>
    /// Booleaning indicating whether we are currently playing the haptic.
    /// </summary>
    protected bool mPlaying;

    /// <summary>
    /// The list of other haptics nodes that we are colliding with.
    /// Note that we store them in a <see cref="HashSet{HapticsAbstractClass}"/>
    /// to make sure that we never store duplicate items.
    /// </summary>
    protected HashSet<HapticsAbstractClass> mHapticNodes = new HashSet<HapticsAbstractClass>();

    /// <summary>
    /// Begin playing this haptic.
    /// </summary>
    protected void Play()
    {
        mPlaying = true;
        if (Math.Abs(mStartTime - -1) < 0.0f)
        {
            mStartTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// Stop this haptic altogether.
    /// </summary>
    protected void Stop()
    {
        mPlaying = false;
        mSampleCopyIndex = 0;
        mStartTime = -1;
    }

    #region HapticsAbstractClass overrides.
    protected override void DoRumble(int id, HapticsClip hapticsClip, bool mix = false)
    {
        base.DoRumble(id, hapticsClip, mix);
        bool hasNull = false;
        foreach (HapticsAbstractClass haptics in mHapticNodes)
        {
            // Because the object can be destroyed mid-use, we should ensure it is not null.
            if (haptics != null)
            {
                haptics.transmitRumble(id, hapticsClip, gameObject, mix);
            }
            else
            {
                hasNull = true;
            }
        }
        if (hasNull)
        {
            mHapticNodes.RemoveWhere(x => x == null);
        }
    }
    #endregion

    #region Collision handling.
    protected void OnTriggerEnter(Collider other)
    {
        HapticsAbstractClass hapticsAbstractClass = other.gameObject.GetComponent<HandHaptics>();
        if (hapticsAbstractClass != null && mHapticNodes.Add(hapticsAbstractClass))
        {
            //eventID = RANDOM.Next();
            // hapticsAbstractClass.transmitRumble(eventID, Haptic.OVRClip, gameObject);
            //Debug.Log("Added object " + hapticsAbstractClass);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        HapticsAbstractClass hapticsAbstractClass = other.gameObject.GetComponent<HandHaptics>();
        if (hapticsAbstractClass != null && mHapticNodes.Remove(hapticsAbstractClass))
        {
            //Debug.Log("Removed object " + hapticsAbstractClass);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        HapticsAbstractClass hapticsAbstractClass = collision.gameObject.GetComponent<HapticsAbstractClass>();
        if (hapticsAbstractClass != null && !(hapticsAbstractClass is HandHaptics) && mHapticNodes.Add(hapticsAbstractClass))
        {
            //Debug.Log("Added object " + hapticsAbstractClass + " to " + gameObject + " with id of " + eventID);
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        HapticsAbstractClass hapticsAbstractClass = collision.gameObject.GetComponent<HapticsAbstractClass>();
        if (hapticsAbstractClass != null && !(hapticsAbstractClass is HandHaptics) && mHapticNodes.Remove(hapticsAbstractClass))
        {
            //Debug.Log("Removed object " + hapticsAbstractClass + " from " + gameObject);
        }
    }
    #endregion

    #region Haptic helpers.
    /// <summary>
    /// Plays a piece of the <see cref="HapticsClip"/> as specified by the sample length parameter.
    /// </summary>
    /// <param name="clip">The <see cref="HapticsClip"/> to pull the sample from.</param>
    /// <param name="sampleLength">The length of the sample to pull from the <see cref="HapticsClip"/>.</param>
    protected void playHapticPiece(HapticsClip clip, int sampleLength)
    {
        if (clip.Length <= sampleLength)
        {
            DoRumble(RANDOM.Next(), new HapticsClip(clip.Samples, clip.Length));
            mSampleCopyIndex = 0;
            mPlaying = Loop;
        }
        else
        {
            byte[] sampleData = new byte[sampleLength];
            if (mSampleCopyIndex >= clip.Length)
            {
                mSampleCopyIndex = 0;
                mPlaying = Loop;
            }

            if (mPlaying)
            {
                int length = Math.Min(clip.Length - mSampleCopyIndex, sampleLength);
                Array.Copy(clip.Samples, mSampleCopyIndex, sampleData, 0, length);

                // If we are at the end of the sample data, restart from the beginning.
                if (mSampleCopyIndex + sampleLength >= clip.Length)
                {
                    mSampleCopyIndex = 0;
                    mPlaying = Loop;
                    if (mPlaying)
                    {
                        Array.Copy(clip.Samples, mSampleCopyIndex, sampleData, length, sampleLength - length);
                        length = sampleLength - length;
                    }
                    else
                    {
                        sampleLength = length;
                        length = 0;
                    }
                }
                mSampleCopyIndex += length;
                DoRumble(RANDOM.Next(), new HapticsClip(sampleData, sampleLength));
            }
        }
    }
    #endregion

    #region Unity overrides.
    protected void Update()
    {
        float elapsedTime = Time.realtimeSinceStartup - mLastTime;
        mLastTime = Time.realtimeSinceStartup;
        if (mPlaying)
        {
            // Get our clip.
            if (Haptic.Clip.Length > 0)
            {
                // Calculate how many samples we want based on the time between frames.
                int sampleLength = (int)(elapsedTime * HapticsUtils.SampleRateHz + 0.5f);
                sampleLength = (int)Math.Min(sampleLength, HapticsUtils.SampleRateHz / HapticsUtils.DisplayFrequency);
                sampleLength = (int)Math.Min(sampleLength, HapticsUtils.MaxBufferSize);
                // Calculate how much time has passed and then normalize it into our haptic sample rate.
                float timePassed = (Time.realtimeSinceStartup - mStartTime) * HapticsUtils.SampleRateHz + 0.5f;
                if (Haptic.ClipType == HapticFrame.Type.Audio && SyncAudioSource != null)
                {
                    timePassed = SyncAudioSource.time * HapticsUtils.SampleRateHz + 0.5f;
                }
                // Now determine where in our audio we should be based on the amount of time that has passed.
                int oldSampleCopyIndex = mSampleCopyIndex;
                mSampleCopyIndex = (((int)timePassed - sampleLength) % Haptic.Clip.Length);
                if (mSampleCopyIndex < 0)
                {
                    mSampleCopyIndex = oldSampleCopyIndex;
                }
                // mStartTime = Time.realtimeSinceStartup - mSampleCopyIndex;
                playHapticPiece(Haptic.Clip, sampleLength);
            }
        }
    }
    #endregion
}
