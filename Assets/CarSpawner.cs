using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class CarSpawner : MonoBehaviour
{
    public float SpawnChance;
    public GameObject ObjectToSpawn;

    private Collider2D _myCollider;
    private readonly ArrayList _currentlyCollidingObjects = new ArrayList();

	// Use this for initialization
	void Start ()
	{
	    _myCollider = GetComponent<Collider2D>();
        Assert.IsNotNull(_myCollider, "Unable to find Collider2D component for spawner!");
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate()
    {
        //randomly spawn cars, but only if there's not already a car colliding with the spawner
        if (((float)Random.Range(0, 100)) / 100f < SpawnChance && _currentlyCollidingObjects.Count == 0)
        {
            SpawnCar();
        }
    }

    /// <summary>
    ///  Spawns a car at the spawner's position.
    /// </summary>
    void SpawnCar()
    {
        Debug.Log("Spawning a new car.");
        var myPos = transform.position;

        //TODO: handle the rotation of spawned object
        Instantiate(ObjectToSpawn, new Vector3(myPos.x, myPos.y, myPos.z),
            this.transform.localRotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _currentlyCollidingObjects.Add(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (_currentlyCollidingObjects.Contains(other.gameObject))
            _currentlyCollidingObjects.Remove(other.gameObject);
    }
}
