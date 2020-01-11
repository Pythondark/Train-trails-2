using System;
using UnityEngine;

[Serializable]
public class CollisionHaptics : StandardHapticsBase
{
    public bool TransferImmediate;

    /// <summary>
    /// For the collision haptics, we want to transmit our haptic each time we have a collision.
    /// </summary>
    /// <param name="collision">The collision event that occured.</param>
    /// 
    new private void OnCollisionEnter(Collision collision)
    {
        HapticsAbstractClass hapticsAbstractClass = collision.gameObject.GetComponent<HapticsAbstractClass>();
        if (hapticsAbstractClass != null)
        {
            mHapticNodes.Add(hapticsAbstractClass);
            if (TransferImmediate)
            {
                eventID = RANDOM.Next();
                hapticsAbstractClass.transmitRumble(eventID, Haptic.Clip, gameObject);
            }
            else
            {
                mPlaying = true;
                mStartTime = Time.realtimeSinceStartup;
            }
            //Debug.Log("Added object " + hapticsAbstractClass + " to " + gameObject + " with id of " + eventID);
        }
    }
}
