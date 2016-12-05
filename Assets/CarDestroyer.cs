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

        if (go.tag != "Cars")
        {
            Debug.LogWarning(string.Format("CarDestroyer collided with an object with tag '{0}'... expected 'Cars'",
                go.tag));
            return;
        }


        Debug.Log(string.Format("Unloading Car object at [{0},{1}].", transform.position.x, transform.position.y));
        Destroy(go);
    }
}
