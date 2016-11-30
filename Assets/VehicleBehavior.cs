using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class VehicleBehavior : MonoBehaviour {

	public enum Status
	{
		Go,
		Slow,
		Stop
	}

	public Vehicle vehicle = new Vehicle();
	GameObject _waypoint;
	Transform targetWaypoint = null;
	int waypointIndex = 0;
	GameObject stoplight;
	Status status;
	float currSpeed;
	bool slightAvail;

	// Use this for initialization
	void Start () {
		var allWaypoints = GameObject.FindGameObjectsWithTag ("Waypoints");
		var closestDist = Mathf.Infinity;
		if (allWaypoints.Length == 0)
			Debug.LogError ("Couldn't find any waypoints!");
		
		foreach (var waypoint in allWaypoints) {
			var distance = Vector2.Distance (this.transform.position, waypoint.transform.position);
			if (distance < closestDist) {
				closestDist = distance;
				_waypoint = waypoint;
			}
		}

		Debug.Assert (_waypoint != null, "Unable to find any waypoints on initialize!");
		status = Status.Go;
		currSpeed = vehicle.Speed;
		stoplight = null;
		slightAvail = true;
	}

	void GetNextWaypoint() {
		try {
			targetWaypoint = _waypoint.transform.GetChild (waypointIndex);
		} catch(UnityException ue) {
			targetWaypoint = null;
		}
		waypointIndex++;
	}

	void updateStatus() {
		if (stoplight != null) {
			StoplightBehavior targetScript = stoplight.GetComponent<StoplightBehavior> ();
			int sls = targetScript.getStoplightStatus ();
			switch (sls) 
			{
				// Green
				case 1:
					if (status != Status.Go) {
						stoplight = null;
					}
					status = Status.Go;
					break;
				// Yellow
				case 0:
					status = Status.Slow;
					break;
				// Red
				case -1:
					status = Status.Stop;
					break;
			}
		}
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
			float distThisFrame;

			if (status == Status.Go) {
				currSpeed = vehicle.Speed;
				distThisFrame = currSpeed * Time.deltaTime;

				if (dir.magnitude <= distThisFrame) {
					// waypoint HIT
					targetWaypoint = null;
					Debug.Log ("Hit the waypoint. Gotta get next one.");
					if (slightAvail == true) {
						stoplight = GameObject.Find ("Stoplight");
						slightAvail = false;
					}
					updateStatus ();
				}
			}

			else if (status == Status.Slow) {
				if (currSpeed > 0f) {
					currSpeed -= 0.5f;
				} else {
					currSpeed = 0f;
				}
			}

			else if (status == Status.Stop) {
				currSpeed = 0f;
			}

			distThisFrame = currSpeed * Time.deltaTime;
			transform.Translate (dir.normalized * distThisFrame, Space.World);
			//turn if we need to turn
			this.transform.rotation = Quaternion.LookRotation(dir, new Vector3(0,0,-1).normalized);
			this.transform.Rotate (new Vector3 (-90, 0));
			updateStatus ();
		}
	}
}