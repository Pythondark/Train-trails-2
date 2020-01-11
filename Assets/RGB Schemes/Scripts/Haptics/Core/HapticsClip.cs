using UnityEngine;
using System.Threading;

public class HapticsClip
{
    private readonly Thread mThread;

    private byte[] mSamples;
    private int mLength;

    public int Length
    {
        get
        {
            if (mThread == null || !mThread.IsAlive)
            {
                return mLength;
            }
            return 0;
        }
        private set
        {
            mLength = value;
        }
    }

    public byte[] Samples
    {
        get
        {
            if (mThread == null || !mThread.IsAlive)
            {
                return mSamples;
            }
            return new byte[0];
        }
        private set
        {
            mSamples = value;
        }
    }

    public HapticsClip()
    {
        mSamples = new byte[0];
        mLength = 0;
    }

    public HapticsClip(byte[] samples, int samplesCount)
    {
        Samples = samples;
        Length = (samplesCount >= 0) ? samplesCount : 0;
    }

    public HapticsClip(AudioClip audioClip, bool loadSeparateThread = false, int channel = 0)
    {
        /*
         * None of the below can be done outside of the main thread because Unity
         * doesn't like calls being done on separate threads. This should be fine
         * since most of these calls **should** be done quickly. Long term a better
         * solution will be needed however.
         */
        uint controllerSampleRate = HapticsUtils.SampleRateHz;
        int channelCount = audioClip.channels;
        float frequency = audioClip.frequency;
        float[] audioData = new float[audioClip.samples * channelCount];
        audioClip.GetData(audioData, 0);

        if (loadSeparateThread)
        {
            mThread = new Thread(() => LoadAudio(channelCount, audioData, frequency, channel, controllerSampleRate));
            if (audioClip.loadState == AudioDataLoadState.Loaded)
            {
                mThread.Start();
            }
        }
        else
        {
            LoadAudio(channelCount, audioData, frequency, channel, controllerSampleRate);
        }
    }

    private void LoadAudio(int channelCount, float[] audioData, float frequency, int channel, uint controllerSampleRate)
    {
        // Start by getting our audio data.
        Length = 0;

        // Next get our step size precision and calculate how large the step should
        // be. Need to ensure that our step size is at least 1 so as not to
        // copy no data and hit an infinite loop.
        double stepSizePrecision = (frequency + 1e-6) / controllerSampleRate;

        if (stepSizePrecision >= 1.0)
        {
            // We're going to accumulate an error when using doubles due to
            // the precision, so let's keep track of this and ensure that we
            // account for it.
            int stepSize = (int)stepSizePrecision;
            double stepSizeError = stepSizePrecision - stepSize;
            double accumulatedStepSizeError = 0.0f;

            // Setup our Haptic Clip properties.
            Samples = new byte[audioData.Length / channelCount / stepSize + 1];

            // Copy over all of the data from the audio clip, ensure that we
            // clamp the maximum value of a byte.
            int position = channel % channelCount;
            while (position < audioData.Length)
            {
                if (Length < Samples.Length)
                {
                    // Does not support multi-byte channels.
                    Samples[Length] =
                        (byte)(Mathf.Clamp01(Mathf.Abs(audioData[position])) * byte.MaxValue);
                    Length += 1;
                }
                position += stepSize * channelCount;
                accumulatedStepSizeError += stepSizeError;
                if ((int)accumulatedStepSizeError > 0)
                {
                    position += (int)accumulatedStepSizeError * channelCount;
                    accumulatedStepSizeError -= (int)accumulatedStepSizeError;
                }
            }
        }
    }
}
