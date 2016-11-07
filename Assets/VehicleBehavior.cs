using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class VehicleBehavior : MonoBehaviour {

	public Vehicle vehicle = new Vehicle();
	GameObject waypoint;
	Transform targetWaypoint;
	int waypointIndex = 0;

	// Use this for initialization
	void Start () {
		waypoint = GameObject.Find ("Waypoint");
	}

	void GetNextWaypoint() {
		try {
			targetWaypoint = waypoint.transform.GetChild (waypointIndex);
		} catch(UnityException ue) {
			targetWaypoint = null;
		}
		waypointIndex++;
	}
	
	// Update is called once per frame
	void Update () {
		bool done = false;
		if (targetWaypoint == null) {
			GetNextWaypoint ();
			if (targetWaypoint == null) {
				Destroy (gameObject);
				done = true;
			}
		}

		if (done == false) {

			Vector2 dir = targetWaypoint.position - this.transform.localPosition;
			float distThisFrame = vehicle.Speed * Time.deltaTime;

			if (dir.magnitude <= distThisFrame) {
				targetWaypoint = null;
			} else {
				transform.Translate (dir.normalized * distThisFrame);
				//Quaternion targetRotation = Quaternion.LookRotation (dir);
				//this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 1);
			}
		}

	}
}
