using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class StoplightBehavior : MonoBehaviour {

	public Stoplight stoplight = new Stoplight ();
	int redTimer;
	int yellowTimer;
	int greenTimer;

	private Color _currentColor{
		get{
			return gameObject.GetComponent<Renderer> ().material.color;
		}
		set{
			gameObject.GetComponent<Renderer> ().material.color = value;
		}
	}

	// Use this for initialization
	void Start () {
		stoplight.Status = Stoplight.Light.Yellow;
		_currentColor = Color.yellow;
		resetTimers ();
		Debug.Log ("Light: Yellow");
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	public int getStoplightStatus() {
		if (stoplight.Status == Stoplight.Light.Green) {
			return 1;
		}
		if (stoplight.Status == Stoplight.Light.Yellow) {
			return 0;
		}
		return -1;
	}

	void resetTimers() {
		redTimer = 0;
		yellowTimer = 0;
		greenTimer = 0;
	}

	void FixedUpdate ()
	{
		if (stoplight.Status == Stoplight.Light.Red) {
			//Debug.Log ("updating Red");
			redTimer++;
			if (redTimer > 500) {
				stoplight.Status = Stoplight.Light.Green;
				_currentColor = Color.green;
				Debug.Log ("Light: Green");
				resetTimers ();
			}
		} else if (stoplight.Status == Stoplight.Light.Yellow) {
			//Debug.Log ("updating Yellow");
			yellowTimer++;
			if (yellowTimer > 150) {
				stoplight.Status = Stoplight.Light.Red;
				_currentColor = Color.red;
				Debug.Log ("Light: Red");
				resetTimers ();
			}
		} else if (stoplight.Status == Stoplight.Light.Green) {
			//Debug.Log ("updating Green");
			greenTimer++;
			if (greenTimer > 500) {
				stoplight.Status = Stoplight.Light.Yellow;
				_currentColor = Color.yellow;
				Debug.Log ("Light: Yellow");
				resetTimers ();
			}
		}
	}
}
