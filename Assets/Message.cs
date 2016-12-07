using UnityEngine;
using System.Collections.Generic;
using System;
using AssemblyCSharp;

public class Message : MonoBehaviour {

    public List<VehicleBehavior> Path;
    public int CurrentIndex = 0;

    [HideInInspector]
    public SDN SdnRef;
    [HideInInspector]
    public bool instantiated = false;

    [HideInInspector]
    public float TravelSpeed; //set by SDN on init

    [HideInInspector]
    public int msToHoldBetweenCars; //set by SDN on init

    private TimeSpan TimeToHoldBetweenCars;
    private DateTime TimeArrivedAtLastCar;

    private 
	// Use this for initialization
	void Start () {
        TimeArrivedAtLastCar = DateTime.Now;
        TimeToHoldBetweenCars = new TimeSpan(0, 0, 0, 0, msToHoldBetweenCars);
	}
	
	// Update is called once per frame
	void Update () {
        if (!instantiated) return;

        if (CurrentIndex >= Path.Count)
        {
            Debug.Log("Message has been delivered.");
            SdnRef.MessagesDelivered++;
            SdnRef.CurrentlyRouting = false;
            instantiated = false;

            Destroy(gameObject);
            return;
        }

        var curVehicleTarget = Path[CurrentIndex];
        if (curVehicleTarget == null)
        {
            Debug.LogWarning("Vehicle in path is null! Did we travel too slowly?");
            SdnRef.MessagesLostInTransit++;
            SdnRef.CurrentlyRouting = false;
            instantiated = false;

            Destroy(gameObject);
            return;
        }

        Vector2 dir = curVehicleTarget.transform.position - this.transform.localPosition;
        float distThisFrame = TravelSpeed * Time.deltaTime;
        
        if (dir.magnitude <= distThisFrame)
        {
            if (CurrentIndex == 0 || DateTime.Now - TimeArrivedAtLastCar > TimeToHoldBetweenCars)
            {
                TimeArrivedAtLastCar = DateTime.Now;

                //we've arrived at our current target, let's see if the next car is still in range
                if (Path.Count > CurrentIndex + 1)
                {
                    var nextCar = Path[CurrentIndex + 1];
                    var distToNextCar = nextCar == null ? float.MaxValue : 
                        ((Vector2)(nextCar.transform.position - curVehicleTarget.transform.position)).magnitude;

                    //if (distToNextCar > curVehicleTarget.searchRadius)
                    if (CurrentIndex > 0)
                    {
                        //Debug.LogWarning("The next car is out of range, so we have to recalculate a path...");
                        Path = SdnRef.calculatePath(curVehicleTarget, Path[Path.Count - 1]);
                        if (Path == null)
                        {
                            Debug.LogWarning("Route changed in transit and couldn't find a new suitable route!");
                            SdnRef.MessagesLostInTransit++;
                            Destroy(gameObject);
                        }
                        CurrentIndex = 0;
                        return;
                    }
                }
                CurrentIndex++;
            }
            transform.position = curVehicleTarget.transform.position;
        }
        else
        {
            transform.Translate(dir.normalized * distThisFrame, Space.World);
        }


    }
}
