using UnityEngine;
using System.Collections;
using System;
using AssemblyCSharp;

public class SDN : MonoBehaviour {

	public ArrayList vehicleList;

	// Use this for initialization
	void Start () {
		vehicleList = new ArrayList ();
	}

	// Update is called once per frame
	void Update () {
	
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
