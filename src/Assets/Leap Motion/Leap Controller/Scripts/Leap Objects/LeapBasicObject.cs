using UnityEngine;
using System.Collections;

/// <summary>
/// Basic Leap Game Object, dropped when released
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class LeapBasicObject : LeapGameObject {

    public override LeapState Activate(HandTypeBase h)
    {
        if (owner != null)
            return null;

        if (canGoThroughGeometry && GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
        }
        return base.Activate(h);
    }

    public override LeapState Release(HandTypeBase h)
    {
        LeapState state = null;

        if (!isStatePersistent)
        {
            if (canGoThroughGeometry && GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
            }

            state = base.Release(h);
        }

        return state;
    }

    public override void UpdateTransform(HandTypeBase t)
    {
        base.UpdateTransform(t);

        if (!GetComponent<Rigidbody>().isKinematic)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // Necessary to switch Hand Updates for collisions
            if (collisionOccurred)
            {
                owner.unityHand.runUpdate = false;
            }
            else
            {
                owner.unityHand.runUpdate = true;
            }
        }
    }
}
