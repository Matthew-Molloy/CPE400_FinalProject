using UnityEngine;
using System.Collections;

public class WaypointNode : MonoBehaviour
{
    private bool _isOccupied = false;
    public bool IsOccupied
    {
        get
        {
            return _isOccupied;
        }
        set
        {
            _isOccupied = value;

            var color = value ? Color.red : Color.green;
            gameObject.GetComponent<Renderer>().material.color = color;
        }
    }

    // Use this for initialization
	void Start ()
	{
        IsOccupied = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
