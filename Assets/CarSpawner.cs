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
        if (((float)Random.Range(0, 10000)) / 10000f < SpawnChance && _currentlyCollidingObjects.Count == 0)
        {
            SpawnCar();
        }
    }

    /// <summary>
    ///  Spawns a car at the spawner's position.
    /// </summary>
    void SpawnCar()
    {
        var myPos = transform.position;

        var newObj = Instantiate(ObjectToSpawn, new Vector3(myPos.x, myPos.y, myPos.z),
            this.transform.localRotation);

		//Debug.Log(string.Format("Spawning a new car at [{0},{1}] with ID '{2}'.", myPos.x, myPos.y, (uint)newObj.GetInstanceID()));

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
