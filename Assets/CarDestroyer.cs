using UnityEngine;
using System.Collections;

public class CarDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Trigger: " + GetComponent<Collider2D>().isTrigger);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        var go = other.gameObject;

        Debug.Log("Unloading Car object.");
        Destroy(go);
    }
}
