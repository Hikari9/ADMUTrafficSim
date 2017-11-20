using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsupervisedIntersectionAgent : MonoBehaviour {

    public Color go = Color.green;
    public Color slow = Color.yellow;
    public Color stop = Color.red;

    GameObject car;
    CarMovement movement { get { return car.GetComponent<CarMovement>(); } }
    public MeshRenderer renderer { get { return GetComponent<MeshRenderer>(); } }

    float lastUpdateTime;
    bool stopSignal;
    int slowSignal;

    // Use this for initialization
    void Start() {
        car = transform.parent.gameObject;
        renderer.material.color = go;
        lastUpdateTime = Time.time;
        stopSignal = false;
        slowSignal = 0;
    }

	// Update is called once per frame
	void Update () {
        Color target;
        if (movement.carsAcross.Count > 0 && movement.targetVelocity.Equals(Vector3.zero))
            target = stop;
        else {
            // go if all other colliders are stopped
            target = go;
            foreach (Transform car in carsToWatchOutFor.Keys) {
                UnsupervisedIntersectionAgent agent = car.GetComponent<UnsupervisedIntersectionAgent>();
                if (agent.renderer.material.color != agent.stop) {
                    target = slow;
                    break;
                }
            }
        }
        renderer.material.color = target;
	}

    Dictionary<Transform, bool> carsToWatchOutFor = new Dictionary<Transform, bool>();
    
    void OnTriggerEnter(Collider collision) {
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        if (CarMovement.OrthogonalCars(car1.parent, car2.parent) && !carsToWatchOutFor.ContainsKey(car2)) {
            carsToWatchOutFor.Add(car2, true);
        }
    }

    void OnTriggerExit(Collider collision) {
        Transform car1 = this.transform;
        Transform car2 = collision.transform;
        if (carsToWatchOutFor.ContainsKey(car2)) {
            carsToWatchOutFor.Remove(car2);
        }
    }
}
