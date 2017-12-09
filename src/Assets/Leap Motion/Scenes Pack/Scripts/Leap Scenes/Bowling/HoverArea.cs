using UnityEngine;
using System.Collections;

public class HoverArea : MonoBehaviour {

    public float hoverForce = 12;
    public bool up = true;

    void OnTriggerStay(Collider o)
    {
        if (up)
        {
            o.GetComponent<Rigidbody>().AddForce(Vector3.up * hoverForce, ForceMode.Acceleration);
        }
        else
        {
            o.GetComponent<Rigidbody>().AddForce(-Vector3.up * hoverForce, ForceMode.Acceleration);
        }
    }

    void OnTriggerExit(Collider o)
    {
        if (!o.GetComponent<Rigidbody>().isKinematic)
        {
            o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

}
