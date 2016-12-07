using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class SDN : MonoBehaviour {

	public ArrayList vehicleList;
	public ArrayList allPaths;

	// Use this for initialization
	void Start () {
		vehicleList = new ArrayList ();
		allPaths = new ArrayList ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void calculatePaths(GameObject vehicleStart, GameObject vehicleEnd) {

	}

	public UtilityTuple calculatePath(ArrayList path, GameObject vehicleStart, GameObject vehicleEnd) {

		UtilityTuple result = null;

		// get position of both vehicles
		VehicleBehavior vb1 = vehicleStart.GetComponent<VehicleBehavior> ();
		int radius = vb1.vehicle.radius;

		// see which vehicles are within radius range
		foreach (GameObject vehicle in vehicleList) {
			
			// check if its the same vehicle before proceeding
			if (vehicle.GetInstanceID () != vehicleStart.GetInstanceID ()) {
				VehicleBehavior vb2 = vehicle.GetComponent<VehicleBehavior> ();

				// Calculate distance
				Vector2 dir = vehicleStart.transform.position - vehicle.transform.position;
				float dist = dir.magnitude;

				// Check if within range
				if (dist <= radius) {

					// If destination reachable
					if (vehicle.GetInstanceID () == vehicleEnd.GetInstanceID ()) {
						path.Add (vehicle);
						return new UtilityTuple (dist, vehicle);
					}
						
					result = calculatePath(vehicle, vehicleEnd);
					result.dist = result.dist + dist;
					path.Add (vehicle);
				}
			}
		}

		return result;
	}

	public static bool addToVehicleList(GameObject vehicle) {
		vehicleList.Add (vehicle);
		return true;
	}

	public static bool removeFromVehicleList(GameObject vehicle) {
		if (vehicleList.Contains (vehicle)) {
			vehicleList.Remove (vehicle);
			return true;
		}
		return false;
	}

}
