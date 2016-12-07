using UnityEngine;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class SDN : MonoBehaviour {

	public List<VehicleBehavior> vehicleList = new List<VehicleBehavior> ();

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	
	}

	public List<VehicleBehavior> calculatePath(VehicleBehavior vehicleStart, VehicleBehavior vehicleEnd) {

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
			var maxRadius = vehicle.GetComponent<Vehicle> ().radius;
			var vehiclesInRange = GetAllVehiclesWithinRadius (vehicle, maxRadius);

			var vehicleInfo = bfsInfo [vehicle];

			//go through all nearby vehicles
			foreach (var otherVehicle in vehiclesInRange) {
				var distBtwnVehicles = DistBetweenGameObjects(vehicle.gameObject, otherVehicle.gameObject);

				//is this the first time we're vising this node...
				//or did we just find a more efficient path?
				if (!bfsInfo.ContainsKey (otherVehicle)
					|| bfsInfo[otherVehicle].First > distBtwnVehicles) {
					//We just found out that going from 'vehicle' to 'otherVehicle' is more efficient
					//than whatever we had before, so save this info.
					bfsInfo [otherVehicle] = new Tuple<float, VehicleBehavior> (distBtwnVehicles, vehicle);

					//because we just updated this node, there might be more info to glean from revisiting it,
					//so let's add it back into the queue
					bfsQueue.Enqueue (otherVehicle);
				}
			}

		}

		//check if we found a solution
		if (!bfsInfo.ContainsKey (vehicleEnd)) {
			//uh oh, we never encountered the end vehicle, so there's no path!
			Debug.LogWarning("Couldn't find any path between the vehicles!!!");

			return null;
		}

		//Time to traverse the path back to the start node
		var shortestPath = new List<VehicleBehavior>();
		var node = vehicleEnd;
		while (node != vehicleStart) {
			shortestPath.Add (node);
			node = bfsInfo [node].Second;
		}

		Debug.Log ("Shortest path involves traversing through " + shortestPath.Count + " nodes.");
		return shortestPath;
	}

	private List<VehicleBehavior> GetAllVehiclesWithinRadius(VehicleBehavior vehicle, float maxDist)
	{
		var vehiclesInRange = new List<VehicleBehavior> ();

		foreach (var otherVehicle in vehicleList) {
			if (otherVehicle == vehicle)
				continue;

			//check if 'otherVehicle' is within (maxDist) of 'vehicle'
			if (DistBetweenGameObjects(vehicle.gameObject, otherVehicle.gameObject) <= maxDist) {
				vehiclesInRange.Add (otherVehicle);
			}
		}

		return vehiclesInRange;
	}

	private float DistBetweenGameObjects(GameObject one, GameObject two)
	{
		return ((Vector2)one.transform.position - (Vector2)two.transform.position).magnitude;
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

}
