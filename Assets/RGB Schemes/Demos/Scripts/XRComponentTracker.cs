using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRComponentTracker : MonoBehaviour
{
    public Rigidbody LeftHand, RightHand;
    public float Height = 1.7f;

    private List<XRNodeState> mNodeStates = new List<XRNodeState>();
    private Vector3 mLeftHandPos, mRightHandPos;
    private Quaternion mLeftHandRot, mRightHandRot;

    // FixedUpdate stays in sync with the physics engine.
    void FixedUpdate()
    {
        InputTracking.GetNodeStates(mNodeStates);

        foreach (XRNodeState nodeState in mNodeStates)
        {
            switch (nodeState.nodeType)
            {
                case XRNode.LeftHand:
                    nodeState.TryGetPosition(out mLeftHandPos);
                    nodeState.TryGetRotation(out mLeftHandRot);
                    break;
                case XRNode.RightHand:
                    nodeState.TryGetPosition(out mRightHandPos);
                    nodeState.TryGetRotation(out mRightHandRot);
                    break;
            }
        }

        LeftHand.MovePosition(mLeftHandPos + (Vector3.up * Height));
        LeftHand.MoveRotation(mLeftHandRot.normalized);
        RightHand.MovePosition(mRightHandPos + (Vector3.up * Height));
        RightHand.MoveRotation(mRightHandRot.normalized);
    }
}
