using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;
using AssemblyCSharp;

public class SDN : MonoBehaviour {

	public List<VehicleBehavior> vehicleList = new List<VehicleBehavior> ();

	public List<VehicleBehavior> ShortestPath = new List<VehicleBehavior> ();

	private TimeSpan timeToRunTest;
	private DateTime lastRun;

	// Use this for initialization
	void Start () {
		lastRun = DateTime.Now;
		timeToRunTest = new TimeSpan (0, 0, 0, 5);
	}

	// Update is called once per frame
	void Update () {
		if (DateTime.Now - lastRun >= timeToRunTest) {
			OnTestTimerElapsed ();
		}
	}

	public bool addToVehicleList(VehicleBehavior vehicle) {
		vehicleList.Add (vehicle);
		return true;
	}

	public bool removeFromVehicleList(VehicleBehavior vehicle) {
		if (vehicleList.Contains (vehicle)) {
			vehicleList.Remove (vehicle);
			return true;
		}
		return false;
	}

	public List<VehicleBehavior> calculatePath(VehicleBehavior vehicleStart, VehicleBehavior vehicleEnd) {
		ShortestPath.Clear ();

		// Deactivate all radius
		foreach (VehicleBehavior vehicle in vehicleList) {
			vehicle.showSearchRadius = false;
		}

		//bfsInfo keeps track of the shortest path
		// Key: The VehicleBehavior (vehicle)
		// Value: Tuple of <float, GameObject> where
		//         float: the distance from vehicleStart to this VehicleBehavior
		//         VehicleBehavior: the prior Vehicle that has the shortest path back to the start node
		var bfsInfo = new Dictionary<VehicleBehavior, Tuple<float, VehicleBehavior>>();

		//add the starting node to our BFS map
		bfsInfo.Add (vehicleStart, new Tuple<float, VehicleBehavior> (0f, null));

		var bfsQueue = new Queue<VehicleBehavior> ();

		//seed the search with the first item
		bfsQueue.Enqueue (vehicleStart);

		while (bfsQueue.Count > 0) {
			var vehicle = bfsQueue.Dequeue ();
			var vehiclesInRange = GetAllVehiclesWithinRadius (vehicle, vehicle.searchRadius);

			var vehicleInfo = bfsInfo [vehicle];

			//go through all nearby vehicles
			foreach (var otherVehicle in vehiclesInRange) {
				var distBtwnVehicles = DistBetweenGameObjects(vehicle.transform, otherVehicle.transform);

				//is this the first time we're vising this node...
				//or did we just find a more efficient path?
				if (!bfsInfo.ContainsKey (otherVehicle)
					|| bfsInfo[otherVehicle].First > distBtwnVehicles + bfsInfo[vehicle].First) {
                    //We just found out that going from 'vehicle' to 'otherVehicle' is more efficient
                    //than whatever we had before, so save this info.
                    bfsInfo[otherVehicle] = new Tuple<float, VehicleBehavior>(distBtwnVehicles + bfsInfo[vehicle].First, vehicle);

					//because we just updated this node, there might be more info to glean from revisiting it,
					//so let's add it back into the queue
                    if (!bfsQueue.Contains(otherVehicle))
					    bfsQueue.Enqueue (otherVehicle);
				}
			}

		}

		Debug.Log ("Done doing BFS.");

		//check if we found a solution
		if (!bfsInfo.ContainsKey (vehicleEnd)) {
			//uh oh, we never encountered the end vehicle, so there's no path!
			Debug.LogWarning("Couldn't find any path between the vehicles!!!");
            DrawPath(ShortestPath);

            return null;
		}

		//Time to traverse the path back to the start node
		var node = vehicleEnd;
		while (node != null) {
			// Activate radius display
			node.showSearchRadius = true;
			ShortestPath.Add (node);
			node = bfsInfo [node].Second;
		}

        ShortestPath.Reverse();
		Debug.Log ("Shortest path involves traversing through " + ShortestPath.Count + " nodes.");
        DrawPath(ShortestPath);

		return ShortestPath;
	}

    private void DrawPath(List<VehicleBehavior> path)
    {
        var line = gameObject.GetComponent<LineRenderer>();
        if (line == null)
            gameObject.AddComponent<LineRenderer>();

        line.SetVertexCount(path.Count);
        line.SetColors(Color.red, Color.black);

        for (int i = 0; i < path.Count; i++)
        {
            line.SetPosition(i, ShortestPath[i].transform.position);
        }

        line.SetWidth(1f, 0.5f);
        line.useWorldSpace = true;
    }

	private List<VehicleBehavior> GetAllVehiclesWithinRadius(VehicleBehavior vehicle, float maxDist)
	{
		var vehiclesInRange = new List<VehicleBehavior> ();

		foreach (var otherVehicle in vehicleList) {
			if (otherVehicle == vehicle)
				continue;

			//check if 'otherVehicle' is within (maxDist) of 'vehicle'
			if (DistBetweenGameObjects(vehicle.transform, otherVehicle.transform) <= maxDist) {
				vehiclesInRange.Add (otherVehicle);
			}
		}

		return vehiclesInRange;
	}

	private void OnTestTimerElapsed()
	{
		if (vehicleList.Count < 8) {
			Debug.Log ("Not enough vehicles to test the pathfinding. Passing.");
			return;
		}

		Debug.Log ("Starting a test run of the pathfinding...");
		var rand = new System.Random();
        var dist = 0;
        int first, second;
        
        //pick two random vehicles
        second = first = rand.Next(vehicleList.Count);
        while (second == first)
            second = rand.Next(vehicleList.Count);
        		
		//test pathfinding
		calculatePath(vehicleList[first], vehicleList[second]);

		lastRun = DateTime.Now;
	}

	private float DistBetweenGameObjects(Transform one, Transform two)
	{
		return ((Vector2)one.position - (Vector2)two.position).magnitude;
	}

}
