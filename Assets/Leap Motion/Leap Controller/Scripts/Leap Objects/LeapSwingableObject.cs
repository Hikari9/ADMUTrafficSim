using UnityEngine;
using System.Collections;
using Leap;

/// <summary>
/// Swingable Sword object
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class LeapSwingableObject : LeapGameObject
{
    public TrailRenderer swipe;

    protected override void Start()
    {
        base.Start();

        TrailRenderer trail = GetComponent<TrailRenderer>();

        if (trail && !swipe)
        {
            swipe = trail;
        }
    }

    public override LeapState Activate(HandTypeBase h)
    {
        if (owner != null)
            return null;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Collider>().enabled = false;

        return new LeapSwingableState(this);
    }

    public override LeapState Release(HandTypeBase h)
    {
        LeapState state = null;

        if (!isStatePersistent)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Collider>().enabled = true;

            state = base.Release(h);
        }

        return state;
    }

    public void CheckSwipe()
    {
        if (swipe)
        {
            swipe.enabled = (owner.unityHand.hand.PalmVelocity.ToUnityTranslated().magnitude > 18);

            if (swipe.enabled)
            {
                GetComponent<Collider>().enabled = true;
            }
            else
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
