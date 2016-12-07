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

	[Range(1,20)]
	public float searchRadius = 5f;
    public bool showSearchRadius = true;
	public CircleRenderer radiusRenderer;
	private GameObject _waypoint;
	private WaypointNode targetWaypoint = null;
	private int waypointIndex = 0;
	private GameObject stoplight;
	private Status status;
	private float currSpeed;
	private SDN _sdn;

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

			_sdn = GameObject.FindObjectOfType<SDN> ();
			if (_sdn == null) {
				throw new UnassignedReferenceException ("Couldn't find the SDN object!");
			}

		}

		// Add to vehicle list
		_sdn.addToVehicleList(this);

		Debug.Assert (_waypoint != null, "Unable to find any waypoints on initialize!");
		status = Status.Go;
		currSpeed = vehicle.Speed;
		stoplight = null;

        if (showSearchRadius)
        {
            radiusRenderer = GetComponent<CircleRenderer>();
            radiusRenderer.radius = searchRadius;
        }		
	}

	WaypointNode GetNextWaypoint(int currentIndex)
	{
	    WaypointNode nextNode = null;
		try {
			var waypoints = _waypoint.GetComponentsInChildren<WaypointNode>();
		    if (currentIndex < waypoints.Length)
		        nextNode = waypoints[currentIndex];
		} catch(UnityException ue) {
            nextNode = null;
		}
		if (nextNode != null) {
			try {
				Transform temp = nextNode.transform.FindChild("Stoplight");
				if(temp != null) {
					stoplight = temp.gameObject;
				} else {
					stoplight = null;
				}
			} catch(UnityException ue) {
				stoplight = null;
			}
		}

        return nextNode;
	}

	void updateStatus() {
		if (stoplight != null) {
			StoplightBehavior targetScript = stoplight.GetComponent<StoplightBehavior> ();
			int sls = targetScript.getStoplightStatus ();
			switch (sls) {
			// Green
			case 1:
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
		} else {
			status = Status.Go;
		}
	}

	// Update is called once per frame
	void Update () {
        if (targetWaypoint == null)
        {
            targetWaypoint = GetNextWaypoint(waypointIndex);
            if (targetWaypoint == null)
            {
                Debug.Log("Hit the last waypoint. Despawning car.");
				_sdn.removeFromVehicleList (this);
                Destroy(gameObject);
                return;
            }
            targetWaypoint.IsOccupied = true;
        }

        Vector2 dir = targetWaypoint.transform.position - this.transform.localPosition;
        float distThisFrame;

        if (status == Status.Go)
        {
			//if we're close to our max speed (or greater than it), just snap to it
			if (currSpeed >= vehicle.Speed - 0.05f)
				currSpeed = vehicle.Speed;
			else
				currSpeed += vehicle.Acceleration;
			
            distThisFrame = currSpeed * Time.deltaTime;

            if (dir.magnitude <= distThisFrame)
            {
                // waypoint HIT
                var nextWaypoint = GetNextWaypoint(waypointIndex + 1);

                //if we're at the last waypoint, or if there is a next waypoint and it's not occupied,
                //then we want to start moving to the next waypoint. Otherwise just wait here.
                if (nextWaypoint == null || nextWaypoint.IsOccupied == false)
                {
                    //free up this waypoint for others to occupy
                    targetWaypoint.IsOccupied = false;
                    targetWaypoint = null;
                    waypointIndex++;
                    //Debug.Log("Hit the waypoint. Gotta get next one.");
                }
                else if (nextWaypoint.IsOccupied == true)
                {
                    //just wait for it to be unoccupied
                    currSpeed = 0f;
                    //Debug.Log("Hit the waypoint, but next waypoint is occupied so we're waiting.");
                }

                updateStatus();
            }
        }
        else if (dir.magnitude <= 0.1)
        {
            currSpeed = 0f;
        }
        else if (dir.magnitude > 1.7f)
        {
            //go full speed if we've got some ways before the stoplight
			//if we're close to our max speed (or greater than it), just snap to it
			if (currSpeed >= vehicle.Speed - 0.05f)
				currSpeed = vehicle.Speed;
			else
				currSpeed += vehicle.Acceleration;
		}
        else if (status == Status.Slow)
        {
            if (currSpeed > 0.6f)
            {
                currSpeed *= 0.95f;
            }
            else
            {
                currSpeed = 0.6f;
            }
        }
        else if (status == Status.Stop)
        {
            if (currSpeed > 0.6f)
            {
                currSpeed *= 0.98f;
            }
            else
            {
                currSpeed = 0.6f;
            }
        }


        distThisFrame = currSpeed * Time.deltaTime;
        transform.Translate(dir.normalized * distThisFrame, Space.World);
        //turn if we need to turn
        this.transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 0, -1).normalized);
        this.transform.Rotate(new Vector3(-90, 0));
        updateStatus();

    }
}