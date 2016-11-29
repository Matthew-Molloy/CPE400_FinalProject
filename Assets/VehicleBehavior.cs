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

			Vector3 dir = targetWaypoint.position - this.transform.position;
			float distThisFrame = vehicle.Speed * Time.deltaTime;

			if (dir.magnitude <= distThisFrame) {
				targetWaypoint = null;
			} else {
				//move the vehicle
				transform.Translate (dir.normalized * distThisFrame, Space.World);

				//turn if we need to turn
				this.transform.rotation = Quaternion.LookRotation(dir, new Vector3(0,0,-1).normalized);
				this.transform.Rotate (new Vector3 (-90, 0));
			}
		}

	}
}
