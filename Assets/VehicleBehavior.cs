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
	GameObject waypoint;
	Transform targetWaypoint;
	int waypointIndex = 0;
	GameObject stoplight;
	Status status;
	float currSpeed;
	bool slightAvail;

	// Use this for initialization
	void Start () {
		waypoint = GameObject.Find ("Waypoint");
		status = Status.Go;
		currSpeed = vehicle.Speed;
		stoplight = null;
		slightAvail = true;
	}

	void GetNextWaypoint() {
		try {
			targetWaypoint = waypoint.transform.GetChild (waypointIndex);
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
			float distThisFrame = currSpeed * Time.deltaTime;

			if (status == Status.Go) {
				currSpeed = vehicle.Speed;
				if (dir.magnitude <= distThisFrame) {
					// waypoint HIT
					targetWaypoint = null;
					if (slightAvail == true) {
						stoplight = GameObject.Find ("Stoplight");
						slightAvail = false;
					}
					updateStatus ();
				} else {
					transform.Translate (dir.normalized * distThisFrame);
				}
			}

			else if (status == Status.Slow) {
				if (currSpeed > 0f) {
					currSpeed -= 0.5f;
				} else {
					currSpeed = 0f;
				}
				distThisFrame = currSpeed * Time.deltaTime;
				transform.Translate (dir.normalized * distThisFrame);
			}

			else if (status == Status.Stop) {
				currSpeed = 0f;
				distThisFrame = currSpeed * Time.deltaTime;
				transform.Translate (dir.normalized * distThisFrame);
			}
			updateStatus ();
		}
	}
}