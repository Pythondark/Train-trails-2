using UnityEngine;

public abstract class HapticsAbstractClass : MonoBehaviour {
    protected static readonly System.Random RANDOM = new System.Random();
    protected int eventID = -1;

    protected virtual void DoRumble(int id, HapticsClip hapticsClip, bool mix = false)
    {
        eventID = id;
    }

    public void transmitRumble(int id, HapticsClip hapticsClip, GameObject sendingObject, bool mix = false)
    {
        if (eventID != id)
        {
            DoRumble(id, hapticsClip, mix);
        }
    }

    //public override bool Equals(object other)
    //{
    //    if (other == null)
    //    {
    //        return false;
    //    }
    //    return base.GetHashCode().Equals(other.GetHashCode());
    //}
}
